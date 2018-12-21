//-----------------------------------------------------------------------------
// Copyright (c) 2018 Michael G. Brehm
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

namespace zuki.build.tools
{
	/// <summary>
	/// Holds node information loaded from the manifest.xml file
	/// </summary>
	class ZipNode
	{
		/// <summary>
		/// Instance Constructor
		/// </summary>
		public ZipNode()
		{
		}

		/// <summary>
		/// Path to the source file if applicable
		/// </summary>
		public string Source = String.Empty;

		/// <summary>
		/// Destination path within the generated archive
		/// </summary>
		public string Path = String.Empty;

		/// <summary>
		/// Flag to normalize file node line endings before output
		/// </summary>
		public ZipNormalizeType Normalize = ZipNormalizeType.None;
	}
}
