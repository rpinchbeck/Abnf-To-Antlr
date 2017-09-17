using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbnfToAntlr.Common
{
    public class NamedCharacterLookupUnicode : INamedCharacterLookup
    {
        // Official character names from Unicode.org
        // http://www.unicode.org/charts/PDF/U0000.pdf

        public readonly static Dictionary<char, NamedCharacter> KnownCharacters =
            new Dictionary<char, NamedCharacter> 
            {
                { 'A', new NamedCharacter { Name = "CAPITAL_LETTER_A", Character = 'A' } },
                { 'B', new NamedCharacter { Name = "CAPITAL_LETTER_B", Character = 'B' } },
                { 'C', new NamedCharacter { Name = "CAPITAL_LETTER_C", Character = 'C' } },
                { 'D', new NamedCharacter { Name = "CAPITAL_LETTER_D", Character = 'D' } },
                { 'E', new NamedCharacter { Name = "CAPITAL_LETTER_E", Character = 'E' } },
                { 'F', new NamedCharacter { Name = "CAPITAL_LETTER_F", Character = 'F' } },
                { 'G', new NamedCharacter { Name = "CAPITAL_LETTER_G", Character = 'G' } },
                { 'H', new NamedCharacter { Name = "CAPITAL_LETTER_H", Character = 'H' } },
                { 'I', new NamedCharacter { Name = "CAPITAL_LETTER_I", Character = 'I' } },
                { 'J', new NamedCharacter { Name = "CAPITAL_LETTER_J", Character = 'J' } },
                { 'K', new NamedCharacter { Name = "CAPITAL_LETTER_K", Character = 'K' } },
                { 'L', new NamedCharacter { Name = "CAPITAL_LETTER_L", Character = 'L' } },
                { 'M', new NamedCharacter { Name = "CAPITAL_LETTER_M", Character = 'M' } },
                { 'N', new NamedCharacter { Name = "CAPITAL_LETTER_N", Character = 'N' } },
                { 'O', new NamedCharacter { Name = "CAPITAL_LETTER_O", Character = 'O' } },
                { 'P', new NamedCharacter { Name = "CAPITAL_LETTER_P", Character = 'P' } },
                { 'Q', new NamedCharacter { Name = "CAPITAL_LETTER_Q", Character = 'Q' } },
                { 'R', new NamedCharacter { Name = "CAPITAL_LETTER_R", Character = 'R' } },
                { 'S', new NamedCharacter { Name = "CAPITAL_LETTER_S", Character = 'S' } },
                { 'T', new NamedCharacter { Name = "CAPITAL_LETTER_T", Character = 'T' } },
                { 'U', new NamedCharacter { Name = "CAPITAL_LETTER_U", Character = 'U' } },
                { 'V', new NamedCharacter { Name = "CAPITAL_LETTER_V", Character = 'V' } },
                { 'W', new NamedCharacter { Name = "CAPITAL_LETTER_W", Character = 'W' } },
                { 'X', new NamedCharacter { Name = "CAPITAL_LETTER_X", Character = 'X' } },
                { 'Y', new NamedCharacter { Name = "CAPITAL_LETTER_Y", Character = 'Y' } },
                { 'Z', new NamedCharacter { Name = "CAPITAL_LETTER_Z", Character = 'Z' } },

                { 'a', new NamedCharacter { Name = "SMALL_LETTER_A", Character = 'a' } },
                { 'b', new NamedCharacter { Name = "SMALL_LETTER_B", Character = 'b' } },
                { 'c', new NamedCharacter { Name = "SMALL_LETTER_C", Character = 'c' } },
                { 'd', new NamedCharacter { Name = "SMALL_LETTER_D", Character = 'd' } },
                { 'e', new NamedCharacter { Name = "SMALL_LETTER_E", Character = 'e' } },
                { 'f', new NamedCharacter { Name = "SMALL_LETTER_F", Character = 'f' } },
                { 'g', new NamedCharacter { Name = "SMALL_LETTER_G", Character = 'g' } },
                { 'h', new NamedCharacter { Name = "SMALL_LETTER_H", Character = 'h' } },
                { 'i', new NamedCharacter { Name = "SMALL_LETTER_I", Character = 'i' } },
                { 'j', new NamedCharacter { Name = "SMALL_LETTER_J", Character = 'j' } },
                { 'k', new NamedCharacter { Name = "SMALL_LETTER_K", Character = 'k' } },
                { 'l', new NamedCharacter { Name = "SMALL_LETTER_L", Character = 'l' } },
                { 'm', new NamedCharacter { Name = "SMALL_LETTER_M", Character = 'm' } },
                { 'n', new NamedCharacter { Name = "SMALL_LETTER_N", Character = 'n' } },
                { 'o', new NamedCharacter { Name = "SMALL_LETTER_O", Character = 'o' } },
                { 'p', new NamedCharacter { Name = "SMALL_LETTER_P", Character = 'p' } },
                { 'q', new NamedCharacter { Name = "SMALL_LETTER_Q", Character = 'q' } },
                { 'r', new NamedCharacter { Name = "SMALL_LETTER_R", Character = 'r' } },
                { 's', new NamedCharacter { Name = "SMALL_LETTER_S", Character = 's' } },
                { 't', new NamedCharacter { Name = "SMALL_LETTER_T", Character = 't' } },
                { 'u', new NamedCharacter { Name = "SMALL_LETTER_U", Character = 'u' } },
                { 'v', new NamedCharacter { Name = "SMALL_LETTER_V", Character = 'v' } },
                { 'w', new NamedCharacter { Name = "SMALL_LETTER_W", Character = 'w' } },
                { 'x', new NamedCharacter { Name = "SMALL_LETTER_X", Character = 'x' } },
                { 'y', new NamedCharacter { Name = "SMALL_LETTER_Y", Character = 'y' } },
                { 'z', new NamedCharacter { Name = "SMALL_LETTER_Z", Character = 'z' } },

                { '`', new NamedCharacter { Name = "GRAVE_ACCENT", Character = '`' } },
                { '1', new NamedCharacter { Name = "DIGIT_ONE", Character = '1' } },
                { '2', new NamedCharacter { Name = "DIGIT_TWO", Character = '2' } },
                { '3', new NamedCharacter { Name = "DIGIT_THREE", Character = '3' } },
                { '4', new NamedCharacter { Name = "DIGIT_FOUR", Character = '4' } },
                { '5', new NamedCharacter { Name = "DIGIT_FIVE", Character = '5' } },
                { '6', new NamedCharacter { Name = "DIGIT_SIX", Character = '6' } },
                { '7', new NamedCharacter { Name = "DIGIT_SEVEN", Character = '7' } },
                { '8', new NamedCharacter { Name = "DIGIT_EIGHT", Character = '8' } },
                { '9', new NamedCharacter { Name = "DIGIT_NINE", Character = '9' } },
                { '0', new NamedCharacter { Name = "DIGIT_ZERO", Character = '0' } },
                { '-', new NamedCharacter { Name = "HYPHEN_MINUS", Character = '-' } },
                { '=', new NamedCharacter { Name = "EQUALS_SIGN", Character = '=' } },
                { '[', new NamedCharacter { Name = "LEFT_SQUARE_BRACKET", Character = '[' } },
                { ']', new NamedCharacter { Name = "RIGHT_SQUARE_BRACKET", Character = ']' } },
                { '\\', new NamedCharacter { Name = "REVERSE_SOLIDUS", Character = '\\' } },
                { ';', new NamedCharacter { Name = "SEMICOLON", Character = ';' } },
                { '\'', new NamedCharacter { Name = "APOSTROPHE", Character = '\'' } },
                { ',', new NamedCharacter { Name = "COMMA", Character = ',' } },
                { '.', new NamedCharacter { Name = "FULL_STOP", Character = '.' } },
                { '/', new NamedCharacter { Name = "SOLIDUS", Character = '/' } },
                { '~', new NamedCharacter { Name = "TILDE", Character = '~' } },

                { '!', new NamedCharacter { Name = "EXCLAMATION_MARK", Character = '!' } },
                { '@', new NamedCharacter { Name = "COMMERCIAL_AT", Character = '@' } },
                { '#', new NamedCharacter { Name = "NUMBER_SIGN", Character = '#' } },
                { '$', new NamedCharacter { Name = "DOLLAR_SIGN", Character = '$' } },
                { '%', new NamedCharacter { Name = "PERCENT_SIGN", Character = '%' } },
                { '^', new NamedCharacter { Name = "CIRCUMFLEX_ACCENT", Character = '^' } },
                { '&', new NamedCharacter { Name = "AMPERSAND", Character = '&' } },
                { '*', new NamedCharacter { Name = "ASTERISK", Character = '*' } },
                { '(', new NamedCharacter { Name = "LEFT_PARENTHESIS", Character = '(' } },
                { ')', new NamedCharacter { Name = "RIGHT_PARENTHESIS", Character = ')' } },
                { '_', new NamedCharacter { Name = "LOW_LINE", Character = '_' } },
                { '+', new NamedCharacter { Name = "PLUS_SIGN", Character = '+' } },
                { '{', new NamedCharacter { Name = "LEFT_CURLY_BRACKET", Character = '{' } },
                { '}', new NamedCharacter { Name = "RIGHT_CURLY_BRACKET", Character = '}' } },
                { '|', new NamedCharacter { Name = "VERTICAL_LINE", Character = '|' } },
                { ':', new NamedCharacter { Name = "COLON", Character = ':' } },
                { '\"', new NamedCharacter { Name = "QUOTATION_MARK", Character = '\"' } },
                { '<', new NamedCharacter { Name = "LESS_THAN_SIGN", Character = '<' } },
                { '>', new NamedCharacter { Name = "GREATER_THAN_SIGN", Character = '>' } },
                { '?', new NamedCharacter { Name = "QUESTION_MARK", Character = '?' } },

                { ' ', new NamedCharacter { Name = "SPACE", Character = ' ' } },

                { '\u0009', new NamedCharacter { Name = "TAB", Character = '\u0009' } },
                { '\u000D', new NamedCharacter { Name = "CARRIAGE_RETURN", Character = '\u000D' } },
                { '\u000A', new NamedCharacter { Name = "LINE_FEED", Character = '\u000A' } },
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
                string name = "UNICODE_" + ((int)character).ToString("X4");

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
