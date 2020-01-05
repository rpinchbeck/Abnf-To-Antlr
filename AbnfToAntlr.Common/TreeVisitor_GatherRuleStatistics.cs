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
            ITree ruleNode;
            ITree ruleNameNode;
            string ruleName;
            string parserRuleName;

            for (int index = 0; index < ruleListNode.ChildCount; index++)
            {
                ruleNode = ruleListNode.GetChildWithValidation(index);

                ruleNode.Validate(AbnfAstParser.RULE_NODE);

                ruleNameNode = ruleNode.GetChildWithValidation(0);

                ruleName = GetRuleName(ruleNameNode);
                _ruleStatistics.LhsRawRuleNames.Add(ruleName);

                parserRuleName = AntlrHelper.GetParserRuleName(ruleName);
                _ruleStatistics.LhsParserRuleNames.Add(parserRuleName);

                _ruleStatistics.AllParserRuleNames.Add(parserRuleName);
            }
        }

        protected override void VisitRule(ITree node)
        {
            var definedAsNode = node.GetChildWithValidation(1);
            var definedAsOperator = GetChildrenText(definedAsNode);

            var ruleDetail = GetOrCreateRuleDetail(node);

            if (definedAsOperator == "=")
            {
                ruleDetail.CountOfDefinitions++;
            }
            else
            {
                ruleDetail.CountOfIncrementalAlternatives++;
            }

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
            var ruleNode = node.Parent;

            var ruleDetail = GetOrCreateRuleDetail(ruleNode);

            ruleDetail.CountOfReferences++;
        }

        RuleDetail GetOrCreateRuleDetail(ITree ruleNode)
        {
            var ruleNameNode = ruleNode.GetChildWithValidation(0);

            var ruleName = GetRuleName(ruleNameNode);

            RuleDetail result;

            if (_ruleStatistics.RuleDetails.TryGetValue(ruleName, out result))
            {
                // do nothing, rule details already recorded
            }
            else
            {
                _ruleStatistics.AllRawRuleNames.Add(ruleName);

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

                _ruleStatistics.Aliases.Add(alias);

                result = new RuleDetail
                {
                    Name = ruleName,
                    Alias = alias,
                    IsLexerRule = (!ContainsAnyRuleName(ruleNode, _ruleStatistics.LhsRawRuleNames))
                };

                _ruleStatistics.RuleDetails.Add(ruleName, result);
            }

            return result;
        }

    }
}
