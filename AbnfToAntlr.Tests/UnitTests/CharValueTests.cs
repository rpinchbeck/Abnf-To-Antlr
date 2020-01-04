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
using System.Text;
using System.Reflection;

namespace AbnfToAntlr.Tests
{
    [TestClass]
    public class CharValTests  : FileDrivenTestBase
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // char-val tests
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [TestMethod]
        public void CharVal_With_No_Characters_Is_Empty()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void CharVal_With_One_Character_Is_Case_Insentitive()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void CharVal_With_Two_Characters_Is_Case_Insentitive()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void CharVal_With_One_Digit_Has_No_Alternatives()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void CharVal_With_Two_Digits_Has_No_Alternatives()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // case-sensitive-string tests
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [TestMethod]
        public void CaseSensitiveString_With_No_Characters_Is_Empty()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void CaseSensitiveString_With_One_Character_Is_Case_Insentitive()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void CaseSensitiveString_With_Two_Characters_Is_Case_Insentitive()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void CaseSensitiveString_With_One_Digit_Has_No_Alternatives()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void CaseSensitiveString_With_Two_Digits_Has_No_Alternatives()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // case-insensitive-string tests
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [TestMethod]
        public void CaseInsensitiveString_With_No_Characters_Is_Empty()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void CaseInsensitiveString_With_One_Character_Is_Case_Insentitive()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void CaseInsensitiveString_With_Two_Characters_Is_Case_Insentitive()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void CaseInsensitiveString_With_One_Digit_Has_No_Alternatives()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void CaseInsensitiveString_With_Two_Digits_Has_No_Alternatives()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }
    }
}
