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

using Antlr.Runtime;
using Antlr.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AbnfToAntlr.Common
{
    public static class AntlrHelper
    {
        public static ITree GetAndValidateChild(this ITree node, int index)
        {
            var result = node.GetChild(index);

            if (result is CommonErrorNode)
            {
                var commonErrorNode = (CommonErrorNode)result;

                throw commonErrorNode.trappedException;
            }

            return result;
        }

        public static string GetErrorMessages(IEnumerable<RecognitionException> recognitionExceptions)
        {
            var builder = new StringBuilder();
            foreach (var recognitionException in recognitionExceptions)
            {
                builder.AppendLine(GetErrorMessage(recognitionException));
                builder.AppendLine();
            }

            return builder.ToString();
        }

        public static string GetErrorMessage(RecognitionException recognitionException)
        {
            var message = recognitionException.Message;

            if (message.EndsWith("."))
            {
                // strip ending period from message
                message = message.Substring(0, message.Length - 1);
            }

            if (message.Length > 0)
            {
                message = message + " ";
            }

            var result = message + (recognitionException.ApproximateLineInfo ? "near" : "at") + " line " + recognitionException.Line + " column " + (recognitionException.CharPositionInLine + 1) + " \"" + recognitionException.ToString() + "\"";

            return result;
        }

        public static string CharEscape(char character)
        {
            string result;

            if (character == '\'')
            {
                result = @"\'";
            }
            else if (character == '\\')
            {
                result = @"\\";
            }
            else if (character < 32)
            {
                result = @"\u" + ((int)character).ToString("X4");
            }
            else
            {
                result = character.ToString();
            }

            return result;
        }
    }
}
