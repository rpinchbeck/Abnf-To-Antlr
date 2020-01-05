/*

    Copyright 2020 Robert Pinchbeck
  
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

using Antlr.Runtime;
using Antlr.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AbnfToAntlr.Common
{
    public static class AntlrHelper
    {
        public static ITree GetChildWithValidation(this ITree node, int index)
        {
            ThrowIfErrorNode(node);

            var result = node.GetChild(index);

            ThrowIfErrorNode(result);

            return result;
        }

        public static ITree GetChildWithValidation(this ITree node, int index, int expectedNodeType)
        {
            var result = node.GetChildWithValidation(index);

            result.Validate(expectedNodeType);

            return result;
        }

        public static void Validate(this ITree node, int expectedNodeType)
        {
            ThrowIfErrorNode(node);

            if (node.Type != expectedNodeType)
            {
                throw new InvalidOperationException("Unexpected node type encountered");
            }
        }

        public static void ThrowIfErrorNode(ITree node)
        {
            if (node is CommonErrorNode)
            {
                var commonErrorNode = (CommonErrorNode)node;

                throw commonErrorNode.trappedException;
            }

        }

        public static string GetErrorMessages(IEnumerable<RecognitionException> recognitionExceptions)
        {
            var builder = new StringBuilder();
            foreach (var recognitionException in recognitionExceptions)
            {
                builder.AppendLine(GetErrorMessage(recognitionException));
                builder.AppendLine();
            }

            return builder.ToString();
        }

        public static string GetErrorMessage(RecognitionException recognitionException)
        {
            var message = recognitionException.Message;

            if (message.EndsWith("."))
            {
                // strip ending period from message
                message = message.Substring(0, message.Length - 1);
            }

            if (message.Length > 0)
            {
                message = message + " ";
            }

            var result = message + (recognitionException.ApproximateLineInfo ? "near" : "at") + " line " + recognitionException.Line + " column " + (recognitionException.CharPositionInLine + 1) + " \"" + recognitionException.ToString() + "\"";

            return result;
        }

        public static string CharEscape(char character)
        {
            string result;

            if (character == '\'')
            {
                result = @"\'";
            }
            else if (character == '\\')
            {
                result = @"\\";
            }
            else if (character < 32)
            {
                result = @"\u" + ((int)character).ToString("X4");
            }
            else
            {
                result = character.ToString();
            }

            return result;
        }

        public static readonly HashSet<string> ReservedKeywords =
            new HashSet<string>
            {
                // ANTLR reserved keywords
                "catch",
                "finally",
                "fragment",
                "grammar",
                "import",
                "lexer",
                "locals",
                "mode",
                "options",
                "parser",
                "returns",
                "throws",
                "tokens",

                // ANTLR recommended reserved keywords
                "rule",


                // Java reserved keywords
                "abstract",
                "assert",
                "boolean",
                "break",
                "byte",
                "case",
                "catch", // ANTLR keyword too
                "char",
                "class",
                "const",
                "continue",
                "default",
                "do",
                "double",
                "else",
                "enum",
                "extends",
                "final",
                "finally", // ANTLR keyword too
                "float",
                "for",
                "goto",
                "if",
                "implements",
                "import", // ANTLR keyword too
                "instanceof",
                "int",
                "interface",
                "long",
                "native",
                "new",
                "package",
                "private",
                "protected",
                "public",
                "return",
                "short",
                "static",
                "strictfp",
                "super",
                "switch",
                "synchronized",
                "this",
                "throw",
                "throws", // ANTLR keyword too
                "transient",
                "try",
                "void",
                "volatile",
                "while",

                "false",
                "null",
                "true",

                // C# reserved keywords
                "abstract",
                "as",
                "base",
                "bool",
                "break",
                "byte",
                "case",
                "catch",
                "char",
                "checked",
                "class",
                "const",
                "continue",
                "decimal",
                "default",
                "delegate",
                "do",
                "double",
                "else",
                "enum",
                "event",
                "explicit",
                "extern",
                "false",
                "finally",
                "fixed",
                "float",
                "for",
                "foreach",
                "goto",
                "if",
                "implicit",
                "in",
                "int",
                "interface",
                "internal",
                "is",
                "lock",
                "long",
                "namespace",
                "new",
                "null",
                "object",
                "operator",
                "out",
                "override",
                "params",
                "private",
                "protected",
                "public",
                "readonly",
                "ref",
                "return",
                "sbyte",
                "sealed",
                "short",
                "sizeof",
                "stackalloc",
                "static",
                "string",
                "struct",
                "switch",
                "this",
                "throw",
                "true",
                "try",
                "typeof",
                "uint",
                "ulong",
                "unchecked",
                "unsafe",
                "ushort",
                "using",
                "virtual",
                "void",
                "volatile",
                "while"
            };

        /// <summary>
        /// Determine if the specified string is a reserved keyword in ANTLR, Java or C#
        /// </summary>
        public static bool IsReservedKeyWord(string text)
        {
            var result = ReservedKeywords.Contains(text);

            return result;
        }

        /// <summary>
        /// Get ANTLR compatible rule name
        /// </summary>
        /// <returns>ANTLR compatible rule name</returns>
        public static string GetParserRuleName(string ruleName)
        {
            string result;

            result = ruleName;

            // translate all rules into parser rules until they are later proven to be lexer rules
            result = result.ToLowerInvariant();

            // dashes are not allowed in ANTLR rule names (so replace them with underscores)
            result = result.Replace("-", "_");

            return result;
        }


    }
}
