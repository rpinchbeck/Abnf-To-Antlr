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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Antlr.Runtime;
using Antlr.Runtime.Tree;

namespace AbnfToAntlr.Common
{
    /// <summary>
    /// Output the indirect translation of the ABNF grammar (Substitute lexer rules for each distinct character value in the original grammar).
    /// </summary>
    public class TreeVisitor_OutputTranslation_Indirect : TreeVisitor_OutputTranslation
    {
        Dictionary<char, NamedCharacter> _distinctCharacters;

        public TreeVisitor_OutputTranslation_Indirect(ITokenStream tokens, System.IO.TextWriter writer, Dictionary<char, NamedCharacter> distinctCharacters)
            : base(tokens, writer)
        { 
            _distinctCharacters =distinctCharacters;
        }

        /// <summary>
        /// Output the ANTLR translation of the specified character value node
        /// </summary>
        protected override void WriteCharValNode(ITree node)
        {
            string text;
            var char_val = node.GetChild(0);

            text = char_val.Text;
            text = text.Substring(1, text.Length - 2);

            var length = text.Length;

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

                var namedCharacter = NamedCharacter.GetNamedCharacter(text[index]);
                Write(namedCharacter.Name);
            }

            if (length > 1)
            {
                Write(")");
            }

        }

        /// <summary>
        /// Output the ANTLR translation of the specified numeric value range node
        /// </summary>
        protected override void WriteValueRangeNode(ITree node)
        {
            var minValue = GetValue(node.GetChild(0));
            var maxValue = GetValue(node.GetChild(1));

            if (maxValue > minValue)
            {
                Write("(");
            }

            for (int value = minValue; value <= maxValue; value++)
            {
                if (value > minValue)
                {
                    Write(" | ");
                }

                WriteValue(value);
            }

            if (maxValue > minValue)
            {
                Write(")");
            }
        }

        /// <summary>
        /// Output the named character (lexer rule) that corresponds to the unicode character value
        /// </summary>
        protected override void WriteValue(int value)
        {
            var namedCharacter = NamedCharacter.GetNamedCharacter((char)value);

            Write(namedCharacter.Name);
        }


    } // class
} // namespace
