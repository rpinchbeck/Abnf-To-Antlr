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
    public class DefinedAsTests : FileDrivenTestBase
    {
        [TestMethod]
        public void Definition_Followed_By_One_Incremental_Alternative()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void Definition_Followed_By_Two_Incremental_Alternatives()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void Definition_Preceded_By_One_Incremental_Alternative()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void Definition_Preceded_By_Two_Incremental_Alternatives()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void Definition_Preceded_And_Followed_By_One_Incremental_Alternative()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void Definition_Preceded_And_Followed_By_Two_Incremental_Alternatives()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void Incremental_Aliases_Do_Not_Conflict_With_Existing_Rules()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }
    }
}

