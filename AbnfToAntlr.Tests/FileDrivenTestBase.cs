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
using System.Text;

namespace AbnfToAntlr.Tests
{
    public abstract class FileDrivenTestBase : TestBase
    {
        // translator shared by all tests to help find concurrency and coupling problems
        public static AbnfToAntlrTranslator _sharedTranslator = new AbnfToAntlrTranslator();

        protected AbnfToAntlrTranslator GetSharedTranslator()
        {
            return _sharedTranslator;
        }

        protected void PerformTranslationTest(string testName, string folderName)
        {
            PerformTranslationTest(testName, folderName, TranslationEnum.Direct, new AbnfToAntlrTranslator());
            PerformTranslationTest(testName, folderName, TranslationEnum.Indirect, new AbnfToAntlrTranslator());

            // Use the same translator for every test to ensure that translator state does not affect tests
            var translator = GetSharedTranslator();
            PerformTranslationTest(testName, folderName, TranslationEnum.Direct, translator);
            PerformTranslationTest(testName, folderName, TranslationEnum.Indirect, translator);
        }

        private void PerformTranslationTest(string testName, string folderName, TranslationEnum translationTypeEnum, AbnfToAntlrTranslator translator)
        {
            var pathPrefix = Path.Combine(@"..\..\FileDrivenTests", folderName);

            var inputFileName = testName + ".input.txt";
            var outputFileName = testName + ".output." + translationTypeEnum.ToString().ToLowerInvariant() + ".txt";

            var inputPath = Path.Combine(pathPrefix, inputFileName);
            var expectedOutputPath = Path.Combine(pathPrefix, outputFileName);

            string inputText = null;

            if (File.Exists(inputPath))
            {
                // do nothing
            }
            else
            {
                File.WriteAllText(inputPath, "");
            }

            inputText = File.ReadAllText(inputPath);

            if (string.IsNullOrWhiteSpace(inputText))
            {
                TestContext.WriteLine(inputPath);
                throw new InvalidOperationException("File cannot be empty (\"" + inputPath + "\").");
            }

            var performDirectTranslation = (translationTypeEnum == TranslationEnum.Direct);

            var actualOutput = translator.Translate(inputText, performDirectTranslation);

            if (File.Exists(expectedOutputPath))
            {
                // do nothing
            }
            else
            {
                File.WriteAllText(expectedOutputPath, actualOutput);
            }

            var expectedOutput = File.ReadAllText(expectedOutputPath);

            if (actualOutput.Equals(expectedOutput, StringComparison.Ordinal))
            {
                // do nothing
            }
            else
            {
                TestContext.WriteLine(outputFileName);
                Assert.AreEqual(expectedOutput, actualOutput);
            }
        }

        protected void PerformConsoleTest(string testName, string folderName, ExpectedReturnValueEnum expectedReturnValue = ExpectedReturnValueEnum.Success)
        {
            var pathPrefix = Path.Combine(@"..\..\FileDrivenTests", folderName);

            var argsFileName = testName + ".args.txt";
            var inputFileName = testName + ".input.txt";
            var outputFileName = testName + ".output.txt";
            var errorFileName = testName + ".output.error.txt";

            var argsPath = Path.Combine(pathPrefix, argsFileName);
            var inputPath = Path.Combine(pathPrefix, inputFileName);
            var expectedOutputPath = Path.Combine(pathPrefix, outputFileName);
            var expectedErrorPath = Path.Combine(pathPrefix, errorFileName);

            if (File.Exists(argsPath))
            {
                // do nothing
            }
            else
            {
                File.WriteAllText(argsPath, "");
            }

            var args = File.ReadAllLines(argsPath);

            if (File.Exists(inputPath))
            {
                // do nothing
            }
            else
            {
                File.WriteAllText(inputPath, "");
            }

            var inputText = File.ReadAllText(inputPath);

            if (string.IsNullOrWhiteSpace(inputText))
            {
                TestContext.WriteLine(inputPath);
                throw new InvalidOperationException("Input file cannot be empty (\"" + inputPath + "\").");
            }

            var stdoutBuilder = new StringBuilder();
            var stderrBuilder = new StringBuilder();

            using (var stdin = new StringReader(inputText))
            using (var stdout = new StringWriter(stdoutBuilder))
            using (var stderr = new StringWriter(stderrBuilder))
            {
                var program = new Program();

                var returnValue = program.ConsoleMain(args, stdin, stdout, stderr);

                Assert.AreEqual((int)expectedReturnValue, returnValue);
            }

            var actualOutput = stdoutBuilder.ToString();
            var actualError = stderrBuilder.ToString();

            if (File.Exists(expectedOutputPath))
            {
                // do nothing
            }
            else
            {
                File.WriteAllText(expectedOutputPath, actualOutput);
            }

            if (File.Exists(expectedErrorPath))
            {
                // do nothing
            }
            else
            {
                File.WriteAllText(expectedErrorPath, actualError);
            }

            var expectedOutput = File.ReadAllText(expectedOutputPath);

            if (actualOutput.Equals(expectedOutput, StringComparison.Ordinal))
            {
                // do nothing
            }
            else
            {
                TestContext.WriteLine(outputFileName);
                Assert.AreEqual(expectedOutput, actualOutput);
            }

            var expectedError = File.ReadAllText(expectedErrorPath);

            if (actualError.Equals(expectedError, StringComparison.Ordinal))
            {
                // do nothing
            }
            else
            {
                TestContext.WriteLine(errorFileName);
                Assert.AreEqual(expectedError, actualError);
            }
        }

    }
}
