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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace zuki.build.tools
{
	/// <summary>
	/// Collection of Message objects, loaded from an input MC text file
	/// </summary>
	internal class Messages : ReadOnlyCollection<Message>
	{
		/// <summary>
		/// Instance Constructor
		/// </summary>
		/// <param name="typedef">Value of the MessageIdTypedef indicator</param>
		/// <param name="messages">List<> of message objects to add on creation</param>
		private Messages(string typedef, List<Message> messages) : base(messages.AsReadOnly())
		{
			m_typedef = typedef;
		}

		/// <summary>
		/// Creates a new MessageFile instance by parsing an input .MC text file
		/// </summary>
		/// <param name="path">Path to the input MC text file</param>
		/// <param name="unicode">Flag if the target process will have _UNICODE defined</param>
		public static Messages Load(string path, bool unicode)
		{
			string typedef = "DWORD";				// Default MessageIdTypedef value

			// List<> to pass to the MessageFile constructor
			List<Message> messages = new List<Message>();

			// Attempt to open the provided path as a normal text file
			using (StreamReader sr = File.OpenText(path))
			{
				string nextline = sr.ReadLine();
				while (nextline != null)
				{
					// MESSAGEIDTYPEDEF=
					//
					// Change the current message id typedef string to reflect this data type
					if (nextline.TrimStart().StartsWith("MessageIdTypedef=", StringComparison.OrdinalIgnoreCase))
					{
						if (nextline.Length > 17) typedef = nextline.Substring(17);
						nextline = sr.ReadLine();
					}

					else if (nextline.TrimStart().StartsWith("MessageId=", StringComparison.OrdinalIgnoreCase))
					{
						string symbolicname = String.Empty;
						string classname = String.Empty;
						List<string> argumentnames = new List<String>();
						StringBuilder messagetext = new StringBuilder();

						// Inside a message declaration, process it.  The MessageId= line itself is ignored,
						// so start with the next line of text
						nextline = sr.ReadLine();
						while (nextline != null)
						{
							nextline = nextline.TrimStart();

							// ;//EXCEPTIONNAME=
							//
							// Custom tag to use to force a specific exception class name.  Optional argument
							// names are comma delimited after the class name
							if (nextline.StartsWith(";//ExceptionName=", StringComparison.OrdinalIgnoreCase))
							{
								if (nextline.Length > 17)
								{
									string[] parts = nextline.Substring(17).Split(new char[] { ',' });

									classname = parts[0];
									for (int index = 1; index < parts.Length; index++) argumentnames.Add(parts[index]);
								}

								nextline = sr.ReadLine();
							}

							// MESSAGEIDTYPEDEF=
							//
							// Change the current message id typedef string to reflect this data type
							if (nextline.StartsWith("MessageIdTypedef=", StringComparison.OrdinalIgnoreCase))
							{
								if (nextline.Length > 17) typedef = nextline.Substring(17);
								nextline = sr.ReadLine();
							}

							// SEVERITY=
							//
							// Ignored
							if (nextline.StartsWith("Severity=", StringComparison.OrdinalIgnoreCase))
							{
								nextline = sr.ReadLine();
							}

							// FACILITY=
							//
							// Ignored
							else if (nextline.StartsWith("Facility=", StringComparison.OrdinalIgnoreCase))
							{
								nextline = sr.ReadLine();
							}

							// SYMBOLICNAME=
							//
							// Set the symbolic name for the current message
							else if (nextline.StartsWith("SymbolicName=", StringComparison.OrdinalIgnoreCase))
							{
								if (nextline.Length > 13)
									symbolicname = nextline.Substring(13).TrimEnd();
								
								nextline = sr.ReadLine();
							}

							// OUTPUTBASE=
							//
							// Ignored
							else if (nextline.StartsWith("OutputBase=", StringComparison.OrdinalIgnoreCase))
							{
								nextline = sr.ReadLine();
							}

							// LANGUAGE=
							//
							// Ignored
							else if (nextline.StartsWith("Language=", StringComparison.OrdinalIgnoreCase))
							{
								nextline = sr.ReadLine();
							}

							// MESSAGE TEXT
							//
							// Read until single . line of text encountered
							else
							{
								messagetext.AppendLine(nextline);
								nextline = sr.ReadLine();
								while ((nextline != null) && (!nextline.TrimStart().StartsWith(".")))
								{
									messagetext.AppendLine(nextline);
									nextline = sr.ReadLine();
								}

								// If the required message information was collected, add it to the List<>
								if (!String.IsNullOrEmpty(classname) && !String.IsNullOrEmpty(symbolicname) && (messagetext.Length > 0))
									messages.Add(new Message(symbolicname, classname, messagetext.ToString(), argumentnames, unicode));

								break;
							}
						}
					}

					nextline = sr.ReadLine();
				}
			}

			// Pass the List<> instance to the MessageFile constructor
			return new Messages(typedef, messages);
		}

		/// <summary>
		/// Gets the C++ data type defined as the message id data type
		/// </summary>
		public string MessageIdTypedef
		{
			get { return m_typedef; }
		}

		/// <summary>
		/// Value of the input file's MessageIdTypedef
		/// </summary>
		private string m_typedef;
	}
}
