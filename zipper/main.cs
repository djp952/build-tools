//-----------------------------------------------------------------------------
// Copyright (c) 2019 Michael G. Brehm
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

//---------------------------------------------------------------------------
// Usage: 
//
// > zipper create {zipfile} {manifest} [-md5] [-sha256]
//
// Creates a new ZIP archive based on an archive manifest
//
//	zipfile		- zip archive file to be created
//	manifest	- manifest file describing the archive contents
//	-md5		- generate .md5 hash file
//	-sha256		- generate .sha256 hash file
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

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

				// Get the application command
				string command = commandline.Arguments[0].ToLower();

				// > create {zipfile} {manifest} [-md5] [-sha256]
				if (command == "create")
				{
					if (commandline.Arguments.Count != 3) { ShowUsage(); return 1; }
					string zipfile = CreateZip(commandline.Arguments[1], commandline.Arguments[2]);

					// Optionally create the .md5 and/or .sha256 hash files
					if (commandline.Switches.ContainsKey("md5")) CreateMD5Hash(zipfile);
					if (commandline.Switches.ContainsKey("sha256")) CreateSHA256Hash(zipfile);
				}

				// unrecognized command
				else { ShowUsage(); }

				return 0;
			}

			catch (Exception ex)
			{
				// Nothing fancy, just catch anything that escapes and spit it out
				Console.WriteLine();
				Console.WriteLine("ERROR: " + ex.Message);
				Console.WriteLine();
		
				return unchecked((int)0x80004005);		// <-- E_FAIL
			}
		}

		//-------------------------------------------------------------------
		// Private Member Functions
		//-------------------------------------------------------------------

		/// <summary>
		/// Generates the optional .md5 output file
		/// </summary>
		/// <param name="zipfile">Path to the input file to be hashed</param>
		private static void CreateMD5Hash(string zipfile)
		{
			if (String.IsNullOrEmpty(zipfile)) throw new ArgumentNullException("zipfile");
			if (!File.Exists(zipfile)) throw new FileNotFoundException("zipfile", zipfile);

			using (FileStream fs = File.OpenRead(zipfile))
			{
				string hash = String.Format("{0}  {1}", BitConverter.ToString(MD5.Create().ComputeHash(fs)).Replace("-", "").ToLower(), Path.GetFileName(zipfile));
				File.WriteAllText(zipfile + ".md5", hash, Encoding.ASCII);
				Console.WriteLine(hash);
			}
		}

		/// <summary>
		/// Generates the optional .sha256 output file
		/// </summary>
		/// <param name="zipfile">Path to the input file to be hashed</param>
		private static void CreateSHA256Hash(string zipfile)
		{
			if (String.IsNullOrEmpty(zipfile)) throw new ArgumentNullException("zipfile");
			if (!File.Exists(zipfile)) throw new FileNotFoundException("zipfile", zipfile);

			using (FileStream fs = File.OpenRead(zipfile))
			{
				string hash = String.Format("{0}  {1}", BitConverter.ToString(SHA256.Create().ComputeHash(fs)).Replace("-", "").ToLower(), Path.GetFileName(zipfile));
				File.WriteAllText(zipfile + ".sha256", hash, Encoding.ASCII);
				Console.WriteLine(hash);
			}
		}

		/// <summary>
		/// Creates a new zip file based on the contents of an input manifest file
		/// </summary>
		/// <param name="zipfile">zip file to be created</param>
		/// <param name="manifestfile">zip manifest file name</param>
		private static string CreateZip(string zipfile, string manifestfile)
		{
			// Attempt to load the contents of the manifest file into a List<>
			List<ZipNode> nodes = LoadManifest(manifestfile);

			// Append the zip file name to the current directory in case it's relative
			zipfile = Path.Combine(Environment.CurrentDirectory, zipfile);

			// Ensure that the specified output directory exists
			if (!Directory.Exists(Path.GetDirectoryName(zipfile)) && !TryCreateDirectory(Path.GetDirectoryName(zipfile)))
				throw new DirectoryNotFoundException("specified output directory [" + Path.GetDirectoryName(zipfile) + "] does not exist");

			// Attempt to delete any existing file at the specified location; ZipFile.Open will fail with "exists" if this doesn't work
			if (File.Exists(zipfile)) TryDeleteFile(zipfile);

			try
			{
				// Spit out the name of the archive file being created
				Console.WriteLine("create: " + zipfile);

				// Create the output archive file stream
				using (ZipArchive archive = ZipFile.Open(zipfile, ZipArchiveMode.Create))
				{
					foreach (ZipNode node in nodes)
					{
						string sourcefile = node.Source;        // Initial source file path
						bool deletesource = false;              // Flag to delete source file after processing

						// If creating a file node with the NormalizeLineEndings flag set, generate a normalized temporary 
						// file that will take it's place in the archive
						if ((node.Normalize != ZipNormalizeType.None) && (!String.IsNullOrEmpty(node.Source)))
						{
							sourcefile = NormalizeFile(node.Source, node.Normalize);
							deletesource = true;
						}

						// CreateEntryFromFile() should use a forward slash rather than a backslash for compatibility
						string path = node.Path.Replace('\\', '/');

						// Write the file into the archive and delete any temporary source that was generated
						try { archive.CreateEntryFromFile(sourcefile, path, CompressionLevel.Optimal); }
						finally { if (deletesource) TryDeleteFile(sourcefile); }

						// Just spit out the resultant file name as an "add" operation for now
						Console.WriteLine("add: " + node.Path);
					}
				}
			}

			// Delete the created zip file on any exception during creation
			catch { TryDeleteFile(zipfile); throw; }

			Console.WriteLine();

			// Return the fully qualified path to the generated file
			return zipfile;
		}

		/// <summary>
		/// Loads all of the zip file information from the manifest file
		/// </summary>
		/// <param name="manifestfile">Path to the manifest.xml file</param>
		private static List<ZipNode> LoadManifest(string manifestfile)
		{
			// Ensure that the manifest file exists
			if (!File.Exists(manifestfile)) throw new FileNotFoundException("specified manifest file [" + manifestfile + "] does not exist", manifestfile);

			// Create a new empty list of nodes
			List<ZipNode> zipnodes = new List<ZipNode>();

			// Create a new XmlDocument instance for the specified input file path
			XmlDocument document = new XmlDocument();
			document.Load(manifestfile);

			// Process each child element under the document element
			foreach (XmlNode child in document.DocumentElement.ChildNodes)
			{
				if (child.NodeType != XmlNodeType.Element) continue;
				XmlElement element = (XmlElement)child;

				try
				{
					// Get the name of the child element (<file>)
					string elementname = child.Name.ToLower();

					// <file>
					if (elementname == "file")
					{
						// Construct the ZipNode element to be populated
						ZipNode node = new ZipNode();

						// Required: source= 
						node.Source = element.GetAttribute("source");
						if (String.IsNullOrEmpty(node.Source)) throw new Exception("missing or empty attribute 'source'");

						// Required: path=
						node.Path = element.GetAttribute("path");
						if (String.IsNullOrEmpty(node.Path)) throw new Exception("missing or empty attribute 'path'");

						// Optional: normalize= [default: none]
						string normalize = element.GetAttribute("normalize");
						if (!String.IsNullOrEmpty(normalize) && !Enum.TryParse<ZipNormalizeType>(normalize, true, out node.Normalize)) throw new Exception("unable to parse attribute 'normalize'");

						// Add the ZipNode instance into the collection
						zipnodes.Add(node);
					}

					// <directory>
					else if (elementname == "directory")
					{
						// Required: source=
						string source = element.GetAttribute("source");
						if (String.IsNullOrEmpty(source)) throw new Exception("missing or empty attribute 'source'");

						// Required: path= [default: root]
						string path = element.GetAttribute("path");
						if (path == null) path = String.Empty;
						if (!String.IsNullOrEmpty(path)) path = path.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;

						// Optional: recursive= [default: true]
						bool recursive = true;
						string recursivestr = element.GetAttribute("recursive");
						if (!String.IsNullOrEmpty(recursivestr) && !Boolean.TryParse(recursivestr, out recursive)) throw new Exception("unable to parse attribute 'recursive'");

						// Optional: filter= [default: *.*]
						string filter = element.GetAttribute("filter");
						if (String.IsNullOrEmpty(filter)) filter = "*.*";

						// Generate ZipNodes for each file that matches the directory specification
						foreach (string file in Directory.GetFiles(source, filter, (recursive) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
						{
							// Construct the ZipNode element to be populated
							ZipNode node = new ZipNode();

							// Set the source file name
							node.Source = file;

							// Generate and set a relative path to the file within the archive, don't allow
							// leading path separators if the base path is the root, that messes up the zip file
							if (!file.StartsWith(source)) throw new Exception("unexpected base path detected when adding directory file(s)");
							node.Path = (path + file.Substring(source.Length).Trim(Path.DirectorySeparatorChar));

							// Add the ZipNode instance into the collection
							zipnodes.Add(node);
						}
					}

					else throw new Exception("unexpected element <" + elementname + ">");
				}

				// Reformat the exception to include some context, the standard System.Xml.XmlDocument object
				// does not maintain line/column numbers so dump the first 40 characters of the line
				catch (Exception ex) { throw new Exception(String.Format("Manifest element [{0}...]: {1}", element.OuterXml.Substring(0, 40), ex.Message)); }
			}

			return zipnodes;
		}

		/// <summary>
		/// Normalizes the line endings of a text file
		/// </summary>
		/// <param name="filename">Input file name</param>
		/// <param name="type">Normalization type</param>
		private static string NormalizeFile(string filename, ZipNormalizeType type)
		{
			if (!File.Exists(filename)) throw new FileNotFoundException("specified text file [" + filename + "] does not exist", filename);
			if (type == ZipNormalizeType.None) throw new ArgumentException("type");

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
						// Select either Windows or Unix line endings to apply to the file
						sw.NewLine = (type == ZipNormalizeType.Unix) ? "\n" : "\r\n";

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
			Console.WriteLine("zipper - zip archive creation tool");
			Console.WriteLine();
			Console.WriteLine("Usage:");
			Console.WriteLine();
			Console.WriteLine("> zipper create {zipfile} {manifest} [-md5] [-sha256]");
			Console.WriteLine();
			Console.WriteLine("Creates a new ZIP archive based on an archive manifest");
			Console.WriteLine();
			Console.WriteLine("    zipfile   - zip archive file to be created");
			Console.WriteLine("    manifest  - manifest file describing the archive contents");
			Console.WriteLine("    -md5      - create {zipfile}.md5 hash file");
			Console.WriteLine("    -sha256   - create {zipfile}.sha256 hash file");
			Console.WriteLine();
		}

		/// <summary>
		/// Attempts to create a directory; does not throw any exceptions
		/// </summary>
		/// <param name="path">Path to the file to be deleted</param>
		private static bool TryCreateDirectory(string path)
		{
			try { Directory.CreateDirectory(path); return true; }
			catch { return false; }
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
