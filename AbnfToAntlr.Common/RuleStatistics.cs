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

namespace AbnfToAntlr.Common
{
    public class RuleStatistics
    {
        public readonly HashSet<string> Aliases = new HashSet<string>();

        public readonly HashSet<string> LhsRawRuleNames = new HashSet<string>();
        public readonly HashSet<string> RhsRawRuleNames = new HashSet<string>();
        public readonly HashSet<string> AllRawRuleNames = new HashSet<string>();

        public readonly HashSet<string> LhsParserRuleNames = new HashSet<string>();
        public readonly HashSet<string> RhsParserRuleNames = new HashSet<string>();
        public readonly HashSet<string> AllParserRuleNames = new HashSet<string>();

        public readonly Dictionary<string, RuleDetail> RuleDetails = new Dictionary<string, RuleDetail>();

        public void Clear()
        {
            Aliases.Clear();

            LhsRawRuleNames.Clear();
            RhsRawRuleNames.Clear();
            AllRawRuleNames.Clear();

            LhsParserRuleNames.Clear();
            RhsParserRuleNames.Clear();
            AllParserRuleNames.Clear();

            RuleDetails.Clear();
        }
    }
}
