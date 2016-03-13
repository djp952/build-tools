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
using System.Text.RegularExpressions;

namespace zuki.build.tools
{
	/// <summary>
	/// Creates exception class information based on an input message declaration
	/// </summary>
	internal class Message
	{
		/// <summary>
		/// Instance Constructor
		/// </summary>
		/// <param name="symbolicname">Symbolic name of the message identifier</param>
		/// <param name="classname">Name to assign to the generated exception class</param>
		/// <param name="messagetext">Message text containing the insertions</param>
		/// <param name="argumentnames">Collection of argument names to apply to the insertions</param>
		/// <param name="unicode">Flag if the target project is being built with _UNICODE enabled</param>
		public Message(string symbolicname, string classname, string messagetext, List<string> argumentnames, bool unicode)
		{
			m_symbolicname = symbolicname;			// Store the provided symbolic name
			m_classname = classname;                // Store the provided class name

			m_message = messagetext.TrimEnd(new char[] { ' ', '\r', '\n' });
			
			// Collection of arguments
			List<KeyValuePair<string, string>> arguments = new List<KeyValuePair<string, string>>();

			int lastindex = 0;						// Last seen insertion index

			// Parse the insertions of the message text using a regular expression
			foreach (Match match in Regex.Matches(m_message, @"%(?<index>[1-9]?[0-9])(!(?<formatspecifier>\w*)!)?"))
			{
				// Extract the index and format specifier, if there is no format it defaults to "s"
				int index = Int32.Parse(match.Groups["index"].Value);
				string formatspecifier = match.Groups["formatspecifier"].Value;
				if (String.IsNullOrEmpty(formatspecifier)) formatspecifier = "s";

				string argumentname = (argumentnames.Count >= index) ? argumentnames[index - 1] : null;
				if (String.IsNullOrEmpty(argumentname)) argumentname = "insert" + index.ToString();

				// In the event there is a missing index, add a String type in there as a placeholder
				while (index > (lastindex + 1))
				{
					arguments.AddRange(ProcessInsertionString("s", argumentname, unicode));
					lastindex++;
				}

				// Convert the insertion into a List<> of named constructor arguments
				arguments.AddRange(ProcessInsertionString(formatspecifier, argumentname, unicode));
				lastindex = index;
			}

			m_arguments = arguments.AsReadOnly();		// Store as read-only collection
        }

		/// <summary>
		/// Converts an insertion string into C++ argument type/name pairs
		/// </summary>
		/// <param name="format">Insertion string format</param>
		/// <param name="argumentname">Argument name for this insertion string</param>
		/// <param name="unicode">Project _UNICODE flag</param>
		/// <returns></returns>
		static List<KeyValuePair<string, string>> ProcessInsertionString(string format, string argumentname, bool unicode)
		{
			List<KeyValuePair<string, string>> types = new List<KeyValuePair<string, string>>();

			//
			// TODO: Handle automatic width and precision (*) specifiers, use the same argument name
			// with _width or _precision appended to it.  That's why this returns a List<>
			//

			switch (format)
			{
				case "c":	types.Add(new KeyValuePair<string, string>("wchar_t", argumentname)); break;
				case "C":	types.Add(new KeyValuePair<string, string>("wchar_t", argumentname)); break;
				case "d":	types.Add(new KeyValuePair<string, string>("int", argumentname)); break;
				case "hc":	types.Add(new KeyValuePair<string, string>("wchar_t", argumentname)); break;
				case "hC":	types.Add(new KeyValuePair<string, string>("wchar_t", argumentname)); break;
				case "hd":	types.Add(new KeyValuePair<string, string>("short", argumentname)); break;
				case "hs":	types.Add(new KeyValuePair<string, string>("char const*", argumentname)); break;
				case "hS":	types.Add(new KeyValuePair<string, string>("char const*", argumentname)); break;
				case "hu":	types.Add(new KeyValuePair<string, string>("unsigned short", argumentname)); break;
				case "i":	types.Add(new KeyValuePair<string, string>("int", argumentname)); break;
				case "lc":	types.Add(new KeyValuePair<string, string>("wchar_t", argumentname)); break;
				case "lC":	types.Add(new KeyValuePair<string, string>("wchar_t", argumentname)); break;
				case "ld":	types.Add(new KeyValuePair<string, string>("long", argumentname)); break;
				case "li":	types.Add(new KeyValuePair<string, string>("long", argumentname)); break;
				case "ls":	types.Add(new KeyValuePair<string, string>("wchar_t const*", argumentname)); break;
				case "lS":	types.Add(new KeyValuePair<string, string>("wchar_t const*", argumentname)); break;
				case "lu":	types.Add(new KeyValuePair<string, string>("unsigned long", argumentname)); break;
				case "lx":	types.Add(new KeyValuePair<string, string>("unsigned long", argumentname)); break;
				case "lX":	types.Add(new KeyValuePair<string, string>("unsigned long", argumentname)); break;
				case "p":	types.Add(new KeyValuePair<string, string>("void const*", argumentname)); break;
				case "s":	types.Add(new KeyValuePair<string, string>((unicode) ? "wchar_t const*" : "char const*", argumentname)); break;
				case "S":	types.Add(new KeyValuePair<string, string>((unicode) ? "char const*" : "wchar_t const*", argumentname)); break;
				case "u":	types.Add(new KeyValuePair<string, string>("unsigned int", argumentname)); break;
				case "x":	types.Add(new KeyValuePair<string, string>("unsigned int", argumentname)); break;
				case "X":	types.Add(new KeyValuePair<string, string>("unsigned int", argumentname)); break;

				default:	throw new Exception("Unrecognized MC file insertion string format [" + format + "]");
			}

			return types;
		}

		/// <summary>
		/// Gets the collected of argument types/names
		/// </summary>
		public ReadOnlyCollection<KeyValuePair<string, string>> Arguments
		{
			get { return m_arguments; }
		}

		/// <summary>
		/// Gets the exception class name
		/// </summary>
		public string ClassName
		{
			get { return m_classname; }
		}

		/// <summary>
		/// Gets the collected message text
		/// </summary>
		public string MessageText
		{
			get { return m_message; }
		}

		/// <summary>
		/// Gets the collected symbolic name
		/// </summary>
		public string SymbolicName
		{
			get { return m_symbolicname; }
		}

		/// <summary>
		/// Symbolic name of the message identifier
		/// </summary>
		private string m_symbolicname;
		
		/// <summary>
		/// Original message text
		/// </summary>
		private string m_message;

		/// <summary>
		/// Name to assign to the resultant exception class
		/// </summary>
		private string m_classname;

		/// <summary>
		/// Collection of constructor argument type/name pairs
		/// </summary>
		private ReadOnlyCollection<KeyValuePair<string, string>> m_arguments;
	}
}
