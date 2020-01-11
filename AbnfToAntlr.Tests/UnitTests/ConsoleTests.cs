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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AbnfToAntlr.Tests
{
    [TestClass]
    public class ConsoleTests : FileDrivenTestBase
    {
        /////////////////////////////////////
        // Show help (-h, /h, --h)
        /////////////////////////////////////

        [TestMethod]
        public void Show_Help_Using_Dash_H()
        {
            PerformConsoleTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name, ExpectedReturnValueEnum.SyntaxError);
        }

        [TestMethod]
        public void Show_Help_Using_Slash_H()
        {
            PerformConsoleTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name, ExpectedReturnValueEnum.SyntaxError);
        }

        [TestMethod]
        public void Show_Help_Using_Dash_Dash_H()
        {
            PerformConsoleTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name, ExpectedReturnValueEnum.SyntaxError);
        }

        /////////////////////////////////////
        // Show help (-?, /?, --?)
        /////////////////////////////////////

        [TestMethod]
        public void Show_Help_Using_Dash_Question_Mark()
        {
            PerformConsoleTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name, ExpectedReturnValueEnum.SyntaxError);
        }

        [TestMethod]
        public void Show_Help_Using_Slash_Question_Mark()
        {
            PerformConsoleTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name, ExpectedReturnValueEnum.SyntaxError);
        }

        [TestMethod]
        public void Show_Help_Using_Dash_Dash_QuestionMark()
        {
            PerformConsoleTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name, ExpectedReturnValueEnum.SyntaxError);
        }

        /////////////////////////////////////
        // Show help (-help, /help, --help)
        /////////////////////////////////////

        [TestMethod]
        public void Show_Help_Using_Dash_Help()
        {
            PerformConsoleTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name, ExpectedReturnValueEnum.SyntaxError);
        }

        [TestMethod]
        public void Show_Help_Using_Slash_Help()
        {
            PerformConsoleTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name, ExpectedReturnValueEnum.SyntaxError);
        }

        [TestMethod]
        public void Show_Help_Using_Dash_Dash_Help()
        {
            PerformConsoleTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name, ExpectedReturnValueEnum.SyntaxError);
        }

        /////////////////////////////////////
        // Direct Translation
        /////////////////////////////////////

        [TestMethod]
        public void Direct_Translation_From_StdIn()
        {
            PerformConsoleTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void Direct_Translation_From_File()
        {
            PerformConsoleTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        /////////////////////////////////////
        // Indirect Translation
        /////////////////////////////////////

        [TestMethod]
        public void Indirect_Translation_From_StdIn()
        {
            PerformConsoleTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void Indirect_Translation_From_File()
        {
            PerformConsoleTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name);
        }

        [TestMethod]
        public void No_Args()
        {
            PerformConsoleTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name, ExpectedReturnValueEnum.SyntaxError);
        }

        [TestMethod]
        public void Recognition_Error_From_StdIn()
        {
            PerformConsoleTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name, ExpectedReturnValueEnum.RecognitionError);
        }

        [TestMethod]
        public void Recognition_Error_From_File()
        {
            PerformConsoleTest(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().ReflectedType.Name, ExpectedReturnValueEnum.RecognitionError);
        }
    }
}
