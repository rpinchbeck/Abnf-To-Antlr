/*

    Copyright 2018 Robert Pinchbeck
  
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

using AbnfToAntlr.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbnfToAntlr.Tests
{
    [TestClass]
    public class RuleNameTests  : TranslatorTestBase
    {
        [TestMethod]
        public void RuleNames_AreCaseInsensitive_ForIndirectTranslation()
        {
            IndirectTranslationTest(
                (inputBuilder) =>
                {
                    inputBuilder.AppendLine("someRule = %d65");
                    inputBuilder.AppendLine("someOtherRule = someRULE");
                },
                (expectedBuilder) =>
                {
                    expectedBuilder.AppendLine(@"somerule : CAP_A;");
                    expectedBuilder.AppendLine(@"someotherrule : somerule;");
                    expectedBuilder.AppendLine(@"");
                    expectedBuilder.AppendLine(@"//////////////////////////////////////////////////////////////////////////");
                    expectedBuilder.AppendLine(@"// Lexer rules generated for each distinct character in original grammar");
                    expectedBuilder.AppendLine(@"// per http://www.unicode.org/charts/PDF/U0000.pdf");
                    expectedBuilder.AppendLine(@"//////////////////////////////////////////////////////////////////////////");
                    expectedBuilder.AppendLine(@"");
                    expectedBuilder.AppendLine(@"CAP_A : 'A';");
                }
            );
        }

        [TestMethod]
        public void RuleNames_AreCaseInsensitive_ForDirectTranslation()
        {
            DirectTranslationTest(
                (inputBuilder) =>
                {
                    inputBuilder.AppendLine(@"someRule = %d65");
                    inputBuilder.AppendLine(@"someOtherRule = someRULE");
                },
                (expectedBuilder) =>
                {
                    expectedBuilder.AppendLine(@"SOMERULE : '\u0041';");
                    expectedBuilder.AppendLine(@"someotherrule : SOMERULE;");
                }
            );
        }
    }
}
