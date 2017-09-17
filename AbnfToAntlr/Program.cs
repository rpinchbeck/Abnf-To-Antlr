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
using System.Linq;
using System.Windows.Forms;

using AbnfToAntlr.Common;

namespace AbnfToAntlr
{
    static class Program
    {
        static void ShowSyntax()
        {
            Console.Error.WriteLine("Usage: AbnfToAntlr [--direct] [--stdin | FILE]");
            Console.Error.WriteLine("Translate FILE to ANTLR format and write the results to standard output.");
            Console.Error.WriteLine("If --stdin is specified instead of FILE, then standard input is used.");
            Console.Error.WriteLine("If --direct is specified, then a direct translation is performed.");
            Console.Error.WriteLine("Example: AbnfToAntlr \"AbnfGrammar.txt\" >AntlrGrammar.g");
        }

        [STAThread]
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
                return 0;
            }
            else
            {
                return ConsoleMain(args);
            }
        }

        static int ConsoleMain(string[] args)
        {
            bool shouldShowSyntax = false;
            bool shouldPerformDirectTranslation = false;
            int fileIndex = 0;

            if (args.Length == 0)
            {
                shouldShowSyntax = true;
            }
            else
            {
                switch (args[0])
                {
                    case "-h":
                    case "/h":
                    case "-?":
                    case "/?":
                    case "-help":
                    case "/help":
                    case "--help":
                        shouldShowSyntax = true;
                        break;

                    case "--direct":
                        shouldPerformDirectTranslation = true;
                        fileIndex = 1;
                        break;
                }
            }

            if (shouldShowSyntax || fileIndex >= args.Length || args.Length > fileIndex + 1)
            {
                ShowSyntax();
                return 1;
            }


            string path = null;
            System.IO.TextReader reader;
            string input;
            string output;

            try
            {
                // open input stream
                if (args[fileIndex] == "--stdin")
                {
                    path = "stdin";
                    reader = System.Console.In;
                }
                else
                {
                    path = args[0];
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

                System.Console.Write(output);

#if DEBUG
                // when debugging, output the resulting string builder to a file
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.IO.File.WriteAllText(@"..\..\_AbnfToAntlr_Debug_Output.txt", output);
                }
#endif
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(string.Format("An error occurred while processing '{0}':", path));
                Console.Error.WriteLine(ex.Message);
                return 2;
            }

            return 0;
        }
    }
}
