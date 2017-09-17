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
    class TreeVisitor_GatherDistinctCharacters : TreeVisitor
    {
        IDictionary<char, NamedCharacter> _distinctCharacters;
        INamedCharacterLookup _lookup;

        public TreeVisitor_GatherDistinctCharacters(IDictionary<char, NamedCharacter> literals, INamedCharacterLookup lookup)
        {
            _distinctCharacters = literals;
            _lookup = lookup;

            CommonConstructor();
        }

        protected void CommonConstructor()
        {
            _distinctCharacters.Clear();
        }

        protected override void VisitRuleList(ITree node)
        {
            // Start over each time the rule list is visited
            CommonConstructor();

            VisitChildren(node);
        }

        protected override void VisitCharVal(ITree node)
        {
            AddCharValNode(node);
        }

        protected override void VisitBinValNumber(ITree node)
        {
            AddBinValNumberNode(node);
        }

        protected override void VisitBinValRange(ITree node)
        {
            AddValueRangeNode(node);
        }

        protected override void VisitDecValNumber(ITree node)
        {
            AddDecValNumberNode(node);
        }

        protected override void VisitDecValRange(ITree node)
        {
            AddValueRangeNode(node);
        }

        protected override void VisitHexValNumber(ITree node)
        {
            AddHexValNumberNode(node);
        }

        protected override void VisitHexValRange(ITree node)
        {
            AddValueRangeNode(node);
        }

        protected override void VisitProseVal(ITree node)
        {
            AddCharValNode(node);
        }

        /// <summary>
        /// Gather the distinct characters of the specified character value node
        /// </summary>
        void AddCharValNode(ITree node)
        {
            string text;
            var char_val = node.GetChild(0);

            text = char_val.Text;
            text = text.Substring(1, text.Length - 2);

            AddText(text);
        }

        /// <summary>
        /// Gather the distinct characters of the specified binary value number node
        /// </summary>
        void AddBinValNumberNode(ITree node)
        {
            int value = GetValue(node);

            AddValue(value);
        }

        /// <summary>
        /// Gather the distinct characters of the specified decimal value number node
        /// </summary>
        void AddDecValNumberNode(ITree node)
        {
            int value = GetValue(node);

            AddValue(value);
        }

        /// <summary>
        /// Gather the distinct characters of the specified hexadecimal value number node
        /// </summary>
        void AddHexValNumberNode(ITree node)
        {
            int value = GetValue(node);

            AddValue(value);
        }

        /// <summary>
        /// Gather the distinct characters of the specified numeric value range node
        /// </summary>
        void AddValueRangeNode(ITree node)
        {
            var min = node.GetChild(0);
            var max = node.GetChild(1);

            int minValue = GetValue(min);
            int maxValue = GetValue(max);

            for (int value = minValue; value <= maxValue; value++)
            {
                AddValue(value);
            }
        }

        /// <summary>
        /// Gather the distinct characters of the specified string
        /// </summary>
        void AddText(string text)
        {
            foreach (char character in text)
            {
                if (_distinctCharacters.ContainsKey(character))
                {
                    // do nothing
                }
                else
                {
                    var namedCharacter = _lookup.GetNamedCharacter(character);

                    _distinctCharacters.Add(character, namedCharacter);
                }
            }
        }

        /// <summary>
        /// Add the specified unicode character value to the set of distinct characters
        /// </summary>
        void AddValue(int value)
        {
            var character = (char)value;

            if (_distinctCharacters.ContainsKey(character))
            {
                // do nothing
            }
            else
            {
                var namedCharacter = _lookup.GetNamedCharacter(character);

                _distinctCharacters.Add(character, namedCharacter);
            }
        }
    }
}
