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

namespace AbnfToAntlr.Common
{
    public class NamedCharacterLookupSimple : INamedCharacterLookup
    {
        // Official character names from Unicode.org
        // http://www.unicode.org/charts/PDF/U0000.pdf

        public readonly static Dictionary<char, NamedCharacter> KnownCharacters = 
            new Dictionary<char, NamedCharacter> 
            {
                { 'A', new NamedCharacter { Name = "CAP_A", Character = 'A' } },
                { 'B', new NamedCharacter { Name = "CAP_B", Character = 'B' } },
                { 'C', new NamedCharacter { Name = "CAP_C", Character = 'C' } },
                { 'D', new NamedCharacter { Name = "CAP_D", Character = 'D' } },
                { 'E', new NamedCharacter { Name = "CAP_E", Character = 'E' } },
                { 'F', new NamedCharacter { Name = "CAP_F", Character = 'F' } },
                { 'G', new NamedCharacter { Name = "CAP_G", Character = 'G' } },
                { 'H', new NamedCharacter { Name = "CAP_H", Character = 'H' } },
                { 'I', new NamedCharacter { Name = "CAP_I", Character = 'I' } },
                { 'J', new NamedCharacter { Name = "CAP_J", Character = 'J' } },
                { 'K', new NamedCharacter { Name = "CAP_K", Character = 'K' } },
                { 'L', new NamedCharacter { Name = "CAP_L", Character = 'L' } },
                { 'M', new NamedCharacter { Name = "CAP_M", Character = 'M' } },
                { 'N', new NamedCharacter { Name = "CAP_N", Character = 'N' } },
                { 'O', new NamedCharacter { Name = "CAP_O", Character = 'O' } },
                { 'P', new NamedCharacter { Name = "CAP_P", Character = 'P' } },
                { 'Q', new NamedCharacter { Name = "CAP_Q", Character = 'Q' } },
                { 'R', new NamedCharacter { Name = "CAP_R", Character = 'R' } },
                { 'S', new NamedCharacter { Name = "CAP_S", Character = 'S' } },
                { 'T', new NamedCharacter { Name = "CAP_T", Character = 'T' } },
                { 'U', new NamedCharacter { Name = "CAP_U", Character = 'U' } },
                { 'V', new NamedCharacter { Name = "CAP_V", Character = 'V' } },
                { 'W', new NamedCharacter { Name = "CAP_W", Character = 'W' } },
                { 'X', new NamedCharacter { Name = "CAP_X", Character = 'X' } },
                { 'Y', new NamedCharacter { Name = "CAP_Y", Character = 'Y' } },
                { 'Z', new NamedCharacter { Name = "CAP_Z", Character = 'Z' } },

                { 'a', new NamedCharacter { Name = "A", Character = 'a' } },
                { 'b', new NamedCharacter { Name = "B", Character = 'b' } },
                { 'c', new NamedCharacter { Name = "C", Character = 'c' } },
                { 'd', new NamedCharacter { Name = "D", Character = 'd' } },
                { 'e', new NamedCharacter { Name = "E", Character = 'e' } },
                { 'f', new NamedCharacter { Name = "F", Character = 'f' } },
                { 'g', new NamedCharacter { Name = "G", Character = 'g' } },
                { 'h', new NamedCharacter { Name = "H", Character = 'h' } },
                { 'i', new NamedCharacter { Name = "I", Character = 'i' } },
                { 'j', new NamedCharacter { Name = "J", Character = 'j' } },
                { 'k', new NamedCharacter { Name = "K", Character = 'k' } },
                { 'l', new NamedCharacter { Name = "L", Character = 'l' } },
                { 'm', new NamedCharacter { Name = "M", Character = 'm' } },
                { 'n', new NamedCharacter { Name = "N", Character = 'n' } },
                { 'o', new NamedCharacter { Name = "O", Character = 'o' } },
                { 'p', new NamedCharacter { Name = "P", Character = 'p' } },
                { 'q', new NamedCharacter { Name = "Q", Character = 'q' } },
                { 'r', new NamedCharacter { Name = "R", Character = 'r' } },
                { 's', new NamedCharacter { Name = "S", Character = 's' } },
                { 't', new NamedCharacter { Name = "T", Character = 't' } },
                { 'u', new NamedCharacter { Name = "U", Character = 'u' } },
                { 'v', new NamedCharacter { Name = "V", Character = 'v' } },
                { 'w', new NamedCharacter { Name = "W", Character = 'w' } },
                { 'x', new NamedCharacter { Name = "X", Character = 'x' } },
                { 'y', new NamedCharacter { Name = "Y", Character = 'y' } },
                { 'z', new NamedCharacter { Name = "Z", Character = 'z' } },

                { '`', new NamedCharacter { Name = "ACCENT", Character = '`' } },
                { '1', new NamedCharacter { Name = "ONE", Character = '1' } },
                { '2', new NamedCharacter { Name = "TWO", Character = '2' } },
                { '3', new NamedCharacter { Name = "THREE", Character = '3' } },
                { '4', new NamedCharacter { Name = "FOUR", Character = '4' } },
                { '5', new NamedCharacter { Name = "FIVE", Character = '5' } },
                { '6', new NamedCharacter { Name = "SIX", Character = '6' } },
                { '7', new NamedCharacter { Name = "SEVEN", Character = '7' } },
                { '8', new NamedCharacter { Name = "EIGHT", Character = '8' } },
                { '9', new NamedCharacter { Name = "NINE", Character = '9' } },
                { '0', new NamedCharacter { Name = "ZERO", Character = '0' } },
                { '-', new NamedCharacter { Name = "DASH", Character = '-' } },
                { '=', new NamedCharacter { Name = "EQUALS", Character = '=' } },
                { '[', new NamedCharacter { Name = "LEFT_BRACE", Character = '[' } },
                { ']', new NamedCharacter { Name = "RIGHT_BRACE", Character = ']' } },
                { '\\', new NamedCharacter { Name = "BACKSLASH", Character = '\\' } },
                { ';', new NamedCharacter { Name = "SEMICOLON", Character = ';' } },
                { '\'', new NamedCharacter { Name = "APOSTROPHE", Character = '\'' } },
                { ',', new NamedCharacter { Name = "COMMA", Character = ',' } },
                { '.', new NamedCharacter { Name = "PERIOD", Character = '.' } },
                { '/', new NamedCharacter { Name = "SLASH", Character = '/' } },
                { '~', new NamedCharacter { Name = "TILDE", Character = '~' } },

                { '!', new NamedCharacter { Name = "EXCLAMATION", Character = '!' } },
                { '@', new NamedCharacter { Name = "AT", Character = '@' } },
                { '#', new NamedCharacter { Name = "POUND", Character = '#' } },
                { '$', new NamedCharacter { Name = "DOLLAR", Character = '$' } },
                { '%', new NamedCharacter { Name = "PERCENT", Character = '%' } },
                { '^', new NamedCharacter { Name = "CARAT", Character = '^' } },
                { '&', new NamedCharacter { Name = "AMPERSAND", Character = '&' } },
                { '*', new NamedCharacter { Name = "ASTERISK", Character = '*' } },
                { '(', new NamedCharacter { Name = "LEFT_PAREN", Character = '(' } },
                { ')', new NamedCharacter { Name = "RIGHT_PAREN", Character = ')' } },
                { '_', new NamedCharacter { Name = "UNDERSCORE", Character = '_' } },
                { '+', new NamedCharacter { Name = "PLUS", Character = '+' } },
                { '{', new NamedCharacter { Name = "LEFT_CURLY_BRACE", Character = '{' } },
                { '}', new NamedCharacter { Name = "RIGHT_CURLY_BRACE", Character = '}' } },
                { '|', new NamedCharacter { Name = "PIPE", Character = '|' } },
                { ':', new NamedCharacter { Name = "COLON", Character = ':' } },
                { '\"', new NamedCharacter { Name = "QUOTE", Character = '\"' } },
                { '<', new NamedCharacter { Name = "LESS_THAN", Character = '<' } },
                { '>', new NamedCharacter { Name = "GREATER_THAN", Character = '>' } },
                { '?', new NamedCharacter { Name = "QUESTION", Character = '?' } },

                { ' ', new NamedCharacter { Name = "SPACE", Character = ' ' } },

                { '\u0009', new NamedCharacter { Name = "TAB", Character = '\u0009' } },
                { '\u000D', new NamedCharacter { Name = "CR", Character = '\u000D' } },
                { '\u000A', new NamedCharacter { Name = "LF", Character = '\u000A' } },
            };

        public bool IsKnownCharacter(char character)
        {
            return KnownCharacters.ContainsKey(character);
        }

        public NamedCharacter GetNamedCharacter(char character)
        {
            NamedCharacter result;

            if (KnownCharacters.TryGetValue(character, out result))
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
