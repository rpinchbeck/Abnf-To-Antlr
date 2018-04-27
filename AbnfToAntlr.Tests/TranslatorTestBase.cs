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
    public class TranslatorTestBase : TestBase
    {
        const string _translatorPropertyKey = "AbnfToAntlrTranslator-4058297A-0C8D-469D-B862-502846D1939C";

        [TestInitialize]
        public void TestInitialize()
        {
            var translator = new AbnfToAntlrTranslator();

            TestContext.Properties.Add(_translatorPropertyKey, translator);
        }

        AbnfToAntlrTranslator GetTranslator()
        {
            return (AbnfToAntlrTranslator)TestContext.Properties[_translatorPropertyKey];
        }

        protected void IndirectTranslationTest(Action<StringBuilder> inputBuilderAction, Action<StringBuilder> expectedBuilderAction)
        {
            var performDirectTranslation = false;

            TranslationTest(performDirectTranslation, inputBuilderAction, expectedBuilderAction);
        }

        protected void DirectTranslationTest(Action<StringBuilder> inputBuilderAction, Action<StringBuilder> expectedBuilderAction)
        {
            var performDirectTranslation = true;

            TranslationTest(performDirectTranslation, inputBuilderAction, expectedBuilderAction);
        }

        protected void TranslationTest(bool performDirectTranslation, Action<StringBuilder> inputBuilderAction, Action<StringBuilder> expectedBuilderAction)
        {
            var translator = GetTranslator();

            var inputBuilder = new StringBuilder();
            var expectedBuilder = new StringBuilder();

            inputBuilderAction(inputBuilder);
            expectedBuilderAction(expectedBuilder);

            var input = inputBuilder.ToString();
            var actual = translator.Translate(input, performDirectTranslation);

            var expected = expectedBuilder.ToString();

            Assert.AreEqual(expected, actual);
        }
    }
}
