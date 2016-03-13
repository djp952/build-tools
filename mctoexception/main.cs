//-----------------------------------------------------------------------------
// Copyright (c) 2016 Michael G. Brehm
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//-----------------------------------------------------------------------------

//
// Code Usage:
//
// In the .MC file, the only customization is that you put a special
// ;//ExceptionName= tag in a message declaration to enable exception class
// generation and specify the name of the class.  An optional comma-delimited
// list of argument names can follow the class name:
//
// ;//ExceptionName=MyException,argument1,argument2,argumentN
//

using System;
using System.Collections.Generic;
using System.IO;

namespace zuki.build.tools
{
    static class main
	{
		/// <summary>
		/// Shows command line usage information
		/// </summary>
		static void ShowUsage()
		{
			Console.WriteLine();
			Console.WriteLine(AppDomain.CurrentDomain.FriendlyName.ToUpper() + " inputfile.mc outputfile.h [-unicode] [-include:\"include.h\"[,\"include.h\"]]");
			Console.WriteLine();
			Console.WriteLine("  -unicode      - Generate for a project that will define _UNICODE");
			Console.WriteLine("  -include:     - Comma-delimited list of file(s) to add as #include directives");
			Console.WriteLine("  inputfile.mc  - Input message compiler .MC text file");
			Console.WriteLine("  outputfile.h  - Output C++ header file");
			Console.WriteLine();
		}

		/// <summary>
		/// Main application entry point
		/// </summary>
		/// <param name="cmdlineargs">Array of command line arguments</param>
		[STAThread]
		static void Main(string[] cmdlineargs)
		{
			CommandLine commandline = new CommandLine(cmdlineargs);

			try
			{
				// Insufficient number of arguments
				if (commandline.Arguments.Count < 2) { ShowUsage(); return; }

				// infile --> argv[0]
				string infile = commandline.Arguments[0];
				if(!File.Exists(infile)) throw new FileNotFoundException("Specified input file does not exist", infile);

				// outfile --> argv[1]
				string outfile = commandline.Arguments[1];

				// Get the output directory name and ensure that it exists before continuing
				string outdir = Path.GetDirectoryName(outfile);
				if (!Directory.Exists(outdir)) Directory.CreateDirectory(outdir);

				// -unicode - set _UNICODE output mode
				bool unicode = commandline.Switches.ContainsKey("unicode");

				// -include - add an #include directive
				List<string> includes = new List<string>();
				if (commandline.Switches.ContainsKey("include"))
				{
					string includelist = commandline.Switches["include"];
					if (!String.IsNullOrEmpty(includelist)) includes.AddRange(includelist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
				}

				// banner
				Console.WriteLine();
				Console.WriteLine(Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName).ToUpper());
				Console.WriteLine();
				Console.WriteLine("Input File  : " + infile);
				Console.WriteLine("Output File : " + outfile);
				Console.WriteLine();

				// Create a MessageExceptions runtime text template for the output
				Messages messages = Messages.Load(infile, unicode);
				MessageExceptions output = new MessageExceptions(Path.GetFileNameWithoutExtension(outfile), messages, includes, unicode);

				// Transform the T4 text template into the specified output file
				File.WriteAllText(outfile, output.TransformText());

				Console.WriteLine("Successfully generated " + messages.Count.ToString() + " custom exceptions");
				Console.WriteLine();
			}

			catch (Exception ex)
			{
				Console.WriteLine();
				Console.WriteLine("ERROR: " + ex.Message);
				Console.WriteLine();
			}
		}
	}
}
