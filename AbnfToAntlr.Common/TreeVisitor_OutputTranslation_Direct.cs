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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Antlr.Runtime;
using Antlr.Runtime.Tree;

namespace AbnfToAntlr.Common
{
    /// <summary>
    /// Output the direct translation of the ABNF grammar (Do not substitute lexer rules for each distinct character value in the original grammar).
    /// </summary>
    public class TreeVisitor_OutputTranslation_Direct : TreeVisitor_OutputTranslation
    {
        public TreeVisitor_OutputTranslation_Direct(ITokenStream tokens, System.IO.TextWriter writer, INamedCharacterLookup lookup, RuleStatistics ruleStatistics)
            : base(tokens, writer, lookup, ruleStatistics)
        {
        }

        /// <summary>
        /// Output the ANTLR translation of the specified rule list
        /// </summary>
        protected override void VisitRuleList(ITree node)
        {
            Reset();

            VisitChildren(node);
        }

        /// <summary>
        /// Output the ANTLR translation of the specified character value node
        /// </summary>
        protected override void WriteCharValNode(ITree node)
        {
            var isCaseSensitive = IsCaseSensitive(node);
            var text = GetStringValue(node);

            var length = text.Length;

            if (isCaseSensitive && length > 0)
            {
                Write("'");
                Write(text);
                Write("'");
            }
            else
            {
                if (length > 1)
                {
                    Write("(");
                }

                for (int index = 0; index < length; index++)
                {
                    if (index > 0)
                    {
                        Write(" ");
                    }

                    var upperCharacter = char.ToUpperInvariant(text[index]);
                    var lowerCharacter = char.ToLowerInvariant(text[index]);

                    if (upperCharacter == lowerCharacter)
                    {
                        Write("'");
                        Write(AntlrHelper.CharEscape(lowerCharacter));
                        Write("'");
                    }
                    else
                    {
                        Write("(");

                        Write("'");
                        Write(AntlrHelper.CharEscape(upperCharacter));
                        Write("'");

                        Write(" | ");

                        Write("'");
                        Write(AntlrHelper.CharEscape(lowerCharacter));
                        Write("'");

                        Write(")");
                    }
                }

                if (length > 1)
                {
                    Write(")");
                }
            }
        }

        /// <summary>
        /// Output the ANTLR translation of the specified numeric value range node
        /// </summary>
        protected override void WriteValueRangeNode(ITree node)
        {
            var min = node.GetChildWithValidation(0);
            var max = node.GetChildWithValidation(1);

            Visit(min);
            Write("..");
            Visit(max);
        }

        /// <summary>
        /// Write unicode character value as 4 character hex string
        /// </summary>
        protected override void WriteValue(int value)
        {
            Write(string.Format(@"'\u{0:X4}'", value));
        }

        protected override string GetLexerRuleName(string alias)
        {
            return alias.ToUpperInvariant();
        }

    }
}
