//---------------------------------------------------------------------------
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
//---------------------------------------------------------------------------

//---------------------------------------------------------------------------
// Usage: 
//
// > normalizer {-w[indows]|-u[nix]} filespec [-s]
//
// Normalizes the line endings of text files
//
//  -w[indows]  - Apply Windows CRLF line endings
//  -u[nix]     - Apply Unix LF line endings
//  filespec    - File(s) to normalize, wildcards OK
//  -s          - Process subdirectories (uses same filespec)
//---------------------------------------------------------------------------

using System;
using System.IO;

namespace zuki.build.tools
{
	class main
	{
		/// <summary>
		/// Application entry point
		/// </summary>
		/// <param name="arguments">Command-line arguments</param>
		static int Main(string[] arguments)
		{
			string newline = "\r\n";                    // Default newline character(s)
			bool subdirs = false;                       // Flag to process subdirectories

			try
			{
				// Parse the command line arguments and switches
				CommandLine commandline = new CommandLine(arguments);

				// If insufficient arguments specified or help was requested, display the usage and exit
				if ((commandline.Arguments.Count == 0) || (commandline.Switches.ContainsKey("?")))
				{
					ShowUsage();
					return 0;
				}

				// Get the file search specification
				string filespec = commandline.Arguments[0].ToLower();

				// -w[indows] - Windows line endings
				if (commandline.Switches.ContainsKey("w") || commandline.Switches.ContainsKey("windows")) newline = "\r\n";

				// -u[nix] - Unix line endings
				if (commandline.Switches.ContainsKey("u") || commandline.Switches.ContainsKey("unix")) newline = "\n";

				// -s - Process subdirectories
				if (commandline.Switches.ContainsKey("s")) subdirs = true;

				// Normalize the specified files, optionally including the subdirectories
				string startdir = Path.Combine(Environment.CurrentDirectory, Path.GetDirectoryName(filespec));
				foreach (string filename in Directory.GetFiles(startdir, Path.GetFileName(filespec), (subdirs) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
				{
					string normalized = NormalizeFile(filename, newline);
					File.Copy(normalized, filename, true);
					TryDeleteFile(normalized);
					Console.WriteLine(filename);
				}

				return 0;
			}

			catch (Exception ex)
			{
				// Nothing fancy, just catch anything that escapes and spit it out
				Console.WriteLine();
				Console.WriteLine("ERROR: " + ex.Message);
				Console.WriteLine();

				return unchecked((int)0x80004005);      // <-- E_FAIL
			}
		}

		//-------------------------------------------------------------------
		// Private Member Functions
		//-------------------------------------------------------------------

		/// <summary>
		/// Normalizes the line endings of a text file
		/// </summary>
		/// <param name="filename">Input file name</param>
		/// <param name="newline">Newline string</param>
		private static string NormalizeFile(string filename, string newline)
		{
			if (!File.Exists(filename)) throw new FileNotFoundException("specified text file [" + filename + "] does not exist", filename);

			// Get the timestamps from the original file so they can be preserved
			DateTime mtime = File.GetLastWriteTime(filename);
			DateTime ctime = File.GetCreationTime(filename);

			// Use a system-generated temporary file as the target
			string outfile = Path.GetTempFileName();

			try
			{
				// Open up a reader against the source file ...
				using (StreamReader sr = new StreamReader(File.OpenRead(filename)))
				{
					// ... and a writer against the destination file
					using (StreamWriter sw = new StreamWriter(File.Create(outfile)))
					{
						sw.NewLine = newline;      // <-- Use specified line endings

						// Just read in every line from the source file and write it back
						// out to the destination file using the defined line ending
						string line = sr.ReadLine();
						while (line != null)
						{
							sw.WriteLine(line);
							line = sr.ReadLine();
						}

						// Flush the output stream to disk
						sw.Flush();
					}
				}

				// Alter the creation and modification times on the new file
				File.SetCreationTime(outfile, ctime);
				File.SetLastWriteTime(outfile, mtime);

				// Return the name of the normalized file back to the caller
				return outfile;
			}

			// Delete the temporary file on any exception during conversion
			catch { TryDeleteFile(outfile); throw; }
		}

		/// <summary>
		/// Shows application usage information
		/// </summary>
		private static void ShowUsage()
		{
			Console.WriteLine();
			Console.WriteLine("Normalizes the line endings of all files matching filespec");
			Console.WriteLine();
			Console.WriteLine("Usage:");
			Console.WriteLine();
			Console.WriteLine("normalizer {-w[indows]|-u[nix]} filespec [-s]");
			Console.WriteLine();
			Console.WriteLine(" -w[indows]  - Apply Windows CRLF line endings");
			Console.WriteLine(" -u[nix]     - Apply Unix LF line endings");
			Console.WriteLine(" filespec    - File(s) to normalize, wildcards OK");
			Console.WriteLine(" -s          - Process subdirectories (uses same filespec)");
			Console.WriteLine();
		}

		/// <summary>
		/// Attempts to delete a file; does not throw any exceptions
		/// </summary>
		/// <param name="path">Path to the file to be deleted</param>
		private static bool TryDeleteFile(string path)
		{
			try { File.Delete(path); return true; }
			catch { return false; }
		}
	}
}
