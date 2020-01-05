/*

    Copyright 2012-2018 Robert Pinchbeck
  
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

/*
    --------------------------------------------------------------------------------
    AbnfToAntlr - Translate any ABNF grammar to an ANTLR grammar

    This solution will convert any ABNF grammar into an ANTLR grammar.

    The resulting ANTLR grammar should be syntactically correct; however, 
    some ABNF grammars are inherently ambiguous and ANTLR will complain about them 
    until the ambiguity is resolved by the user.
  
    The solution can be built with Visual Studio or a compatible product like 
    SharpDevelop.  Open the solution, build it, and use the resulting executable to 
    translate any ABNF grammar to an ANTLR grammar.

    The translator makes the following corrections automatically:

        1. Dashes in ABNF rule names are replaced with underscores.
        2. ABNF rule names which are also ANTLR keywords are given a numeric suffix (e.g., fragment_1).
        3. Duplicate rule names are given a numeric suffix to make them unique (e.g., rule_1, rule_2)
        4. All rule names are converted to lowercase (parser rules).
        5. By default, all character literals are converted into lexer rules (resulting in a scannerless parser).

    Happy translating!

    -=Robert Pinchbeck

    http://www.robertpinchbeck.com


    The following articles were instrumental in developing this solution...

    How To Use Antlr With Visual Studio 2010...
    http://techblog.adrianlowdon.co.uk/tag/antlr/

    Translators Should Use Tree Grammars...
    http://antlr.org/article/1100569809276/use.tree.grammars.tml

    Manual Tree Walking Is Better Than Tree Grammars...
    http://www.antlr.org/article/1170602723163/treewalkers.html

    Why I Don't Use StringTemplate For Language Translation...
    http://antlr.org/article/1136917339929/stringTemplate.html

    Preserving whitespace during translation...
    http://www.antlr.org/article/whitespace/index.html

    Augmented BNF for Syntax Specifications (RFC 5234)
    http://tools.ietf.org/html/rfc5234

    Official character names from Unicode.org
    http://www.unicode.org/charts/PDF/U0000.pdf

    --------------------------------------------------------------------------------
*/

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AbnfToAntlr.Common;

namespace AbnfToAntlr.Web
{
    public partial class DefaultPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 3000;

            if (IsPostBack)
            {
            }
            else
            {
                lstStandardGrammars.Items.Add(new ListItem("Custom", "Custom"));
                lstStandardGrammars.Items.Add(new ListItem("RFC 3986 (Uniform Resource Identifier)", "rfc-3986"));
                lstStandardGrammars.Items.Add(new ListItem("RFC 5322 (Internet Message Format)", "rfc-5322"));
                lstStandardGrammars.Items.Add(new ListItem("RFC 5234 (Augmented Backus-Naur Form)", "rfc-5234"));
                this.lblOutput.Visible = false;
                this.txtOutput.Visible = false;
                this.txtError.Visible = false;
            }
        }

        protected void lstStandardGrammars_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFile = "Empty.txt";

            switch ((sender as DropDownList).SelectedValue)
            {
                case "rfc-3986":
                    selectedFile = "ABNF Uniform Resource Identifier (RFC 3986).txt";
                    break;

                case "rfc-5322":
                    selectedFile = "ABNF Internet Message Format (RFC 5322).txt";
                    break;

                case "rfc-5234":
                    selectedFile = "ABNF Specification (RFC 5234 and RFC 7405 and Errata 5334).txt";
                    break;

            }

            txtInput.Text = System.IO.File.ReadAllText(Server.MapPath("~") + "/App_Data/" + selectedFile);
            txtOutput.Text = "";
            lblOutput.Visible = false;
            txtOutput.Visible = false;
        }

        protected void butTranslate_Click(object sender, EventArgs e)
        {
                try
                {
                    var translator = new AbnfToAntlrTranslator();
                    bool performDirectTranslation = false;

                    if (chkPerformDirectTranslation.Checked == true)
                    {
                        performDirectTranslation = true;
                    }

                    var input = txtInput.Text;
                    if (input.EndsWith("\r\n"))
                    {
                        // do nothing
                    }
                    else
                    {
                        input = input + "\r\n";
                    }

                    this.txtOutput.Text = translator.Translate(input, performDirectTranslation);

                    this.lblOutput.Visible = true;
                    this.txtOutput.Visible = true;
                    this.txtError.Visible = false;
                }
                catch (Exception ex)
                {
                    this.txtError.Text = ex.Message;
                    this.txtError.Visible = true;

                    this.lblOutput.Visible = false;
                    this.txtOutput.Visible = false;
                }
            }
    }
}