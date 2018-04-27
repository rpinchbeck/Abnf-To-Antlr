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
    public class CharValueTests  : TranslatorTestBase
    {
        [TestMethod]
        public void CharValueWithZeroCharacters_IsEmpty_ForIndirectTranslation()
        {
            IndirectTranslationTest(
                (inputBuilder) =>
                {
                    inputBuilder.AppendLine("someCharValue = \"\"");
                },
                (expectedBuilder) =>
                {
                    expectedBuilder.AppendLine("somecharvalue : ;");
                }
            );
        }

        [TestMethod]
        public void CharValueWithOneCharacter_IsCaseInsensitive_ForIndirectTranslation()
        {
            IndirectTranslationTest(
                (inputBuilder) =>
                {
                    inputBuilder.AppendLine("someCharValue = \"A\"");
                },
                (expectedBuilder) =>
                {
                    expectedBuilder.AppendLine("somecharvalue : (CAP_A | A);");
                    expectedBuilder.AppendLine("");
                    expectedBuilder.AppendLine("//////////////////////////////////////////////////////////////////////////");
                    expectedBuilder.AppendLine("// Lexer rules generated for each distinct character in original grammar");
                    expectedBuilder.AppendLine("// per http://www.unicode.org/charts/PDF/U0000.pdf");
                    expectedBuilder.AppendLine("//////////////////////////////////////////////////////////////////////////");
                    expectedBuilder.AppendLine("");
                    expectedBuilder.AppendLine("CAP_A : 'A';");
                    expectedBuilder.AppendLine("A : 'a';");
                }
            );
        }

        [TestMethod]
        public void CharValueWithMultipleCharacters_IsCaseInsensitive_ForIndirectTranslation()
        {
            IndirectTranslationTest(
                (inputBuilder) =>
                {
                    inputBuilder.AppendLine("someCharValue = \"AB\"");
                },
                (expectedBuilder) =>
                {
                    expectedBuilder.AppendLine("somecharvalue : ((CAP_A | A) (CAP_B | B));");
                    expectedBuilder.AppendLine("");
                    expectedBuilder.AppendLine("//////////////////////////////////////////////////////////////////////////");
                    expectedBuilder.AppendLine("// Lexer rules generated for each distinct character in original grammar");
                    expectedBuilder.AppendLine("// per http://www.unicode.org/charts/PDF/U0000.pdf");
                    expectedBuilder.AppendLine("//////////////////////////////////////////////////////////////////////////");
                    expectedBuilder.AppendLine("");
                    expectedBuilder.AppendLine("CAP_A : 'A';");
                    expectedBuilder.AppendLine("CAP_B : 'B';");
                    expectedBuilder.AppendLine("A : 'a';");
                    expectedBuilder.AppendLine("B : 'b';");
                }
            );
        }


        [TestMethod]
        public void CharValueWithSingleNumber_HasNoAlternatives_ForIndirectTranslation()
        {
            IndirectTranslationTest(
                (inputBuilder) =>
                {
                    inputBuilder.AppendLine("someCharValue = \"1\"");
                },
                (expectedBuilder) =>
                {
                    expectedBuilder.AppendLine("somecharvalue : ONE;");
                    expectedBuilder.AppendLine("");
                    expectedBuilder.AppendLine("//////////////////////////////////////////////////////////////////////////");
                    expectedBuilder.AppendLine("// Lexer rules generated for each distinct character in original grammar");
                    expectedBuilder.AppendLine("// per http://www.unicode.org/charts/PDF/U0000.pdf");
                    expectedBuilder.AppendLine("//////////////////////////////////////////////////////////////////////////");
                    expectedBuilder.AppendLine("");
                    expectedBuilder.AppendLine("ONE : '1';");
                }
            );
        }

        [TestMethod]
        public void CharValueWithMultipleNumbers_HasNoAlternatives_ForIndirectTranslation()
        {
            IndirectTranslationTest(
                (inputBuilder) =>
                {
                    inputBuilder.AppendLine("someCharValue = \"12\"");
                },
                (expectedBuilder) =>
                {
                    expectedBuilder.AppendLine("somecharvalue : (ONE TWO);");
                    expectedBuilder.AppendLine("");
                    expectedBuilder.AppendLine("//////////////////////////////////////////////////////////////////////////");
                    expectedBuilder.AppendLine("// Lexer rules generated for each distinct character in original grammar");
                    expectedBuilder.AppendLine("// per http://www.unicode.org/charts/PDF/U0000.pdf");
                    expectedBuilder.AppendLine("//////////////////////////////////////////////////////////////////////////");
                    expectedBuilder.AppendLine("");
                    expectedBuilder.AppendLine("ONE : '1';");
                    expectedBuilder.AppendLine("TWO : '2';");
                }
            );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Direct translation tests
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [TestMethod]
        public void CharValueWithZeroCharacters_IsEmpty_ForDirectTranslation()
        {
            DirectTranslationTest(
                (inputBuilder) =>
                {
                    inputBuilder.AppendLine("someCharValue = \"\"");
                },
                (expectedBuilder) =>
                {
                    expectedBuilder.AppendLine("SOMECHARVALUE : ;");
                }
            );
        }

        [TestMethod]
        public void CharValueWithOneCharacter_IsCaseInsensitive_ForDirectTranslation()
        {
            DirectTranslationTest(
                (inputBuilder) =>
                {
                    inputBuilder.AppendLine("someCharValue = \"A\"");
                },
                (expectedBuilder) =>
                {
                    expectedBuilder.AppendLine("SOMECHARVALUE : ('A' | 'a');");
                }
            );
        }

        [TestMethod]
        public void CharValueWithMultipleCharacters_IsCaseInsensitive_ForDirectTranslation()
        {
            DirectTranslationTest(
                (inputBuilder) =>
                {
                    inputBuilder.AppendLine("someCharValue = \"AB\"");
                },
                (expectedBuilder) =>
                {
                    expectedBuilder.AppendLine("SOMECHARVALUE : (('A' | 'a') ('B' | 'b'));");
                }
            );
        }


        [TestMethod]
        public void CharValueWithSingleNumber_HasNoAlternatives_ForDirectTranslation()
        {
            DirectTranslationTest(
                (inputBuilder) =>
                {
                    inputBuilder.AppendLine("someCharValue = \"1\"");
                },
                (expectedBuilder) =>
                {
                    expectedBuilder.AppendLine("SOMECHARVALUE : '1';");
                }
            );
        }

        [TestMethod]
        public void CharValueWithMultipleNumbers_HasNoAlternatives_ForDirectTranslation()
        {
            DirectTranslationTest(
                (inputBuilder) =>
                {
                    inputBuilder.AppendLine("someCharValue = \"12\"");
                },
                (expectedBuilder) =>
                {
                    expectedBuilder.AppendLine("SOMECHARVALUE : ('1' '2');");
                }
            );
        }

        [TestMethod]
        public void CharValueWithRule_IsParserRule_ForDirectTranslation()
        {
            DirectTranslationTest(
                (inputBuilder) =>
                {
                    inputBuilder.AppendLine("someCharValue = \"12\" / someRule");
                },
                (expectedBuilder) =>
                {
                    expectedBuilder.AppendLine("somecharvalue : ('1' '2') | somerule;");
                }
            );
        }
    }
}
