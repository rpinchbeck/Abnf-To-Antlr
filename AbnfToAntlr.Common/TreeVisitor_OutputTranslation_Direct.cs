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
    /// Output the direct translation of the ABNF grammar (Do not substitute lexer rules for each distinct character value in the original grammar).
    /// </summary>
    public class TreeVisitor_OutputTranslation_Direct : TreeVisitor_OutputTranslation
    {
        protected List<string> _lexerRules;

        public TreeVisitor_OutputTranslation_Direct(ITokenStream tokens, System.IO.TextWriter writer)
            : base(tokens, writer)
        {
        }

        /// <summary>
        /// Preprocess rules list: determine unique rule names for all rules, determine which rules are lexer rules
        /// </summary>
        protected override void VisitRuleList(ITree node)
        {
            CommonConstructor();

            CreateRuleNamesCollection(node);
            CreateLexerRulesCollection(node);

            VisitChildren(node);
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
            text = text.Replace(@"\", @"\\"); // escape forwardslashes (order matters, this must be done before escaping anything else)
            text = text.Replace(@"'", @"\'"); // escape apostrophes

            Write("'");
            Write(text);
            Write("'");
        }

        /// <summary>
        /// Output the ANTLR translation of the specified numeric value range node
        /// </summary>
        protected override void WriteValueRangeNode(ITree node)
        {
            var min = node.GetChild(0);
            var max = node.GetChild(1);

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

        /// <summary>
        /// Scan the specified tree for rules that contain only literals
        /// </summary>
        protected void CreateLexerRulesCollection(ITree ruleListNode)
        {
            string ruleName;
            ITree ruleNode;

            _lexerRules = new List<string>();

            for (int index = 0; index < ruleListNode.ChildCount; index++)
            {
                ruleNode = ruleListNode.GetChild(index);

                if (ruleNode.Type != AbnfAstParser.RULE_NODE)
                {
                    throw new InvalidOperationException("Unexpected node type encountered while searching for lexer rules.");
                }

                ruleName = GetChildrenText(ruleNode.GetChild(0));

                if (ContainsRuleName(ruleNode))
                {
                    // do nothing
                }
                else
                {
                    _lexerRules.Add(ruleName);
                }
            }
        }

        /// <summary>
        /// Determine if the specified node contains any rule name nodes
        /// </summary>
        /// <returns>true if the specified node contains a rule name node, false otherwise</returns>
        protected bool ContainsRuleName(ITree node)
        {
            int minIndex;
            ITree child;

            if (node.Type == AbnfAstParser.RULE_NODE)
            {
                minIndex = 1;
            }
            else
            {
                minIndex = 0;
            }

            if (node.Type == AbnfAstParser.RULE_NAME_NODE)
            {
                return true;
            }

            for (int index = minIndex; index < node.ChildCount; index++)
            {
                child = node.GetChild(index);

                if (ContainsRuleName(child))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determine if the specified rule name is a lexer rule
        /// </summary>
        /// <returns>true if the specified rule is a lexer rule, false otherwise</returns>
        protected bool IsLexerRule(string ruleName)
        {
            if (_lexerRules.Contains(ruleName))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Post-processing of mapped rule names
        /// </summary>
        protected override string ProcessMappedRuleName(string ruleNameText, string mappedText)
        {
            string result = mappedText;

            if (IsLexerRule(ruleNameText))
            {
                // capitalize the entire rule name of lexer rules 
                result = mappedText.ToUpperInvariant();
            }

            return result;
        }

    } // class
} // namespace
