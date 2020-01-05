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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Antlr.Runtime;
using Antlr.Runtime.Tree;

namespace AbnfToAntlr.Common
{
    public class TreeVisitor_GatherRuleStatistics : TreeVisitor
    {
        RuleStatistics _ruleStatistics;

        public TreeVisitor_GatherRuleStatistics(RuleStatistics ruleStatistics)
        {
            _ruleStatistics = ruleStatistics;
        }

        protected void Reset()
        {
            _ruleStatistics.Clear();
        }

        protected override void VisitRuleList(ITree node)
        {
            node.Validate(AbnfAstParser.RULE_LIST_NODE);

            // Start over each time the rule list is visited
            Reset();

            ProcessLeftHandSide(node);

            VisitChildren(node);
        }

        /// <summary>
        /// Scan the speficied tree for rule names and remember them for later name collision handling
        /// </summary>
        protected void ProcessLeftHandSide(ITree ruleListNode)
        {
            ruleListNode.Validate(AbnfAstParser.RULE_LIST_NODE);

            ITree ruleNode;
            ITree ruleNameNode;
            string ruleName;
            string parserRuleName;

            for (int index = 0; index < ruleListNode.ChildCount; index++)
            {
                ruleNode = ruleListNode.GetChildWithValidation(index, AbnfAstParser.RULE_NODE);

                ruleNameNode = ruleNode.GetChildWithValidation(0, AbnfAstParser.RULE_NAME_NODE);

                ruleName = GetRuleName(ruleNameNode);

                parserRuleName = AntlrHelper.GetParserRuleName(ruleName);

                _ruleStatistics.LhsRawRuleNames.Add(ruleName);
                _ruleStatistics.LhsParserRuleNames.Add(parserRuleName);
                _ruleStatistics.AllParserRuleNames.Add(parserRuleName);
            }

            for (int index = 0; index < ruleListNode.ChildCount; index++)
            {
                ruleNode = ruleListNode.GetChildWithValidation(index, AbnfAstParser.RULE_NODE);

                GetOrCreateLhsRuleDetail(ruleNode);
            }
        }

        protected override void VisitRule(ITree node)
        {
            node.Validate(AbnfAstParser.RULE_NODE);

            var definedAsNode = node.GetChildWithValidation(1, AbnfAstParser.DEFINED_AS_NODE);
            var definedAsOperator = GetChildrenText(definedAsNode);

            var ruleDetail = GetOrCreateLhsRuleDetail(node);

            if (definedAsOperator == "=")
            {
                ruleDetail.CountOfDefinitions++;
            }
            else
            {
                ruleDetail.CountOfIncrementalAlternatives++;
            }

            // visit rule body
            ITree element;
            var maxIndex = node.ChildCount;
            for (int index = 1; index < maxIndex; index++)
            {
                element = node.GetChildWithValidation(index);
                Visit(element);
            }
        }

        protected override void VisitRuleName(ITree node)
        {
            node.Validate(AbnfAstParser.RULE_NAME_NODE);

            var ruleDetail = GetOrCreateRhsRuleDetail(node);

            ruleDetail.CountOfReferences++;
        }

        RuleDetail GetOrCreateLhsRuleDetail(ITree ruleNode)
        {
            ruleNode.Validate(AbnfAstParser.RULE_NODE);

            var ruleNameNode = ruleNode.GetChildWithValidation(0);

            var ruleName = GetRuleName(ruleNameNode);

            RuleDetail result;

            if (_ruleStatistics.RuleDetails.TryGetValue(ruleName, out result))
            {
                // do nothing, rule details already recorded
            }
            else
            {
                // determine alias
                var parserRuleName = AntlrHelper.GetParserRuleName(ruleName);

                var alias = parserRuleName;

                int counter = 0;

                while (AntlrHelper.IsReservedKeyWord(alias) || _ruleStatistics.Aliases.Contains(alias))
                {
                    counter++;
                    alias = parserRuleName + "_" + counter;

                    while (_ruleStatistics.AllParserRuleNames.Contains(alias))
                    {
                        counter++;
                        alias = parserRuleName + "_" + counter;
                    }
                }

                var isLexerRule = (!(ContainsAnyRuleName(ruleNode, _ruleStatistics.LhsRawRuleNames)));

                result = new RuleDetail
                {
                    Name = ruleName,
                    Alias = alias,
                    IsLexerRule = isLexerRule
                };

                _ruleStatistics.Aliases.Add(alias);
                _ruleStatistics.LhsRawRuleNames.Add(ruleName);
                _ruleStatistics.AllRawRuleNames.Add(ruleName);
                _ruleStatistics.LhsParserRuleNames.Add(parserRuleName);
                _ruleStatistics.AllParserRuleNames.Add(parserRuleName);
                _ruleStatistics.RuleDetails.Add(ruleName, result);
            }

            return result;
        }

        RuleDetail GetOrCreateRhsRuleDetail(ITree ruleNameNode)
        {
            ruleNameNode.Validate(AbnfAstParser.RULE_NAME_NODE);

            var ruleName = GetRuleName(ruleNameNode);

            RuleDetail result;

            if (_ruleStatistics.RuleDetails.TryGetValue(ruleName, out result))
            {
                // do nothing, rule details already recorded
            }
            else
            {
                // if we wind up here, then we have an unmatched rule reference
                // (there are no rule definitions matching this rule reference)
                // determine alias
                var parserRuleName = AntlrHelper.GetParserRuleName(ruleName);

                var alias = parserRuleName;

                int counter = 0;

                while (AntlrHelper.IsReservedKeyWord(alias) || _ruleStatistics.Aliases.Contains(alias))
                {
                    counter++;
                    alias = parserRuleName + "_" + counter;

                    while (_ruleStatistics.AllParserRuleNames.Contains(alias))
                    {
                        counter++;
                        alias = parserRuleName + "_" + counter;
                    }
                }

                result = new RuleDetail
                {
                    Name = ruleName,
                    Alias = alias,
                    IsLexerRule = false // treat unmatched rule references as parser rules
                };

                _ruleStatistics.Aliases.Add(alias);
                _ruleStatistics.RhsRawRuleNames.Add(ruleName);
                _ruleStatistics.AllRawRuleNames.Add(ruleName);
                _ruleStatistics.RhsParserRuleNames.Add(parserRuleName);
                _ruleStatistics.AllParserRuleNames.Add(parserRuleName);
                _ruleStatistics.RuleDetails.Add(ruleName, result);
            }

            return result;
        }
    }
}
