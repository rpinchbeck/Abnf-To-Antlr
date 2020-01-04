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
    public class NamedCharacterLookupUnicode : INamedCharacterLookup
    {
        public string Description
        {
            get { return "Official Unicode character names (http://www.unicode.org/charts/PDF/U0000.pdf)"; }
        }
       
        private readonly static NamedCharacter[] _namedCharacters =
            new NamedCharacter[]
            {
                new NamedCharacter { Name = "CAPITAL_LETTER_A", Character = 'A' },
                new NamedCharacter { Name = "CAPITAL_LETTER_B", Character = 'B' },
                new NamedCharacter { Name = "CAPITAL_LETTER_C", Character = 'C' },
                new NamedCharacter { Name = "CAPITAL_LETTER_D", Character = 'D' },
                new NamedCharacter { Name = "CAPITAL_LETTER_E", Character = 'E' },
                new NamedCharacter { Name = "CAPITAL_LETTER_F", Character = 'F' },
                new NamedCharacter { Name = "CAPITAL_LETTER_G", Character = 'G' },
                new NamedCharacter { Name = "CAPITAL_LETTER_H", Character = 'H' },
                new NamedCharacter { Name = "CAPITAL_LETTER_I", Character = 'I' },
                new NamedCharacter { Name = "CAPITAL_LETTER_J", Character = 'J' },
                new NamedCharacter { Name = "CAPITAL_LETTER_K", Character = 'K' },
                new NamedCharacter { Name = "CAPITAL_LETTER_L", Character = 'L' },
                new NamedCharacter { Name = "CAPITAL_LETTER_M", Character = 'M' },
                new NamedCharacter { Name = "CAPITAL_LETTER_N", Character = 'N' },
                new NamedCharacter { Name = "CAPITAL_LETTER_O", Character = 'O' },
                new NamedCharacter { Name = "CAPITAL_LETTER_P", Character = 'P' },
                new NamedCharacter { Name = "CAPITAL_LETTER_Q", Character = 'Q' },
                new NamedCharacter { Name = "CAPITAL_LETTER_R", Character = 'R' },
                new NamedCharacter { Name = "CAPITAL_LETTER_S", Character = 'S' },
                new NamedCharacter { Name = "CAPITAL_LETTER_T", Character = 'T' },
                new NamedCharacter { Name = "CAPITAL_LETTER_U", Character = 'U' },
                new NamedCharacter { Name = "CAPITAL_LETTER_V", Character = 'V' },
                new NamedCharacter { Name = "CAPITAL_LETTER_W", Character = 'W' },
                new NamedCharacter { Name = "CAPITAL_LETTER_X", Character = 'X' },
                new NamedCharacter { Name = "CAPITAL_LETTER_Y", Character = 'Y' },
                new NamedCharacter { Name = "CAPITAL_LETTER_Z", Character = 'Z' },

                new NamedCharacter { Name = "SMALL_LETTER_A", Character = 'a' },
                new NamedCharacter { Name = "SMALL_LETTER_B", Character = 'b' },
                new NamedCharacter { Name = "SMALL_LETTER_C", Character = 'c' },
                new NamedCharacter { Name = "SMALL_LETTER_D", Character = 'd' },
                new NamedCharacter { Name = "SMALL_LETTER_E", Character = 'e' },
                new NamedCharacter { Name = "SMALL_LETTER_F", Character = 'f' },
                new NamedCharacter { Name = "SMALL_LETTER_G", Character = 'g' },
                new NamedCharacter { Name = "SMALL_LETTER_H", Character = 'h' },
                new NamedCharacter { Name = "SMALL_LETTER_I", Character = 'i' },
                new NamedCharacter { Name = "SMALL_LETTER_J", Character = 'j' },
                new NamedCharacter { Name = "SMALL_LETTER_K", Character = 'k' },
                new NamedCharacter { Name = "SMALL_LETTER_L", Character = 'l' },
                new NamedCharacter { Name = "SMALL_LETTER_M", Character = 'm' },
                new NamedCharacter { Name = "SMALL_LETTER_N", Character = 'n' },
                new NamedCharacter { Name = "SMALL_LETTER_O", Character = 'o' },
                new NamedCharacter { Name = "SMALL_LETTER_P", Character = 'p' },
                new NamedCharacter { Name = "SMALL_LETTER_Q", Character = 'q' },
                new NamedCharacter { Name = "SMALL_LETTER_R", Character = 'r' },
                new NamedCharacter { Name = "SMALL_LETTER_S", Character = 's' },
                new NamedCharacter { Name = "SMALL_LETTER_T", Character = 't' },
                new NamedCharacter { Name = "SMALL_LETTER_U", Character = 'u' },
                new NamedCharacter { Name = "SMALL_LETTER_V", Character = 'v' },
                new NamedCharacter { Name = "SMALL_LETTER_W", Character = 'w' },
                new NamedCharacter { Name = "SMALL_LETTER_X", Character = 'x' },
                new NamedCharacter { Name = "SMALL_LETTER_Y", Character = 'y' },
                new NamedCharacter { Name = "SMALL_LETTER_Z", Character = 'z' },

                new NamedCharacter { Name = "GRAVE_ACCENT", Character = '`' },
                new NamedCharacter { Name = "DIGIT_ONE", Character = '1' },
                new NamedCharacter { Name = "DIGIT_TWO", Character = '2' },
                new NamedCharacter { Name = "DIGIT_THREE", Character = '3' },
                new NamedCharacter { Name = "DIGIT_FOUR", Character = '4' },
                new NamedCharacter { Name = "DIGIT_FIVE", Character = '5' },
                new NamedCharacter { Name = "DIGIT_SIX", Character = '6' },
                new NamedCharacter { Name = "DIGIT_SEVEN", Character = '7' },
                new NamedCharacter { Name = "DIGIT_EIGHT", Character = '8' },
                new NamedCharacter { Name = "DIGIT_NINE", Character = '9' },
                new NamedCharacter { Name = "DIGIT_ZERO", Character = '0' },
                new NamedCharacter { Name = "HYPHEN_MINUS", Character = '-' },
                new NamedCharacter { Name = "EQUALS_SIGN", Character = '=' },
                new NamedCharacter { Name = "LEFT_SQUARE_BRACKET", Character = '[' },
                new NamedCharacter { Name = "RIGHT_SQUARE_BRACKET", Character = ']' },
                new NamedCharacter { Name = "REVERSE_SOLIDUS", Character = '\\' },
                new NamedCharacter { Name = "SEMICOLON", Character = ';' },
                new NamedCharacter { Name = "APOSTROPHE", Character = '\'' },
                new NamedCharacter { Name = "COMMA", Character = ',' },
                new NamedCharacter { Name = "FULL_STOP", Character = '.' },
                new NamedCharacter { Name = "SOLIDUS", Character = '/' },

                new NamedCharacter { Name = "TILDE", Character = '~' },
                new NamedCharacter { Name = "EXCLAMATION_MARK", Character = '!' },
                new NamedCharacter { Name = "COMMERCIAL_AT", Character = '@' },
                new NamedCharacter { Name = "NUMBER_SIGN", Character = '#' },
                new NamedCharacter { Name = "DOLLAR_SIGN", Character = '$' },
                new NamedCharacter { Name = "PERCENT_SIGN", Character = '%' },
                new NamedCharacter { Name = "CIRCUMFLEX_ACCENT", Character = '^' },
                new NamedCharacter { Name = "AMPERSAND", Character = '&' },
                new NamedCharacter { Name = "ASTERISK", Character = '*' },
                new NamedCharacter { Name = "LEFT_PARENTHESIS", Character = '(' },
                new NamedCharacter { Name = "RIGHT_PARENTHESIS", Character = ')' },
                new NamedCharacter { Name = "LOW_LINE", Character = '_' },
                new NamedCharacter { Name = "PLUS_SIGN", Character = '+' },
                new NamedCharacter { Name = "LEFT_CURLY_BRACKET", Character = '{' },
                new NamedCharacter { Name = "RIGHT_CURLY_BRACKET", Character = '}' },
                new NamedCharacter { Name = "VERTICAL_LINE", Character = '|' },
                new NamedCharacter { Name = "COLON", Character = ':' },
                new NamedCharacter { Name = "QUOTATION_MARK", Character = '\"' },
                new NamedCharacter { Name = "LESS_THAN_SIGN", Character = '<' },
                new NamedCharacter { Name = "GREATER_THAN_SIGN", Character = '>' },
                new NamedCharacter { Name = "QUESTION_MARK", Character = '?' },

                new NamedCharacter { Name = "SPACE", Character = ' ' },

                new NamedCharacter { Name = "TAB", Character = '\u0009' },
                new NamedCharacter { Name = "CARRIAGE_RETURN", Character = '\u000D' },
                new NamedCharacter { Name = "LINE_FEED", Character = '\u000A' },
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

    }
}
