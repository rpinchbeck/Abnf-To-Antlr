/*

    Copyright 2013 Robert Pinchbeck
  
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
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AbnfToAntlr.Common;

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
                this.txtOutput.ForeColor = Color.Black;
            }
            catch (Exception ex)
            {
                this.txtOutput.Text = ex.Message;
                this.txtOutput.ForeColor = Color.Red;
            }
            finally
            {
                btnTranslate.Enabled = true;
            }
        }
    }
}
