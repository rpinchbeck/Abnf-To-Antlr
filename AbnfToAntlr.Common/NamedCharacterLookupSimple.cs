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

namespace AbnfToAntlr.Common
{
    public class NamedCharacterLookupSimple : INamedCharacterLookup
    {
        public string Description
        {
            get { return "Simplified character names based on Unicode (http://www.unicode.org/charts/PDF/U0000.pdf)"; }
        }

        private readonly static NamedCharacter[] _namedCharacters =
            new NamedCharacter[]
            {
                new NamedCharacter { Name = "CAP_A", Character = 'A' },
                new NamedCharacter { Name = "CAP_B", Character = 'B' },
                new NamedCharacter { Name = "CAP_C", Character = 'C' },
                new NamedCharacter { Name = "CAP_D", Character = 'D' },
                new NamedCharacter { Name = "CAP_E", Character = 'E' },
                new NamedCharacter { Name = "CAP_F", Character = 'F' },
                new NamedCharacter { Name = "CAP_G", Character = 'G' },
                new NamedCharacter { Name = "CAP_H", Character = 'H' },
                new NamedCharacter { Name = "CAP_I", Character = 'I' },
                new NamedCharacter { Name = "CAP_J", Character = 'J' },
                new NamedCharacter { Name = "CAP_K", Character = 'K' },
                new NamedCharacter { Name = "CAP_L", Character = 'L' },
                new NamedCharacter { Name = "CAP_M", Character = 'M' },
                new NamedCharacter { Name = "CAP_N", Character = 'N' },
                new NamedCharacter { Name = "CAP_O", Character = 'O' },
                new NamedCharacter { Name = "CAP_P", Character = 'P' },
                new NamedCharacter { Name = "CAP_Q", Character = 'Q' },
                new NamedCharacter { Name = "CAP_R", Character = 'R' },
                new NamedCharacter { Name = "CAP_S", Character = 'S' },
                new NamedCharacter { Name = "CAP_T", Character = 'T' },
                new NamedCharacter { Name = "CAP_U", Character = 'U' },
                new NamedCharacter { Name = "CAP_V", Character = 'V' },
                new NamedCharacter { Name = "CAP_W", Character = 'W' },
                new NamedCharacter { Name = "CAP_X", Character = 'X' },
                new NamedCharacter { Name = "CAP_Y", Character = 'Y' },
                new NamedCharacter { Name = "CAP_Z", Character = 'Z' },

                new NamedCharacter { Name = "A", Character = 'a' },
                new NamedCharacter { Name = "B", Character = 'b' },
                new NamedCharacter { Name = "C", Character = 'c' },
                new NamedCharacter { Name = "D", Character = 'd' },
                new NamedCharacter { Name = "E", Character = 'e' },
                new NamedCharacter { Name = "F", Character = 'f' },
                new NamedCharacter { Name = "G", Character = 'g' },
                new NamedCharacter { Name = "H", Character = 'h' },
                new NamedCharacter { Name = "I", Character = 'i' },
                new NamedCharacter { Name = "J", Character = 'j' },
                new NamedCharacter { Name = "K", Character = 'k' },
                new NamedCharacter { Name = "L", Character = 'l' },
                new NamedCharacter { Name = "M", Character = 'm' },
                new NamedCharacter { Name = "N", Character = 'n' },
                new NamedCharacter { Name = "O", Character = 'o' },
                new NamedCharacter { Name = "P", Character = 'p' },
                new NamedCharacter { Name = "Q", Character = 'q' },
                new NamedCharacter { Name = "R", Character = 'r' },
                new NamedCharacter { Name = "S", Character = 's' },
                new NamedCharacter { Name = "T", Character = 't' },
                new NamedCharacter { Name = "U", Character = 'u' },
                new NamedCharacter { Name = "V", Character = 'v' },
                new NamedCharacter { Name = "W", Character = 'w' },
                new NamedCharacter { Name = "X", Character = 'x' },
                new NamedCharacter { Name = "Y", Character = 'y' },
                new NamedCharacter { Name = "Z", Character = 'z' },

                new NamedCharacter { Name = "ACCENT", Character = '`' },
                new NamedCharacter { Name = "ONE", Character = '1' },
                new NamedCharacter { Name = "TWO", Character = '2' },
                new NamedCharacter { Name = "THREE", Character = '3' },
                new NamedCharacter { Name = "FOUR", Character = '4' },
                new NamedCharacter { Name = "FIVE", Character = '5' },
                new NamedCharacter { Name = "SIX", Character = '6' },
                new NamedCharacter { Name = "SEVEN", Character = '7' },
                new NamedCharacter { Name = "EIGHT", Character = '8' },
                new NamedCharacter { Name = "NINE", Character = '9' },
                new NamedCharacter { Name = "ZERO", Character = '0' },
                new NamedCharacter { Name = "DASH", Character = '-' },
                new NamedCharacter { Name = "EQUALS", Character = '=' },
                new NamedCharacter { Name = "LEFT_BRACE", Character = '[' },
                new NamedCharacter { Name = "RIGHT_BRACE", Character = ']' },
                new NamedCharacter { Name = "BACKSLASH", Character = '\\' },
                new NamedCharacter { Name = "SEMICOLON", Character = ';' },
                new NamedCharacter { Name = "APOSTROPHE", Character = '\'' },
                new NamedCharacter { Name = "COMMA", Character = ',' },
                new NamedCharacter { Name = "PERIOD", Character = '.' },
                new NamedCharacter { Name = "SLASH", Character = '/' },

                new NamedCharacter { Name = "TILDE", Character = '~' },
                new NamedCharacter { Name = "EXCLAMATION", Character = '!' },
                new NamedCharacter { Name = "AT", Character = '@' },
                new NamedCharacter { Name = "HASH", Character = '#' },
                new NamedCharacter { Name = "DOLLAR", Character = '$' },
                new NamedCharacter { Name = "PERCENT", Character = '%' },
                new NamedCharacter { Name = "CARAT", Character = '^' },
                new NamedCharacter { Name = "AMPERSAND", Character = '&' },
                new NamedCharacter { Name = "ASTERISK", Character = '*' },
                new NamedCharacter { Name = "LEFT_PAREN", Character = '(' },
                new NamedCharacter { Name = "RIGHT_PAREN", Character = ')' },
                new NamedCharacter { Name = "UNDERSCORE", Character = '_' },
                new NamedCharacter { Name = "PLUS", Character = '+' },
                new NamedCharacter { Name = "LEFT_CURLY_BRACE", Character = '{' },
                new NamedCharacter { Name = "RIGHT_CURLY_BRACE", Character = '}' },
                new NamedCharacter { Name = "PIPE", Character = '|' },
                new NamedCharacter { Name = "COLON", Character = ':' },
                new NamedCharacter { Name = "QUOTE", Character = '\"' },
                new NamedCharacter { Name = "LESS_THAN", Character = '<' },
                new NamedCharacter { Name = "GREATER_THAN", Character = '>' },
                new NamedCharacter { Name = "QUESTION", Character = '?' },

                new NamedCharacter { Name = "SPACE", Character = ' ' },

                new NamedCharacter { Name = "TAB", Character = '\u0009' },
                new NamedCharacter { Name = "CR", Character = '\u000D' },
                new NamedCharacter { Name = "LF", Character = '\u000A' },
            };

        private static readonly IDictionary<char, NamedCharacter> _namedCharacterMap = CreateNamedCharacterMap();

        private static IDictionary<char, NamedCharacter> CreateNamedCharacterMap()
        {
            var result = new Dictionary<char, NamedCharacter>();

            foreach (var namedCharacter in _namedCharacters)
            {
                result.Add(namedCharacter.Character, namedCharacter);
            }

            return result;
        }

        public bool IsKnownCharacter(char character)
        {
            return _namedCharacterMap.ContainsKey(character);
        }

        public NamedCharacter GetNamedCharacter(char character)
        {
            NamedCharacter result;

            if (_namedCharacterMap.TryGetValue(character, out result))
            {
                // do nothing
            }
            else
            {
                string name = "U_" + ((int)character).ToString("X4");

                result =
                    new NamedCharacter
                    {
                        Name = name,
                        Character = character
                    };
            }

            return result;
        }

    } // class
} // namespace
