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
    public abstract class TreeVisitor_OutputTranslation : TreeVisitor
    {
        protected System.IO.TextWriter _writer;            // writer to output the translated grammar
        protected ITokenStream _tokens;                    // the token stream output from the AbnfAstLexer class
        protected INamedCharacterLookup _lookup;

        protected HashSet<string> _rawTranslatedRuleNames; // remember rule names for later name collision handling
        protected HashSet<IToken> _processedTokens;        // keep track of whitespace/comment tokens which have already been output
        protected HashSet<ITree> _visitedNodes;            // keep track of whitespace/comment tokens which have already been output
        protected Dictionary<string, string> _ruleNameMap; // map original rule names to new rule names
        protected ITree _finalRuleElement;                 // keep track of the final node in the tree (the final node is the one immediately before any trailing whitespace in the token stream)
        protected int _finalNonWhiteSpaceTokenIndex;       // keep track of the final non-whitespace token in the token stream
        protected int _finalNonWhiteSpaceTokenIndexInRule; // keep track of the final non-whitespace token in the current rule
        int _nestedRepetitionCount = 0;                    // keep track of nested repetitions (for preserving whitespace)

        public TreeVisitor_OutputTranslation(ITokenStream tokens, System.IO.TextWriter writer, INamedCharacterLookup lookup)
        {
            _tokens = tokens;
            _writer = writer;
            _lookup = lookup;

            CommonConstructor();
        }

        protected void CommonConstructor()
        {
            // initialize
            _ruleNameMap = new Dictionary<string, string>();
            _processedTokens = new HashSet<IToken>();
            _visitedNodes = new HashSet<ITree>();
            _finalRuleElement = null;

            _finalNonWhiteSpaceTokenIndex = FindStartOfWhiteSpaceBlock(_tokens.Count) - 1;
            _finalNonWhiteSpaceTokenIndexInRule = _finalNonWhiteSpaceTokenIndex;
        }

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
            CommonConstructor();

            CreateRuleNamesCollection(node);

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
            WriteCharValNode(node);
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
            if (node.Type == AbnfAstParser.CONCATENATION_NODE)
            {
                if (node.ChildCount > 1 && node.Parent.ChildCount > 1)
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
            for (int index = 0; index < maxIndex; index++)
            {
                if (index > 0)
                {
                    Write(separator);
                }

                child = node.GetChild(index);

                Visit(child);

                if (isSpecialConcatenationCase)
                {
                    if (index == maxIndex - 1)
                    {
                        // in ABNF, the concatenation rule has higher precedence than the alternation rule; therefore, surround the concatenation with parentheses
                        Write(")");
                    }
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
            ITree element = null;
            var rulename = node.GetChild(0);

            _finalNonWhiteSpaceTokenIndexInRule = FindStartOfWhiteSpaceBlock(node.TokenStopIndex + 1) - 1;

            Visit(rulename);

            WriteWhiteSpaceAfterNode(rulename);

            Write(":");

            var maxIndex = node.ChildCount;
            for (int index = 1; index < maxIndex; index++)
            {
                element = node.GetChild(index);
                Visit(element);
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
            string mappedText;
            string ruleNameText;

            ruleNameText = GetChildrenText(node);

            // is rulename already mapped to a new name?
            if (_ruleNameMap.ContainsKey(ruleNameText))
            {
                // rulename is already mapped, so use the mapped name...
                mappedText = _ruleNameMap[ruleNameText];
            }
            else
            {
                // rulename is not mapped, so map it now...

                mappedText = TranslateRuleName(ruleNameText);

                mappedText = GetUniqueRuleName(mappedText);

                // remember the mapping of the original rule name to the new rule name
                _ruleNameMap[ruleNameText] = mappedText;
            }

            mappedText = ProcessMappedRuleName(ruleNameText, mappedText);

            Write(mappedText);
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
            var element = node.GetChild(0);
            var min = node.GetChild(1);
            var max = node.GetChild(2);

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
                    if (minValue == 1)
                    {
                        Visit(element);
                    }
                    else
                    {
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
                    }

                    break;

                default:
                    // max = number
                    var maxText = GetChildrenText(max);
                    maxValue = Int32.Parse(maxText, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture);

                    _nestedRepetitionCount++;

                    for (int count = 0; count < minValue; count++)
                    {
                        if (count > 0)
                        {
                            Write(" ");
                        }

                        Visit(element);
                    }

                    if (minValue < maxValue)
                    {
                        Write(" (");
                    }

                    for (int index = minValue; index < maxValue; index++)
                    {
                        if (index > minValue)
                        {
                            Write(" ");
                        }

                        Visit(element);
                        Write("?");
                    }

                    _nestedRepetitionCount--;

                    if (minValue < maxValue)
                    {
                        Write(")");
                    }

                    break;
            }
        }


        protected abstract void WriteCharValNode(ITree node);

        protected abstract void WriteValue(int value);


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
            WriteChildren(node, "|");
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
        /// Scan the speficied tree for rule names and remember them for later name collision handling
        /// </summary>
        protected void CreateRuleNamesCollection(ITree ruleListNode)
        {
            ITree ruleNode;
            ITree ruleNameNode;
            string ruleName;
            string translatedRuleName;

            _rawTranslatedRuleNames = new HashSet<string>();

            for (int index = 0; index < ruleListNode.ChildCount; index++)
            {
                ruleNode = ruleListNode.GetChild(index);

                if (ruleNode.Type != AbnfAstParser.RULE_NODE)
                {
                    throw new InvalidOperationException("Unexpected node type encountered in " + System.Reflection.MethodInfo.GetCurrentMethod().Name + " (rule node expected)");
                }

                ruleNameNode = ruleNode.GetChild(0);

                if (ruleNameNode.Type != AbnfAstParser.RULE_NAME_NODE)
                {
                    throw new InvalidOperationException("Unexpected node type encountered in " + System.Reflection.MethodInfo.GetCurrentMethod().Name + " (rule name node expected)");
                }

                ruleName = GetChildrenText(ruleNameNode);

                translatedRuleName = TranslateRuleName(ruleName);

                if (_rawTranslatedRuleNames.Contains(translatedRuleName))
                {
                    // do nothing
                }
                else
                {
                    _rawTranslatedRuleNames.Add(translatedRuleName);
                }
            }
        }

        /// <summary>
        /// Get ANTLR compatible rule name
        /// </summary>
        /// <returns>ANTLR compatible rule name</returns>
        protected string TranslateRuleName(string ruleName)
        {
            string result;

            result = ruleName;

            // translate all rules into parser rules until they are later proven to be lexer rules
            result = result.ToLowerInvariant();

            // dashes are not allowed in ANTLR rule names (so replace them with underscores)
            result = result.Replace("-", "_");

            return result;
        }

        /// <summary>
        /// Append numeric suffixes to rulename until a unique one is found
        /// </summary>
        /// <param name="startCounter">first numeric suffix to attempt</param>
        /// <returns>unique rule name with numeric suffix</returns>
        protected string GetUniqueRuleName(string ruleName)
        {
            string result;
            int uniqueIndex;

            uniqueIndex = 0;
            result = ruleName;

            while (IsMappedRule(result) || IsReservedKeyWord(result))
            {
                uniqueIndex++;
                result = ruleName + "_" + uniqueIndex;
            }

            return result;
        }

        /// <summary>
        /// Determine if the specified rule name is already related (mapped) to another rule name
        /// </summary>
        protected bool IsMappedRule(string ruleName)
        {
            return (_ruleNameMap.ContainsKey(ruleName) || _ruleNameMap.ContainsValue(ruleName));
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

        /// <summary>
        /// Post-processing of rule name
        /// </summary>
        protected virtual string ProcessMappedRuleName(string ruleNameText, string mappedText)
        {
            // do not modify the specified rule name
            return mappedText;
        }

    } // class
} // namespace
