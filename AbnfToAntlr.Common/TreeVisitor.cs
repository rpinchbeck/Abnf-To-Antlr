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
                    throw new InvalidOperationException("Unexpected node type encountered.");
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
                var child = node.GetChild(index);

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
                    throw new InvalidOperationException("Unexpected value node encountered.");
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
                child = node.GetChild(index);

                if (child.ChildCount == 0)
                {
                    builder.Append(child.Text);
                }
                else
                {
                    throw new InvalidOperationException("Unexpected child node encountered.");
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Determine if the specified string is a reserved keyword in ANTLR, Java or C#
        /// </summary>
        protected bool IsReservedKeyWord(string text)
        {
            bool result;

            // prevent collisions with ANTLR keywords (give colliding rule names a numeric suffix)
            switch (text)
            {
                // ANTLR reserved keywords
                case "catch":
                case "finally":
                case "fragment":
                case "grammar":
                case "import":
                case "lexer":
                case "locals":
                case "mode":
                case "options":
                case "parser":
                case "returns":
                case "throws":
                case "tokens":

                // ANTLR recommended reserved keywords
                case "rule":
                    result = true;
                    goto ReturnResult;
            }

            // Java reserved keywords
            switch (text)
            {
                case "abstract":
                case "assert":
                case "boolean":
                case "break":
                case "byte":
                case "case":
                case "catch": // ANTLR keyword too
                case "char":
                case "class":
                case "const":
                case "continue":
                case "default":
                case "do":
                case "double":
                case "else":
                case "enum":
                case "extends":
                case "final":
                case "finally": // ANTLR keyword too
                case "float":
                case "for":
                case "goto":
                case "if":
                case "implements":
                case "import": // ANTLR keyword too
                case "instanceof":
                case "int":
                case "interface":
                case "long":
                case "native":
                case "new":
                case "package":
                case "private":
                case "protected":
                case "public":
                case "return":
                case "short":
                case "static":
                case "strictfp":
                case "super":
                case "switch":
                case "synchronized":
                case "this":
                case "throw":
                case "throws": // ANTLR keyword too
                case "transient":
                case "try":
                case "void":
                case "volatile":
                case "while":

                case "false":
                case "null":
                case "true":
                    result = true;
                    goto ReturnResult;
            }

            // C# reserved keywords
            switch (text)
            {
                case "abstract":
                case "as":
                case "base":
                case "bool":
                case "break":
                case "byte":
                case "case":
                case "catch":
                case "char":
                case "checked":
                case "class":
                case "const":
                case "continue":
                case "decimal":
                case "default":
                case "delegate":
                case "do":
                case "double":
                case "else":
                case "enum":
                case "event":
                case "explicit":
                case "extern":
                case "false":
                case "finally":
                case "fixed":
                case "float":
                case "for":
                case "foreach":
                case "goto":
                case "if":
                case "implicit":
                case "in":
                case "int":
                case "interface":
                case "internal":
                case "is":
                case "lock":
                case "long":
                case "namespace":
                case "new":
                case "null":
                case "object":
                case "operator":
                case "out":
                case "override":
                case "params":
                case "private":
                case "protected":
                case "public":
                case "readonly":
                case "ref":
                case "return":
                case "sbyte":
                case "sealed":
                case "short":
                case "sizeof":
                case "stackalloc":
                case "static":
                case "string":
                case "struct":
                case "switch":
                case "this":
                case "throw":
                case "true":
                case "try":
                case "typeof":
                case "uint":
                case "ulong":
                case "unchecked":
                case "unsafe":
                case "ushort":
                case "using":
                case "virtual":
                case "void":
                case "volatile":
                case "while":
                    result = true;
                    goto ReturnResult;
            }

            result = false;

        ReturnResult:
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
            var element = node.GetChild(0);

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
