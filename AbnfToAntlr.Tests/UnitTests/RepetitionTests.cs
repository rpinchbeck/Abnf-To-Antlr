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
    public class RepetitionTests : FileDrivenTestBase
    {
        //////////////////////////////////////////////////
        // Zero 
        //////////////////////////////////////////////////

        [TestMethod]
        public void Repetition_Zero_Occurences()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void Repetition_Zero_To_One_Occurences()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void Repetition_Zero_To_Three_Occurrences()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void Repetition_Zero_To_N_Occurrences()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void Repetition_Zero_Or_More_Occurences()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        //////////////////////////////////////////////////
        // One 
        //////////////////////////////////////////////////

        [TestMethod]
        public void Repetition_One_Occurence()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void Repetition_One_To_One_Occurences()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void Repetition_One_To_Two_Occurrences()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void Repetition_One_To_Three_Occurrences()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void Repetition_One_To_N_Occurrences()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void Repetition_One_Or_More_Occurences()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        //////////////////////////////////////////////////
        // N
        //////////////////////////////////////////////////

        [TestMethod]
        public void Repetition_N_Exact_Occurences()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void Repetition_N_Or_More_Occurences()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void Repetition_M_To_N_Occurrences()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        //////////////////////////////////////////////////
        // Other
        //////////////////////////////////////////////////

        [TestMethod]
        public void Nested_Repetition_Has_Parentheses()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void Repetition_Unspecified_Occurences()
        {
            PerformTranslationTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }


    }
}
