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

using AbnfToAntlr.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AbnfToAntlr.Tests
{
    [TestClass]
    public class RecognitionErrorTests : FileDrivenTestBase
    {
        [TestMethod]
        [ExpectedException(typeof(AbnfToAntlr.Common.TranslationException))]
        public void RuleWithoutAssignment()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(AbnfToAntlr.Common.TranslationException))]
        public void RuleWithBadAssignment()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(AbnfToAntlr.Common.TranslationException))]
        public void RuleWithTrailingQuotedString()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }
    }
}
