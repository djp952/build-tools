//-----------------------------------------------------------------------------
// Copyright (c) 2019-2021 Michael G. Brehm
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace zuki.build.tools
{
    static class main
	{
		#region Win32 API
		private class Win32
		{
			[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
			public static extern uint GetPrivateProfileString(
			   string lpAppName,
			   string lpKeyName,
			   string lpDefault,
			   StringBuilder lpReturnedString,
			   uint nSize,
			   string lpFileName);
		};
		#endregion

		/// <summary>
		/// Shows application usage information
		/// </summary>
		static void ShowUsage()
		{
			Console.WriteLine();
			Console.WriteLine(AppDomain.CurrentDomain.FriendlyName.ToUpper() + " outdir [-ini:inifile] [-clean] [-rebuild] [-format:fmt[,fmt]] " +
				"[-company:companyname] [-copyright:copyrightstring] [-product:productname] [-version:major[.minor[.build[.revision]]]]");
			Console.WriteLine();
			Console.WriteLine("Automates creation of version resource file(s) for Visual Studio projects.");
			Console.WriteLine();
			Console.WriteLine("  outdir     : Output directory; will be created if it does not exist");
			Console.WriteLine("  -ini       : Retrieve version properties from specified version.ini file");
			Console.WriteLine("  -clean     : Clean the output directory, do not generate version file(s)");
			Console.WriteLine("  -rebuild   : Rebuild existing file(s) in output directory");
			Console.WriteLine("  -filename  : Base filename to use for the generated file(s) (default: version)");
			Console.WriteLine("  -format    : Comma-delimited list of format specifiers (default: all)");
			Console.WriteLine("  -company   : Company name property for generated version file(s)");
			Console.WriteLine("  -copyright : Copyright string for generated file(s) (default: auto-generate)");
			Console.WriteLine("  -product   : Product name property for generated version file(s)");
			Console.WriteLine("  -version   : Version property for generated version file(s).  See below.");
			Console.WriteLine();
			Console.WriteLine("-version format:");
			Console.WriteLine();
			Console.WriteLine("  Version string must be of the format major[.minor[.build[.revision]]]");
			Console.WriteLine();
			Console.WriteLine("  major      : Major version number (default: 0)");
			Console.WriteLine("  minor      : Minor version number (default: 0)");
			Console.WriteLine("  build      : Build number (default: 0)");
			Console.WriteLine("  revision   : Revision number (default is days since Jan. 1, 2000)");
			Console.WriteLine();
			Console.WriteLine("-format specifiers:");
			Console.WriteLine();
			Console.WriteLine("  all - Generate all supported formats");
			Console.WriteLine("  cpp - Managed C++/CLI project (version.cpp)");
			Console.WriteLine("  cs  - Managed C# project (version.cs)");
			Console.WriteLine("  h   - Unmanaged C/C++ project header file (version.h)");
			Console.WriteLine("  ini - Configuration file for this utility (version.ini)");
			Console.WriteLine("  rc  - Unmanaged C/C++ project resource script (version.rc)");
			Console.WriteLine("  txt - Generic text file (version.txt)");
			Console.WriteLine("  vb  - Managed VB.NET project (version.vb)");
			Console.WriteLine("  wxi - WiX installer project (version.wxi)");
		}

		/// <summary>
		/// Application entry point
		/// </summary>
		/// <param name="arguments">Command-line arguments</param>
		static int Main(string[] arguments)
		{
			string basename = "version";			// Base filename string

			try
			{
				// Parse the command line arguments and switches
				CommandLine commandline = new CommandLine(arguments);

				// If help was requested, just display the usage and exit
				if (commandline.Switches.ContainsKey("?")) { ShowUsage(); return 0; }

				// There has to be at least one command line argument to indicate the output directory
				if (commandline.Arguments.Count < 1) throw new ArgumentException("An output directory must be specified");
				string outdir = commandline.Arguments[0];

				// Attempt to create the output directory if it does not exist
				if (!Directory.Exists(outdir)) Directory.CreateDirectory(outdir);

				// Create the default template fields, and override as arguments are processed
				TemplateFields fields = new TemplateFields();

				// Override the base file name if specified on the command line
				if (commandline.Switches.ContainsKey("filename"))
					basename = commandline.Switches["filename"];

				// -clean: just delete all version.* files in the output directory
				bool clean = commandline.Switches.ContainsKey("clean");

				// -rebuild: force overwrite of existing files, default is to leave them in place
				bool rebuild = commandline.Switches.ContainsKey("rebuild");

				// If clean or rebuild has been specified, delete all matching output files
				if (clean || rebuild)
				{
					foreach (string filename in Directory.GetFiles(outdir, basename + ".*")) File.Delete(filename);
				}

				// If clean was specified, there is no more action required
				if (clean) return 0;

				// First process the ini file, if one was specified
				if (commandline.Switches.ContainsKey("ini"))
				{
					string inifile = commandline.Switches["ini"];
					if (!File.Exists(inifile)) throw new FileNotFoundException("Specified version.ini file does not exist", inifile);

					StringBuilder inistring = new StringBuilder(512);

					// Company (free-form)
					Win32.GetPrivateProfileString("Version", "Company", "", inistring, 512, inifile);
					fields.CompanyName = inistring.ToString();
					inistring.Clear();

					// Copyright (free-form)
					Win32.GetPrivateProfileString("Version", "Copyright", "", inistring, 512, inifile);
					fields.LegalCopyright = inistring.ToString();
					inistring.Clear();

					// Product (free-form)
					Win32.GetPrivateProfileString("Version", "Product", "", inistring, 512, inifile);
					fields.ProductName = inistring.ToString();
					inistring.Clear();

					// Version (MAJ[.MIN[.BUILD[.REVISION]]])
					Win32.GetPrivateProfileString("Version", "Version", "", inistring, 512, inifile);
					fields.VersionString = inistring.ToString();
					if (!TryGenerateVersion(inistring.ToString(), out fields.Version))
						throw new ArgumentException("Invalid version string format specified in .ini file");
					inistring.Clear();
				}

				// Process the individual version component switches, they can override the .ini file
				if (commandline.Switches.ContainsKey("company")) fields.CompanyName = commandline.Switches["company"];
				if (commandline.Switches.ContainsKey("copyright")) fields.LegalCopyright = commandline.Switches["copyright"];
				if (commandline.Switches.ContainsKey("product")) fields.ProductName = commandline.Switches["product"];
				if (commandline.Switches.ContainsKey("version"))
				{
					fields.VersionString = commandline.Switches["version"];
					if (!TryGenerateVersion(commandline.Switches["version"], out fields.Version))
						throw new ArgumentException("Invalid version string format specified on command line");
				}

				// Convert the version into a pseudo-uuid based on the final full version number
				fields.VersionGuid = GuidFromVersion(fields.ProductName, fields.Version);

				// Generate the list of formats to be generated (empty = generate all)
				List<string> formats = new List<string>();
				if (commandline.Switches.ContainsKey("format"))
				{
					// Convert all of the provided format codes into lowercase
					string[] parts = commandline.Switches["format"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
					foreach (string part in parts) formats.Add(part.ToLower());
				}

				// If there were no formats specified or 'all' was specified, generate everything
				bool all = ((formats.Count == 0) || (formats.Contains("all")));

				// Generate the requested format(s) in the output directory
				if (all || formats.Contains("cpp")) GenerateOutputFile(outdir, basename + ".cpp", new TemplateCPP(fields).TransformText());
				if (all || formats.Contains("cs")) GenerateOutputFile(outdir, basename + ".cs", new TemplateCS(fields).TransformText());
				if (all || formats.Contains("h")) GenerateOutputFile(outdir, basename + ".h", new TemplateH(fields).TransformText());
				if (all || formats.Contains("ini")) GenerateOutputFile(outdir, basename + ".ini", new TemplateINI(fields).TransformText());
				if (all || formats.Contains("rc")) GenerateOutputFile(outdir, basename + ".rc", new TemplateRC(fields).TransformText());
				if (all || formats.Contains("txt")) GenerateOutputFile(outdir, basename + ".txt", new TemplateTXT(fields).TransformText());
				if (all || formats.Contains("vb")) GenerateOutputFile(outdir, basename + ".vb", new TemplateVB(fields).TransformText());
				if (all || formats.Contains("wxi")) GenerateOutputFile(outdir, basename + ".wxi", new TemplateWXI(fields).TransformText());
			}

			catch (Exception ex)
			{
				Console.WriteLine();
				Console.WriteLine("ERROR: " + ex.Message);
				Console.WriteLine();
			}

			return 0;
		}

		/// <summary>
		/// Generates the output file, if the file exists it will NOT be overwritten
		/// </summary>
		/// <param name="outdir">Output directory</param>
		/// <param name="filename">Output file name</param>
		/// <param name="content">Output file content</param>
		static void GenerateOutputFile(string outdir, string filename, string content)
		{
			string outname = Path.Combine(outdir, filename);

			// If the file exists already, do not overwrite it.  The caller can specify -rebuild to
			// the application to force the files to be deleted first (this is done so visual studio
			// won't always trigger a compile/link when this is used in a custom build step)
			if (File.Exists(outname)) return;
			else File.WriteAllText(outname, content);
		}

		/// <summary>
		/// Converts a Version object into a pseudo-uuid that will be the same provided that
		/// the project string and all four fields of the version number are the same
		/// </summary>
		/// <param name="product">Product name</param>
		/// <param name="version">Full version number</param>
		/// <returns></returns>
		static Guid GuidFromVersion(string product, Version version)
		{
			if(product == null) throw new ArgumentNullException("product");
			if(version == null) throw new ArgumentNullException("version");

			// Convert the product into lowercase before hashing it
			product = product.ToLowerInvariant();

			// Hash four 32-bit values that can be used to generate the Guid
			int producthash = product.GetHashCode();
			int majorhash = version.Major.ToString().GetHashCode();
			int minorhash = version.Minor.ToString().GetHashCode();
			int buildhash = version.Build.ToString().GetHashCode();

			// Convert the four hashed values into a 128-bit Guid object
			byte[] bytes = new byte[16];
			BitConverter.GetBytes(producthash).CopyTo(bytes, 0);
			BitConverter.GetBytes(majorhash ^ producthash).CopyTo(bytes, 4);
			BitConverter.GetBytes(minorhash ^ producthash).CopyTo(bytes, 8);
			BitConverter.GetBytes(buildhash ^ producthash).CopyTo(bytes, 12);

			return new Guid(bytes);
		}

		/// <summary>
		/// Attempts to generate a Version instance from a partial or complete version string
		/// </summary>
		/// <param name="verstring">Version string to be parsed/completed</param>
		/// <param name="version">Receives the generated version instance on success</param>
		static bool TryGenerateVersion(string verstring, out Version version)
		{
			int major = 0;
			int minor = 0;
			int build = 0;

			// The default revision is the number of days since January 1, 2000
			int revision = DateTime.Today.Subtract(DateTime.Parse("1/1/2000")).Days;

			try
			{
				if (!String.IsNullOrEmpty(verstring))
				{
					// Split the input string into a maximum of four components and
					// convert them into integer values (will throw if not numeric)
					string[] parts = verstring.Split(new char[] { '.' }, 4);
					if (parts.Length >= 1) major = Int32.Parse(parts[0]);
					if (parts.Length >= 2) minor = Int32.Parse(parts[1]);
					if (parts.Length >= 3) build = Int32.Parse(parts[2]);
					if (parts.Length >= 4) revision = Int32.Parse(parts[3]);
				}

				// Construct the [out] version instance for the caller
				version = new Version(major, minor, build, revision);
				return true;
			}

			catch (Exception) {

				// If any of the string components failed to parse, return false
				version = new Version(0, 0, 0, 0);
				return false;
			}
		}
	}
}
