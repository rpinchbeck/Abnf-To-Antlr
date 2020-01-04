/*

    Copyright 2013-2020 Robert Pinchbeck
  
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AbnfToAntlr.Common;
using Antlr.Runtime;

namespace AbnfToAntlr
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnTranslate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                btnTranslate.Enabled = false;
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
                this.txtOutput.ForeColor = this.ForeColor;
                this.txtOutput.BackColor = this.BackColor; // readonly textbox forecolor only changes when backcolor is set
            }
            catch (TranslationException ex)
            {
                this.txtOutput.Text = AntlrHelper.GetErrorMessages(ex.ParserRecognitionExceptions) + AntlrHelper.GetErrorMessages(ex.LexerRecognitionExceptions);
                this.txtOutput.ForeColor = Color.DarkRed;
                this.txtOutput.BackColor = this.BackColor; // readonly textbox forecolor only changes when backcolor is set
            }
            catch (RecognitionException ex)
            {
                this.txtOutput.Text = AntlrHelper.GetErrorMessage(ex);
                this.txtOutput.ForeColor = Color.DarkRed;
                this.txtOutput.BackColor = this.BackColor; // readonly textbox forecolor only changes when backcolor is set
            }
            catch (Exception ex)
            {
                this.txtOutput.Text = ex.Message;
                this.txtOutput.ForeColor = Color.DarkRed;
                this.txtOutput.BackColor = this.BackColor; // readonly textbox forecolor only changes when backcolor is set
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnTranslate.Enabled = true;
                lblOutput.Visible = true;
                txtOutput.Visible = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            cboStandardGrammars.Items.Add(new ListItem("Custom", "Custom"));
            cboStandardGrammars.Items.Add(new ListItem("RFC 3986 (Uniform Resource Identifier)", "rfc-3986"));
            cboStandardGrammars.Items.Add(new ListItem("RFC 5322 (Internet Message Format)", "rfc-5322"));
            cboStandardGrammars.Items.Add(new ListItem("RFC 5234 (Augmented Backus-Naur Form)", "rfc-5234"));

            cboStandardGrammars.DisplayMember = "Text";
            cboStandardGrammars.ValueMember = "Value";

            cboStandardGrammars.SelectedIndex = 0;
        }

        private void cboStandardGrammars_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedFile = "Empty.txt";

            var selectedListItem = (ListItem)(sender as ComboBox).SelectedItem;
            switch (selectedListItem.Value)
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

            var path = System.IO.Path.Combine("App_Data", selectedFile);
            txtInput.Text = System.IO.File.ReadAllText(path);
            txtOutput.Text = "";
            lblOutput.Visible = false;
            txtOutput.Visible = false;
        }

        private void txtOutput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.A)
            {
                txtOutput.SelectAll();
            }
        }
    }
}
