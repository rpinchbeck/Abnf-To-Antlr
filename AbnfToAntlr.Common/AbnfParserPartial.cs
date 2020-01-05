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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;

public partial class AbnfAstParser : Antlr.Runtime.Parser
{
    public List<RecognitionException> RecognitionExceptions = new List<RecognitionException>();

    public override void DisplayRecognitionError(string[] tokenNames, RecognitionException e)
    {
        RecognitionExceptions.Add(e);

        base.DisplayRecognitionError(tokenNames, e);
    }
}


