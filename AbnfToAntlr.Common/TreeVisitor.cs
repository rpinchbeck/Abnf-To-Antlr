﻿/*

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
    /// Abstract class for visiting the nodes of an ABNF Abstract Syntax Tree
    /// </summary>
    public abstract class TreeVisitor
    {
        // Called when visiting a node in the ABNF Abstract Syntax Tree
        public void Visit(ITree node)
        {
            BeforeVisit(node);

            switch (node.Type)
            {
                case AbnfAstParser.RULE_LIST_NODE:
                    VisitRuleList(node);
                    break;

                case AbnfAstParser.RULE_NODE:
                    VisitRule(node);
                    break;

                case AbnfAstParser.RULE_NAME_NODE:
                    VisitRuleName(node);
                    break;

                case AbnfAstParser.DEFINED_AS_NODE:
                    VisitDefinedAs(node);
                    break;

                case AbnfAstParser.ALTERNATION_NODE:
                    VisitAlternation(node);
                    break;

                case AbnfAstParser.CONCATENATION_NODE:
                    VisitConcatenation(node);
                    break;

                case AbnfAstParser.GROUP_NODE:
                    VisitGroup(node);
                    break;

                case AbnfAstParser.OPTION_NODE:
                    VisitOption(node);
                    break;

                case AbnfAstParser.REPETITION_NODE:
                    VisitRepetition(node);
                    break;

                case AbnfAstParser.CHAR_VAL_NODE:
                    VisitCharVal(node);
                    break;

                case AbnfAstParser.BIN_VAL_NODE:
                    VisitBinVal(node);
                    break;

                case AbnfAstParser.BIN_VAL_NUMBER_NODE:
                    VisitBinValNumber(node);
                    break;

                case AbnfAstParser.BIN_VAL_RANGE_NODE:
                    VisitBinValRange(node);
                    break;

                case AbnfAstParser.BIN_VAL_CONCAT_NODE:
                    VisitBinValConcat(node);
                    break;

                case AbnfAstParser.DEC_VAL_NODE:
                    VisitDecVal(node);
                    break;

                case AbnfAstParser.DEC_VAL_NUMBER_NODE:
                    VisitDecValNumber(node);
                    break;

                case AbnfAstParser.DEC_VAL_RANGE_NODE:
                    VisitDecValRange(node);
                    break;

                case AbnfAstParser.DEC_VAL_CONCAT_NODE:
                    VisitDecValConcat(node);
                    break;

                case AbnfAstParser.HEX_VAL_NODE:
                    VisitHexVal(node);
                    break;

                case AbnfAstParser.HEX_VAL_NUMBER_NODE:
                    VisitHexValNumber(node);
                    break;

                case AbnfAstParser.HEX_VAL_RANGE_NODE:
                    VisitHexValRange(node);
                    break;

                case AbnfAstParser.HEX_VAL_CONCAT_NODE:
                    VisitHexValConcat(node);
                    break;

                case AbnfAstParser.PROSE_VAL_NODE:
                    VisitProseVal(node);
                    break;

                default:

                    if (node is CommonErrorNode)
                    {
                        var commonErrorNode = node as CommonErrorNode;

                        var trappedException = commonErrorNode.trappedException;

                        throw trappedException;
                    }
                    else
                    {
                        var ancestorNode = node;

                        while (ancestorNode.Line <= 0 && ancestorNode.Parent != null)
                        {
                            ancestorNode = ancestorNode.Parent;
                        }

                        throw new InvalidOperationException("Unexpected node type encountered near line " + ancestorNode.Line + ", position " + ancestorNode.CharPositionInLine + ".");
                    }
            }

            AfterVisit(node);
        }

        /// <summary>
        /// Visit the children of a node in left-to-right depth-first order
        /// </summary>
        protected void VisitChildren(ITree node)
        {
            int childCount = node.ChildCount;

            for (int index = 0; index < childCount; index++)
            {
                var child = node.GetAndValidateChild(index);

                Visit(child);
            }
        }

        /// <summary>
        /// Get the integer value of a node
        /// </summary>
        protected int GetValue(ITree node)
        {
            int result = 0;
            string text;

            switch (node.Type)
            {
                case AbnfAstParser.BIN_VAL_NUMBER_NODE:
                    text = GetChildrenText(node);
                    result = Convert.ToInt32(text, 2);
                    break;

                case AbnfAstParser.DEC_VAL_NUMBER_NODE:
                    text = GetChildrenText(node);
                    result = Int32.Parse(text, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case AbnfAstParser.HEX_VAL_NUMBER_NODE:
                    text = GetChildrenText(node);
                    result = Int32.Parse(text, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                default:
                    throw new InvalidOperationException("Unexpected node type encountered while getting value.");
            }

            return result;
        }

        /// <summary>
        /// Get the combined text of all children of the specified node
        /// </summary>
        /// <returns>the combined text of all children of the specified node</returns>
        protected string GetChildrenText(ITree node)
        {
            var builder = new StringBuilder();
            ITree child;

            var maxIndex = node.ChildCount;
            for (int index = 0; index < maxIndex; index++)
            {
                child = node.GetAndValidateChild(index);

                if (child.ChildCount == 0)
                {
                    builder.Append(child.Text);
                }
                else
                {
                    throw new InvalidOperationException("Unexpected node type encountered while getting text of children.");
                }
            }

            return builder.ToString();
        }

        public string GetStringValue(ITree node)
        {
            ITree char_val;

            if (node.Type == AbnfAstParser.CHAR_VAL_NODE)
            {
                var string_node = node.GetAndValidateChild(0);

                char_val = string_node.GetAndValidateChild(0);
            }
            else if (node.Type == AbnfAstParser.PROSE_VAL_NODE)
            {
                char_val = node.GetAndValidateChild(0);
            }
            else
            {
                throw new Exception("Unexpected node type encountered while getting string value");
            }

            var result = char_val.Text;
            result = result.Substring(1, result.Length - 2);

            return result;
        }

        public bool IsCaseSensitive(ITree node)
        {
            bool result;

            if (node.Type == AbnfAstParser.CHAR_VAL_NODE)
            {
                var string_node = node.GetAndValidateChild(0);

                result = (string_node.Type == AbnfAstParser.CASE_SENSITIVE_STRING_NODE);
            }
            else if (node.Type == AbnfAstParser.PROSE_VAL_NODE)
            {
                result = true;
            }
            else
            {
                throw new Exception("Unexpected node type encountered while checking case sensitivity of string value");
            }

            return result;
        }

        public readonly HashSet<string> ReservedKeywords =
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
        protected bool IsReservedKeyWord(string text)
        {
            var result = ReservedKeywords.Contains(text);

            return result;
        }


        protected virtual void BeforeVisit(ITree node)
        {
            // do nothing by default
        }

        protected virtual void AfterVisit(ITree node)
        {
            // do nothing by default
        }

        protected virtual void VisitRuleList(ITree node)
        {
            VisitChildren(node);
        }

        protected virtual void VisitRule(ITree node)
        {
            VisitChildren(node);
        }

        protected virtual void VisitRuleName(ITree node)
        {
            // do nothing, terminal node
        }

        protected virtual void VisitDefinedAs(ITree node)
        {
            // do nothing, terminal node
        }

        protected virtual void VisitAlternation(ITree node)
        {
            VisitChildren(node);
        }

        protected virtual void VisitConcatenation(ITree node)
        {
            VisitChildren(node);
        }

        protected virtual void VisitGroup(ITree node)
        {
            VisitChildren(node);
        }

        protected virtual void VisitOption(ITree node)
        {
            VisitChildren(node);
        }

        protected virtual void VisitRepetition(ITree node)
        {
            var element = node.GetAndValidateChild(0);

            Visit(element);
        }

        protected virtual void VisitCharVal(ITree node)
        {
            // do nothing, terminal node
        }

        protected virtual void VisitBinVal(ITree node)
        {
            VisitChildren(node);
        }

        protected virtual void VisitBinValNumber(ITree node)
        {
            // do nothing, terminal node
        }

        protected virtual void VisitBinValRange(ITree node)
        {
            VisitChildren(node);
        }

        protected virtual void VisitBinValConcat(ITree node)
        {
            VisitChildren(node);
        }

        protected virtual void VisitDecVal(ITree node)
        {
            VisitChildren(node);
        }

        protected virtual void VisitDecValNumber(ITree node)
        {
            // do nothing, terminal node
        }

        protected virtual void VisitDecValRange(ITree node)
        {
            VisitChildren(node);
        }

        protected virtual void VisitDecValConcat(ITree node)
        {
            VisitChildren(node);
        }

        protected virtual void VisitHexVal(ITree node)
        {
            VisitChildren(node);
        }

        protected virtual void VisitHexValNumber(ITree node)
        {
            // do nothing, terminal node
        }

        protected virtual void VisitHexValRange(ITree node)
        {
            VisitChildren(node);
        }

        protected virtual void VisitHexValConcat(ITree node)
        {
            VisitChildren(node);
        }

        protected virtual void VisitProseVal(ITree node)
        {
            // do nothing, terminal node
        }
    }
}
