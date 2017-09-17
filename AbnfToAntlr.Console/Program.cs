/*

    Copyright 2012-2013 Robert Pinchbeck
  
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
  
    The solution can be built with Visual Studio 2010 or a compatible product like 
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


    The following articles were instrumental in developing this solution...

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

    Augmented BNF for Syntax Specifications (RFC 5234)
    http://tools.ietf.org/html/rfc5234

    Official character names from Unicode.org
    http://www.unicode.org/charts/PDF/U0000.pdf

    --------------------------------------------------------------------------------
*/

using System;
using System.Collections.Generic;
using System.Text;

using AbnfToAntlr.Common;

[assembly: CLSCompliant(true)]

namespace AbnfToAntlr.Console
{
    class Program
    {
        static void ShowSyntax()
        {
            System.Console.Error.WriteLine("Usage: AbnfToAntlr [--stdin | FILE]");
            System.Console.Error.WriteLine("Translate FILE to ANTLR format and write the results to standard output.");
            System.Console.Error.WriteLine("If --stdin is specified instead of FILE, then standard input is used.");
            System.Console.Error.WriteLine("Example: AbnfToAntlr \"AbnfGrammar.txt\" >AntlrGrammar.g");
        }

        static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                ShowSyntax();
                return 1;
            }

            if (args.Length == 1)
            {
                switch (args[0])
                {
                    case "-h":
                    case "/h":
                    case "-?":
                    case "/?":
                    case "-help":
                    case "/help":
                    case "--help":
                        ShowSyntax();
                        return 1;
                }
            }

            string path = null;
            System.IO.TextReader reader;
            string input;
            string output;

            try
            {
                // open input stream
                if (args[0] == "--stdin")
                {
                    path = "stdin";
                    reader = System.Console.In;
                }
                else
                {
                    path = args[0];
                    reader = new System.IO.StreamReader(path);
                }

                input = reader.ReadToEnd();
                if (input.EndsWith("\r\n"))
                {
                    // do nothing
                }
                else
                {
                    input = input + "\r\n";
                }

                var translator = new AbnfToAntlrTranslator();

                output = translator.Translate(input);

                System.Console.Write(output);

#if DEBUG
                // when debugging, output the resulting string builder to a file
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.IO.File.WriteAllText(@"..\..\_AbnfToAntlr_Debug_Output.txt", output);
                }
#endif
            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine(string.Format("An error occurred while processing '{0}':", path));
                System.Console.Error.WriteLine(ex.Message);
                return 1;
            }

            return 0;
        }

    } // class
} // namespace
