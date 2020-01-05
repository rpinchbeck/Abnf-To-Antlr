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

using Antlr.Runtime;
using Antlr.Runtime.Tree;

namespace AbnfToAntlr.Common
{
    public abstract class TreeVisitor_OutputTranslation : TreeVisitor
    {

        HashSet<string> _allIncrementalAliases = new HashSet<string>();
        Dictionary<string, string[]> _mapRuleNameToIncrementalAliases = new Dictionary<string, string[]>();
        Dictionary<string, int> _lhsIncrementalRuleCounters = new Dictionary<string, int>();


        protected System.IO.TextWriter _writer;            // writer to output the translated grammar
        protected ITokenStream _tokens;                    // the token stream output from the AbnfAstLexer class
        protected INamedCharacterLookup _lookup;
        protected RuleStatistics _ruleStatistics;

        protected HashSet<IToken> _processedTokens;        // keep track of whitespace/comment tokens which have already been output
        protected HashSet<ITree> _visitedNodes;            // keep track of whitespace/comment tokens which have already been output
        protected ITree _finalRuleElement;                 // keep track of the final node in the tree (the final node is the one immediately before any trailing whitespace in the token stream)
        protected int _finalNonWhiteSpaceTokenIndex;       // keep track of the final non-whitespace token in the token stream
        protected int _finalNonWhiteSpaceTokenIndexInRule; // keep track of the final non-whitespace token in the current rule
        int _nestedRepetitionCount = 0;                    // keep track of nested repetitions (for preserving whitespace)

        public TreeVisitor_OutputTranslation(ITokenStream tokens, System.IO.TextWriter writer, INamedCharacterLookup lookup, RuleStatistics ruleStatistics)
        {
            _tokens = tokens;
            _writer = writer;
            _lookup = lookup;
            _ruleStatistics = ruleStatistics;

            Reset();
        }

        protected void Reset()
        {
            // initialize
            _processedTokens = new HashSet<IToken>();
            _visitedNodes = new HashSet<ITree>();
            _finalRuleElement = null;

            _finalNonWhiteSpaceTokenIndex = FindStartOfWhiteSpaceBlock(_tokens.Count) - 1;
            _finalNonWhiteSpaceTokenIndexInRule = _finalNonWhiteSpaceTokenIndex;
            _nestedRepetitionCount = 0;

            _allIncrementalAliases.Clear();
            _mapRuleNameToIncrementalAliases.Clear();
            _lhsIncrementalRuleCounters.Clear();

            GatherIncrementalAliases();
        }

        protected abstract void WriteCharValNode(ITree node);

        protected abstract void WriteValue(int value);

        protected abstract string GetLexerRuleName(string alias);


        protected override void BeforeVisit(Antlr.Runtime.Tree.ITree node)
        {
            WriteWhiteSpaceBeforeNode(node);
        }

        protected override void AfterVisit(Antlr.Runtime.Tree.ITree node)
        {
            _visitedNodes.Add(node);

            if (node.TokenStopIndex == _finalNonWhiteSpaceTokenIndex)
            {
                _finalRuleElement = node;
            }
        }

        protected override void VisitRuleList(ITree node)
        {
            Reset();

            VisitChildren(node);
        }

        protected override void VisitRule(ITree node)
        {
            WriteRuleNode(node);
        }

        protected override void VisitRuleName(ITree node)
        {
            WriteRuleNameNode(node);
        }

        protected override void VisitAlternation(ITree node)
        {
            WriteChildren(node, "|");
        }

        protected override void VisitConcatenation(ITree node)
        {
            WriteChildren(node);
        }

        protected override void VisitGroup(ITree node)
        {
            WriteGroupNode(node);
        }

        protected override void VisitOption(ITree node)
        {
            WriteOptionNode(node);
        }

        protected override void VisitRepetition(ITree node)
        {
            WriteRepetitionNode(node);
        }

        protected override void VisitCharVal(ITree node)
        {
            WriteCharValNode(node);
        }

        protected override void VisitBinVal(ITree node)
        {
            VisitChildren(node);
        }

        protected override void VisitBinValNumber(ITree node)
        {
            WriteBinValNumberNode(node);
        }

        protected override void VisitBinValRange(ITree node)
        {
            WriteValueRangeNode(node);
        }

        protected override void VisitBinValConcat(ITree node)
        {
            WriteValueConcatNode(node);
        }

        protected override void VisitDecVal(ITree node)
        {
            VisitChildren(node);
        }

        protected override void VisitDecValNumber(ITree node)
        {
            WriteDecValNumberNode(node);
        }

        protected override void VisitDecValRange(ITree node)
        {
            WriteValueRangeNode(node);
        }

        protected override void VisitDecValConcat(ITree node)
        {
            WriteValueConcatNode(node);
        }

        protected override void VisitHexVal(ITree node)
        {
            VisitChildren(node);
        }

        protected override void VisitHexValNumber(ITree node)
        {
            WriteHexValNumberNode(node);
        }

        protected override void VisitHexValRange(ITree node)
        {
            WriteValueRangeNode(node);
        }

        protected override void VisitHexValConcat(ITree node)
        {
            WriteValueConcatNode(node);
        }

        protected override void VisitProseVal(ITree node)
        {
            WriteProseValNode(node);
        }

        /// <summary>
        /// Output the ANTLR translation of the children of the specified node
        /// </summary>
        protected void WriteChildren(ITree node, string separator)
        {
            ITree child;
            bool isSpecialConcatenationCase = false;

            // detect the special case when multiple concatenations are contained within an alternation rule which also has multiple alternatives
            // (in ABNF, the concatenation rule has higher precedence than the alternation rule; therefore, surround the concatenation with parentheses)
            //
            // for example:   somerule = concatenation1 concatenation2 / concatenation3 concatenation4
            // translates to: somerule : (concatenation1 concatenation2) / (concatenation3 concatenation4)
            if (node.Type == AbnfAstParser.CONCATENATION_NODE && node.ChildCount > 1)
            {
                var parent = node.Parent;

                if (parent.Type == AbnfAstParser.ALTERNATION_NODE && parent.ChildCount > 1)
                {
                    isSpecialConcatenationCase = true;
                }
            }

            if (isSpecialConcatenationCase)
            {
                // in ABNF, the concatenation rule has higher precedence than the alternation rule; therefore, surround the concatenation with parentheses
                Write("(");
            }

            var maxIndex = node.ChildCount;
            var lastIndex = maxIndex - 1;

            for (int index = 0; index < maxIndex; index++)
            {
                if (index > 0)
                {
                    Write(separator);
                }

                child = node.GetChildWithValidation(index);

                Visit(child);

                if (index == lastIndex && isSpecialConcatenationCase)
                {
                    // in ABNF, the concatenation rule has higher precedence than the alternation rule; therefore, surround the concatenation with parentheses
                    Write(")");
                }

                // preserve whitespace and comments
                if (child.TokenStopIndex < _finalNonWhiteSpaceTokenIndexInRule)
                {
                    WriteWhiteSpaceAfterNode(child);
                }

            }
        }

        /// <summary>
        /// Output the ANTLR translation of the children of the specified node
        /// </summary>
        protected void WriteChildren(ITree node)
        {
            if (_nestedRepetitionCount > 0 && _visitedNodes.Contains(node))
            {
                WriteChildren(node, " ");
            }
            else
            {
                WriteChildren(node, "");
            }
        }

        /// <summary>
        /// Output the ANTLR translation of the specified rule node
        /// </summary>
        protected void WriteRuleNode(ITree node)
        {
            var ruleNameNode = node.GetChildWithValidation(0);
            var ruleName = GetRuleName(ruleNameNode);
            var ruleDetail = _ruleStatistics.RuleDetails[ruleName];

            var definedAsNode = node.GetChildWithValidation(1);
            var definedAsOperator = GetChildrenText(definedAsNode);

            _finalNonWhiteSpaceTokenIndexInRule = FindStartOfWhiteSpaceBlock(node.TokenStopIndex + 1) - 1;

            var ruleAlias = GetLhsAlias(ruleName, definedAsOperator);

            // never output uppercase rule names when there are incremental alternatives 
            // because they will be rewritten later as parser rules
            if (ruleDetail.CountOfIncrementalAlternatives > 0)
            {
                var incrementalIndex = _lhsIncrementalRuleCounters[ruleName];
                var lastIncrementalIndex = ruleDetail.CountOfIncrementalAlternatives - 1;

                if (definedAsOperator == "=" || incrementalIndex < lastIncrementalIndex)
                {
                    ruleAlias = ruleAlias.ToLowerInvariant();
                }
            }

            Write(ruleAlias);

            WriteWhiteSpaceAfterNode(ruleNameNode);

            Write(":");

            WriteWhiteSpaceAfterNode(definedAsNode.GetChildWithValidation(0));

            bool ruleBodyNeedsParentheses = false;

            // parentheses are needed for some rules with incremental alternatives
            if (ruleDetail.CountOfIncrementalAlternatives > 0)
            {
                var incrementalIndex = _lhsIncrementalRuleCounters[ruleName];
                var lastIncrementalIndex = ruleDetail.CountOfIncrementalAlternatives - 1;

                ruleBodyNeedsParentheses = (incrementalIndex < lastIncrementalIndex || definedAsOperator == "=");
            }

            if (ruleBodyNeedsParentheses)
            {
                Write("(");
            }

            // write rule body
            ITree element;
            var maxIndex = node.ChildCount;
            for (int index = 1; index < maxIndex; index++)
            {
                element = node.GetChildWithValidation(index);
                Visit(element);
            }

            if (ruleBodyNeedsParentheses)
            {
                Write(")");
            }

            // append reference to incremental alternative rule (if any)
            if (ruleDetail.CountOfIncrementalAlternatives > 0)
            {
                var incrementalIndex = _lhsIncrementalRuleCounters[ruleName];
                var lastIncrementalIndex = ruleDetail.CountOfIncrementalAlternatives - 1;

                if (definedAsOperator == "=")
                {
                    Write(" | ");
                    Write(_mapRuleNameToIncrementalAliases[ruleName][0]);
                }
                else
                {
                    if (incrementalIndex < lastIncrementalIndex)
                    {
                        Write(" | ");
                        Write(_mapRuleNameToIncrementalAliases[ruleName][incrementalIndex + 1]);
                    }

                    _lhsIncrementalRuleCounters[ruleName]++;
                }

            }

            Write(";");

            // if this is the final rule, then output all whitespace and comments after it
            if (_finalRuleElement != null)
            {
                WriteWhiteSpaceAfterNode(_finalRuleElement);
            }
        }

        /// <summary>
        /// Output the ANTLR translation of the specified rule name node
        /// </summary>
        protected void WriteRuleNameNode(ITree node)
        {
            var ruleName = GetRuleName(node);

            var alias = GetRhsAlias(ruleName);

            Write(alias);
        }

        string GetLhsAlias(string ruleName, string definedAsOperator)
        {
            string result;

            var ruleDetail = _ruleStatistics.RuleDetails[ruleName];

            if (definedAsOperator == "=/")
            {
                var index = _lhsIncrementalRuleCounters[ruleName];

                var incrementalAliases = _mapRuleNameToIncrementalAliases[ruleName];

                result = incrementalAliases[index];
            }
            else
            {
                result = GetAlias(ruleName);
            }

            return result;
        }

        string GetRhsAlias(string ruleName)
        {
            var result = GetAlias(ruleName);

            return result;
        }

        string GetAlias(string ruleName)
        {
            var ruleDetails = _ruleStatistics.RuleDetails[ruleName];

            var result = ruleDetails.Alias;

            if (ruleDetails.IsLexerRule)
            {
                result = GetLexerRuleName(result);
            }

            return result;
        }

        /// <summary>
        /// Output the ANTLR translation of the specified group node
        /// </summary>
        protected void WriteGroupNode(ITree node)
        {
            Write("(");
            WriteChildren(node);
            Write(")");
        }

        /// <summary>
        /// Output the ANTLR translation of the specified prose val node
        /// </summary>
        private void WriteProseValNode(ITree node)
        {
            var proseVal = GetProseVal(node);

            var proseValAsRuleName = proseVal.ToLowerInvariant();

            if (_ruleStatistics.LhsRawRuleNames.Contains(proseVal))
            {
                var ruleDetail = _ruleStatistics.RuleDetails[proseValAsRuleName];

                var alias = ruleDetail.Alias;

                if (ruleDetail.IsLexerRule)
                {
                    alias = GetLexerRuleName(alias);
                }

                Write(alias);
            }
            else
            {
                WriteCharValNode(node);
            }
        }

        /// <summary>
        /// Output the ANTLR translation of the specified option node
        /// </summary>
        protected void WriteOptionNode(ITree node)
        {
            Write("(");
            WriteChildren(node);
            Write(")?");
        }

        /// <summary>
        /// Output the ANTLR translation of the specified repetition node
        /// </summary>
        protected void WriteRepetitionNode(ITree node)
        {
            var element = node.GetChildWithValidation(0);
            var min = node.GetChildWithValidation(1);
            var max = node.GetChildWithValidation(2);

            int minValue;
            int maxValue;

            switch (min.Type)
            {
                case AbnfAstParser.ZERO_OCCURENCES:
                    minValue = 0;
                    break;
                case AbnfAstParser.ONE_OCCURENCE:
                    minValue = 1;
                    break;
                default:
                    // min = number
                    var minText = GetChildrenText(min);
                    minValue = Int32.Parse(minText, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture);
                    break;
            }

            switch (max.Type)
            {
                case AbnfAstParser.EXACT_OCCURENCES:
                    if (minValue == 0)
                    {
                        // do nothing
                    }
                    else if (minValue == 1)
                    {
                        Visit(element);
                    }
                    else
                    {
                        Write("(");

                        _nestedRepetitionCount++;

                        for (int count = 0; count < minValue; count++)
                        {
                            if (count > 0)
                            {
                                Write(" ");
                            }

                            Visit(element);
                        }

                        _nestedRepetitionCount--;

                        Write(")");
                    }

                    break;

                case AbnfAstParser.ORMORE_OCCURENCES:
                    if (minValue == 0)
                    {
                        Visit(element);
                        Write("*");
                    }
                    else if (minValue == 1)
                    {
                        Visit(element);
                        Write("+");
                    }
                    else
                    {
                        Write("(");

                        _nestedRepetitionCount++;

                        for (int count = 0; count < minValue; count++)
                        {
                            if (count > 0)
                            {
                                Write(" ");
                            }

                            Visit(element);
                        }

                        _nestedRepetitionCount--;

                        Write("+");

                        Write(")");
                    }

                    break;

                default:
                    // max = number
                    var maxText = GetChildrenText(max);
                    maxValue = Int32.Parse(maxText, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture);

                    _nestedRepetitionCount++;

                    var needsOuterParentheses = (maxValue > 1);
                    
                    if (needsOuterParentheses)
                    {
                        Write("(");
                    }

                    for (int count = 0; count < minValue; count++)
                    {
                        if (count > 0)
                        {
                            Write(" ");
                        }

                        Visit(element);
                    }

                    _nestedRepetitionCount--;

                    var needsInnerParentheses = (maxValue - minValue > 1);

                    if (minValue > 0 && maxValue > minValue)
                    {
                        Write(" ");
                    }

                    if (needsInnerParentheses)
                    {
                        Write("(");
                    }

                    _nestedRepetitionCount++;

                    for (int count = maxValue; count > minValue; count--)
                    {
                        if (count < maxValue)
                        {
                            Write(" | ");
                        }

                        if (count - minValue == 1)
                        {
                            if (minValue > 0 && !needsInnerParentheses)
                            {
                                Write(" ");
                            }

                            Visit(element);
                            Write("?");
                        }
                        else
                        {
                            Write("(");

                            _nestedRepetitionCount++;

                            for (int subCount = minValue; subCount < count; subCount++)
                            {
                                if (subCount > minValue)
                                {
                                    Write(" ");
                                }

                                Visit(element);
                            }

                            _nestedRepetitionCount--;

                            Write(")");
                        }
                    }

                    _nestedRepetitionCount--;

                    if (needsInnerParentheses)
                    {
                        Write(")");
                    }

                    if (needsOuterParentheses)
                    {
                        Write(")");
                    }

                    break;
            }
        }


        protected void WriteValueNode(ITree node)
        {
            int value = GetValue(node);
            WriteValue(value);
        }

        /// <summary>
        /// Output the ANTLR translation of the specified binary value number node
        /// </summary>
        protected void WriteBinValNumberNode(ITree node)
        {
            WriteValueNode(node);
        }

        /// <summary>
        /// Output the ANTLR translation of the specified decimal value number node
        /// </summary>
        protected void WriteDecValNumberNode(ITree node)
        {
            WriteValueNode(node);
        }

        /// <summary>
        /// Output the ANTLR translation of the specified hexadecimal value number node
        /// </summary>
        protected void WriteHexValNumberNode(ITree node)
        {
            WriteValueNode(node);
        }

        protected abstract void WriteValueRangeNode(ITree node);

        /// <summary>
        /// Output the ANTLR translation of the specified numeric value concatenation node
        /// </summary>
        protected void WriteValueConcatNode(ITree node)
        {
            Write("(");
            WriteChildren(node, " ");
            Write(")");
        }

        /// <summary>
        /// Output the specified text
        /// </summary>
        protected void Write(string text)
        {
            _writer.Write(text);
        }

        /// <summary>
        /// Output all unprocessed whitespace tokens to the left of a given node
        /// </summary>
        protected void WriteWhiteSpaceBeforeNode(ITree node)
        {
            int minIndex = FindStartOfWhiteSpaceBlock(node.TokenStartIndex);
            int maxIndex = node.TokenStartIndex - 1;

            WriteWhiteSpace(minIndex, maxIndex);
        }


        /// <summary>
        /// Output all unprocessed whitespace tokens to the right of a given node
        /// </summary>
        protected void WriteWhiteSpaceAfterNode(ITree node)
        {
            int minIndex = node.TokenStopIndex + 1;
            int maxIndex = FindWhiteSpaceBlockIndex(node.TokenStopIndex, 1);

            WriteWhiteSpace(minIndex, maxIndex);
        }

        /// <summary>
        /// Output all unprocessed whitespace tokens in the given token stream index range
        /// </summary>
        protected void WriteWhiteSpace(int minIndex, int maxIndex)
        {
            IToken token;
            for (int index = minIndex; index <= maxIndex; index++)
            {
                token = _tokens.Get(index);

                if (IsWhiteSpace(token))
                {
                    WriteWhitespaceToken(token);
                }
            }
        }

        /// <summary>
        /// Output specified whitespace token if it has not already been processed
        /// </summary>
        protected void WriteWhitespaceToken(IToken token)
        {
            if (!(IsWhiteSpace(token)))
            {
                throw new InvalidOperationException("Non-whitespace token encountered");
            }

            if (_processedTokens.Contains(token))
            {
                // token already processed, do nothing 
                // (whitespace/comment tokens should only be output once)
            }
            else
            {
                _processedTokens.Add(token);

                var text = token.Text;

                if (token.Type == AbnfAstLexer.WSP || token.Type == AbnfAstLexer.CRLF)
                {
                    Write(text);
                }

                if (token.Type == AbnfAstLexer.COMMENT)
                {
                    Write("//");
                    Write(text.Substring(1));
                }
            }
        }

        /// <summary>
        /// Determine the index of the outermost whitespace token
        /// </summary>
        /// <param name="index">the index in the token stream to begin searching</param>
        /// <param name="direction">-1 to search left, 1 to search right</param>
        /// <returns>the index of the outermost whitespace token found</returns>
        protected int FindWhiteSpaceBlockIndex(int index, int direction)
        {
            int maxIndex;
            IToken token;
            int result;

            maxIndex = _tokens.Count - 1;
            index += direction;

            while (index >= 0 && index <= maxIndex)
            {
                token = _tokens.Get(index);

                if (IsWhiteSpace(token))
                {
                    // do nothing
                }
                else
                {
                    result = index - direction;
                    goto ReturnResult;
                }

                index += direction;
            }

            if (direction == -1)
            {
                result = 0;
                goto ReturnResult;
            }

            result = maxIndex;

        ReturnResult:
            return result;
        }

        /// <summary>
        /// Determine the start of a whitespace block
        /// </summary>
        /// <param name="index">the index in the token stream to begin searching</param>
        /// <returns>the index of the whitespace block</returns>
        protected int FindStartOfWhiteSpaceBlock(int index)
        {
            return FindWhiteSpaceBlockIndex(index, -1);
        }

        /// <summary>
        /// Determine the end of a whitespace block
        /// </summary>
        /// <param name="index">the index in the token stream to begin searching</param>
        /// <returns>the index of the whitespace block</returns>
        protected int FindEndOfWhiteSpaceBlock(int index)
        {
            return FindWhiteSpaceBlockIndex(index, 1);
        }

        /// <summary>
        /// Determine if the specified token is a whitespace token
        /// </summary>
        /// <returns>true if the specified token is a whitespace token, false otherwise</returns>
        protected bool IsWhiteSpace(IToken token)
        {
            bool result;

            if (token.Type == AbnfAstLexer.WSP)
            {
                result = true;
                goto ReturnResult;
            }

            if (token.Type == AbnfAstLexer.CRLF)
            {
                result = true;
                goto ReturnResult;
            }

            if (token.Type == AbnfAstLexer.COMMENT)
            {
                result = true;
                goto ReturnResult;
            }

            if (token.Type == AbnfAstLexer.EOF)
            {
                result = true;
                goto ReturnResult;
            }

            result = false;

        ReturnResult:

            return result;
        }

        public void GatherIncrementalAliases()
        {
            foreach (var ruleName in _ruleStatistics.LhsRawRuleNames)
            {
                var ruleDetail = _ruleStatistics.RuleDetails[ruleName];

                var countOfIncrementalAlternatives = ruleDetail.CountOfIncrementalAlternatives;

                if (countOfIncrementalAlternatives > 0)
                {
                    var incrementalAliases = new string[countOfIncrementalAlternatives];

                    var lastIndex = countOfIncrementalAlternatives - 1;

                    for (int index = 0; index < countOfIncrementalAlternatives; index++)
                    {
                        // determine alias
                        var parserRuleName = AntlrHelper.GetParserRuleName(ruleName);

                        var alias = parserRuleName;

                        int counter = 0;

                        while (AntlrHelper.IsReservedKeyWord(alias) || _ruleStatistics.Aliases.Contains(alias) || _allIncrementalAliases.Contains(alias))
                        {
                            counter++;
                            alias = parserRuleName + "_" + counter;

                            while (_ruleStatistics.AllParserRuleNames.Contains(alias))
                            {
                                counter++;
                                alias = parserRuleName + "_" + counter;
                            }
                        }

                        _allIncrementalAliases.Add(alias);

                        if (index == lastIndex)
                        {
                            if (ruleDetail.IsLexerRule)
                            {
                                alias = GetLexerRuleName(alias);
                            }
                        }

                        incrementalAliases[index] = alias;
                    }

                    _mapRuleNameToIncrementalAliases.Add(ruleName, incrementalAliases);
                    _lhsIncrementalRuleCounters[ruleName] = 0;
                }
            }
        }
    }
}
