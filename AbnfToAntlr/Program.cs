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
using System.IO;
using System.Linq;
using System.Windows.Forms;

using AbnfToAntlr.Common;

namespace AbnfToAntlr
{
    public class Program
    {
        static void ShowSyntax(TextWriter stderr)
        {
            stderr.WriteLine("Usage: AbnfToAntlr [--direct] [--stdin | FILE]");
            stderr.WriteLine("Translate FILE to ANTLR format and write the results to standard output.");
            stderr.WriteLine("If --stdin is specified instead of FILE, then standard input is used.");
            stderr.WriteLine("If --direct is specified, then a direct translation is performed.");
            stderr.WriteLine("Example: AbnfToAntlr \"AbnfGrammar.txt\" >\"AntlrGrammar.g4\"");
        }

        [STAThread]
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static int Main(string[] args)
        {
            int result;

            var program = new Program();

            if (args.Length == 0)
            {
                result = program.WinFormsMain();
            }
            else
            {
                result = program.ConsoleMain(args, Console.In, Console.Out, Console.Error);
            }

            return result;
        }

        int WinFormsMain()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());

            return 0;
        }

        public int ConsoleMain(string[] args, TextReader stdin, TextWriter stdout, TextWriter stderr)
        {
            bool shouldShowSyntax = false;
            bool shouldPerformDirectTranslation = false;
            int fileArgIndex = 0;

            if (args.Length == 0)
            {
                shouldShowSyntax = true;
            }
            else
            {
                switch (args[0].ToLowerInvariant())
                {
                    case "-h":
                    case "/h":
                    case "--h":

                    case "-?":
                    case "/?":
                    case "--?":

                    case "-help":
                    case "/help":
                    case "--help":
                        shouldShowSyntax = true;
                        break;

                    case "--direct":
                        shouldPerformDirectTranslation = true;
                        fileArgIndex = 1;
                        break;
                }
            }

            if (shouldShowSyntax || fileArgIndex >= args.Length || args.Length > fileArgIndex + 1)
            {
                ShowSyntax(stderr);
                return 1;
            }

            string path = null;
            System.IO.TextReader reader;
            string input;
            string output;

            try
            {
                // open input stream
                if (args[fileArgIndex] == "--stdin")
                {
                    path = "stdin";
                    reader = stdin;
                }
                else
                {
                    path = args[fileArgIndex];
                    reader = new System.IO.StreamReader(path);
                }

                input = reader.ReadToEnd();
                if (input.EndsWith("\r\n"))
                {
                    // do nothing
                }
                else
                {
                    input = input + "\r\n";
                }

                var translator = new AbnfToAntlrTranslator();

                output = translator.Translate(input, shouldPerformDirectTranslation);

                stdout.Write(output);
            }
            catch (Exception ex)
            {
                stderr.WriteLine(string.Format("An error occurred while processing '{0}':", path));
                stderr.WriteLine();
                stderr.WriteLine(ex.Message);
                return 2;
            }

            return 0;
        }
    }
}
