/*

    Copyright 2012-2020 Robert Pinchbeck
  
    This file is part of AbnfToAntlr.

    AbnfToAntlr is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    AbnfToAntlr is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with AbnfToAntlr.  If not, see <http://www.gnu.org/licenses/>.

*/

/*
    --------------------------------------------------------------------------------
    AbnfToAntlr - Translate any ABNF grammar to an ANTLR grammar

    This solution will convert any ABNF grammar into an ANTLR grammar.

    The resulting ANTLR grammar should be syntactically correct; however, 
    some ABNF grammars are inherently ambiguous and ANTLR will complain about them 
    until the ambiguity is resolved by the user.
  
    The solution can be built with Visual Studio or a compatible product like 
    SharpDevelop.  Open the solution, build it, and use the resulting executable to 
    translate any ABNF grammar to an ANTLR grammar.

    The translator makes the following corrections automatically:
  
        1. Dashes in ABNF rule names are replaced with underscores.
        2. ABNF rule names which are also ANTLR keywords are given a numeric suffix (e.g., fragment_1).
        3. Duplicate rule names are given a numeric suffix to make them unique (e.g., rule_1, rule_2)
        4. All rule names are converted to lowercase (parser rules).
        5. By default, all character literals are converted into lexer rules (resulting in a scannerless parser).

    Happy translating!

    -=Robert Pinchbeck

    http://www.robertpinchbeck.com


    The following articles were instrumental in developing this solution:

    How To Use Antlr With Visual Studio 2010...
    http://techblog.adrianlowdon.co.uk/tag/antlr/

    Translators Should Use Tree Grammars...
    http://antlr.org/article/1100569809276/use.tree.grammars.tml

    Manual Tree Walking Is Better Than Tree Grammars...
    http://www.antlr.org/article/1170602723163/treewalkers.html

    Why I Don't Use StringTemplate For Language Translation...
    http://antlr.org/article/1136917339929/stringTemplate.html

    Preserving whitespace during translation...
    http://www.antlr.org/article/whitespace/index.html

    Augmented BNF for Syntax Specifications (RFC 5234)...
    http://tools.ietf.org/html/rfc5234

    Case-Sensitive String Support in ABNF (RFC 7405)...
    https://tools.ietf.org/html/rfc7405

    Errata 5334...
    https://www.rfc-editor.org/errata/eid5334

    Official character names from Unicode.org...
    http://www.unicode.org/charts/PDF/U0000.pdf

    --------------------------------------------------------------------------------
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Antlr.Runtime;
using Antlr.Runtime.Tree;

using System.IO;

namespace AbnfToAntlr.Common
{
    /// <summary>
    /// Translates ABNF grammars to ANTLR grammars
    /// </summary>
    /// <remarks>
    /// The translator makes the following corrections automatically:
    /// 1. Dashes in ABNF rule names are replaced with underscores.
    /// 2. ABNF rule names which are also ANTLR keywords are given a numeric suffix (e.g., fragment_1).
    /// 3. Duplicate rule names are given a numeric suffix to make them unique (e.g., rule_1, rule_2)
    /// 4. All rule names are converted to lowercase (parser rules).
    /// 5. By default, all character literals are converted into lexer rules (resulting in a scannerless parser).
    /// </remarks>
    public class AbnfToAntlrTranslator
    {
        object SyncLock = new object();                      // stop multiple threads from interfering with each other
        Dictionary<char, NamedCharacter> LiteralsCollection; // literals collection

        /// <summary>
        /// Translate ABNF grammar to ANTLR grammar
        /// </summary>
        /// <param name="input">TextReader which reads the ABNF grammar</param>
        /// <param name="output">TextWriter which writes the ANTLR grammar</param>
        public void Translate(TextReader input, TextWriter writer, bool performDirectTranslation)
        {
            // force single threaded operation
            lock (SyncLock)
            {
                this.LiteralsCollection = new Dictionary<char, NamedCharacter>();

                // open input stream
                var stream = new Antlr.Runtime.ANTLRReaderStream(input);

                // create lexer
                var lexer = new AbnfAstLexer(stream);

                // get token stream from input stream
                var tokens = new CommonTokenStream(lexer);

                // create parser
                var parser = new AbnfAstParser(tokens);

                // parse token stream
                var results = parser.start();

                if (parser.RecognitionExceptions.Count > 0 || lexer.RecognitionExceptions.Count > 0)
                {
                    var message =
                        AntlrHelper.GetErrorMessages(parser.RecognitionExceptions)
                        + AntlrHelper.GetErrorMessages(lexer.RecognitionExceptions)
                        ;

                    throw new TranslationException(message, parser.RecognitionExceptions, lexer.RecognitionExceptions);
                }

                // get parse tree
                var tree = results.Tree;

                // Use simplified named characters for indirect translation
                var lookup = new NamedCharacterLookupSimple();

                // enable this line to use Unicode named characters for indirect translation
                // var lookup = new NamedCharacterLookupUnicode();

                var ruleStatistics = new RuleStatistics();
                var statisticsVisitor = new TreeVisitor_GatherRuleStatistics(ruleStatistics);
                statisticsVisitor.Visit(tree);

                // output translated grammar
                if (performDirectTranslation)
                {
                    OutputDirectTranslation(writer, tokens, tree, lookup, ruleStatistics);
                }
                else
                {
                    OutputIndirectTranslation(writer, tokens, tree, lookup, ruleStatistics);
                }
            }
        }

        /// <summary>
        /// Translate ABNF grammar to ANTLR grammar
        /// </summary>
        /// <param name="abnfGrammar">ABNF grammar to translate</param>
        /// <returns>ANTLR grammar</returns>
        public string Translate(string abnfGrammar, bool performDirectTranslation = false)
        {
            var input = new StringReader(abnfGrammar);
            var output = new System.IO.StringWriter();

            Translate(input, output, performDirectTranslation);

            // return the output
            return output.ToString();
        }


        void OutputDirectTranslation(TextWriter writer, CommonTokenStream tokens, CommonTree tree, INamedCharacterLookup lookup, RuleStatistics ruleStatistics)
        {
            // output ANTLR translation
            var outputVisitor = new TreeVisitor_OutputTranslation_Direct(tokens, writer, lookup, ruleStatistics);
            outputVisitor.Visit(tree);
        }

        void OutputIndirectTranslation(TextWriter writer, CommonTokenStream tokens, CommonTree tree, INamedCharacterLookup lookup, RuleStatistics ruleStatistics)
        {
            // gather distinct literals
            var distinctCharacters = new Dictionary<char, NamedCharacter>();
            var literalVisitor = new TreeVisitor_GatherDistinctCharacters(distinctCharacters, lookup, ruleStatistics);
            literalVisitor.Visit(tree);

            // output ANTLR translation (substitute rules for character literals)
            var outputVisitor = new TreeVisitor_OutputTranslation_Indirect(tokens, writer, lookup, ruleStatistics, distinctCharacters);
            outputVisitor.Visit(tree);

            // append literal rules to output
            OutputLiteralRules(distinctCharacters, writer, lookup);
        }

        void OutputLiteralRules(IDictionary<char, NamedCharacter> literals, TextWriter writer, INamedCharacterLookup lookup)
        {
            var knownValues = 
                literals.Values
                .Where(x => lookup.IsKnownCharacter(x.Character))
                .OrderBy(x => x.Character)
                .Select(x => x);

            var unknownValues = 
                literals.Values
                .Where(x => !(lookup.IsKnownCharacter(x.Character)))
                .OrderBy(x => x.Character)
                .Select(x => x);

            if (literals.Count > 0)
            {
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine(@"////////////////////////////////////////////////////////////////////////////////////////////");
                writer.WriteLine(@"// Lexer rules generated for each distinct character in original grammar");
                writer.WriteLine(@"// " + lookup.Description);
                writer.WriteLine(@"////////////////////////////////////////////////////////////////////////////////////////////");
                writer.WriteLine("");
            }

            // output known (named) literals first
            foreach (var value in knownValues)
            {
                writer.Write(value.Name);
                writer.Write(" : ");

                writer.Write("'");

                var character = value.Character;

                writer.Write(AntlrHelper.CharEscape(character));

                writer.Write("'");

                writer.WriteLine(";");
            }

            // output unknown literals
            foreach (var value in unknownValues)
            {
                writer.Write(value.Name);
                writer.Write(" : ");

                int number = value.Character;

                writer.Write(@"'\u");
                writer.Write(number.ToString("X4"));
                writer.Write("'");

                writer.WriteLine(";");
            }
        }

    } // class
} // namespace