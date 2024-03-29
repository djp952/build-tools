﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace zuki.build.tools
{
    using System.IO;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class MessageExceptions : MessageExceptionsBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("//\r\n// Code automatically generated by ");
            
            #line 7 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName).ToUpper()));
            
            #line default
            #line hidden
            this.Write(" tool\r\n//\r\n// Command Line:\r\n//\r\n// ");
            
            #line 11 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Environment.CommandLine));
            
            #line default
            #line hidden
            this.Write("\r\n//\r\n\r\n#ifndef __AUTOGEN_");
            
            #line 14 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(m_classname.ToUpper()));
            
            #line default
            #line hidden
            this.Write("_H_\r\n#define __AUTOGEN_");
            
            #line 15 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(m_classname.ToUpper()));
            
            #line default
            #line hidden
            this.Write("_H_\r\n#pragma once\r\n\r\n#include <exception>\r\n#include <vector>\r\n#include <type_trai" +
                    "ts>\r\n#include <Windows.h>\r\n");
            
            #line 22 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
 foreach(string include in m_includes) { 
            
            #line default
            #line hidden
            this.Write("#include ");
            
            #line 23 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(include));
            
            #line default
            #line hidden
            this.Write(" \r\n");
            
            #line 24 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\r\n#pragma warning(push, 4)\r\n\r\n");
            
            #line 28 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
 if(m_unicode) { 
            
            #line default
            #line hidden
            this.Write("#ifndef _UNICODE\r\n#error Exception classes generated with -unicode flag; project " +
                    "must #define _UNICODE\r\n#endif\t// _UNICODE\r\n");
            
            #line 32 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
 } else { 
            
            #line default
            #line hidden
            this.Write(" \r\n#ifdef _UNICODE\r\n#error Exception classes generated without -unicode flag; pro" +
                    "ject must not #define _UNICODE\r\n#endif\r\n");
            
            #line 36 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\r\n//-----------------------------------------------------------------------------" +
                    "\r\n// __autogen_");
            
            #line 39 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(m_classname));
            
            #line default
            #line hidden
            this.Write("\r\n//\r\n// Auto-generated base class for custom exceptions declared below\r\n\r\nclass " +
                    "__autogen_");
            
            #line 43 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(m_classname));
            
            #line default
            #line hidden
            this.Write(" : public std::exception\r\n{\r\npublic:\r\n\r\n\t// Copy Constructor\r\n\t//\r\n\t__autogen_");
            
            #line 49 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(m_classname));
            
            #line default
            #line hidden
            this.Write("(__autogen_");
            
            #line 49 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(m_classname));
            
            #line default
            #line hidden
            this.Write(" const& rhs) : m_what(rhs.m_what), m_owned(false) \r\n\t{\r\n\t}\r\n\r\n\t// Destructor\r\n\t//" +
                    "\r\n\tvirtual ~__autogen_");
            
            #line 55 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(m_classname));
            
            #line default
            #line hidden
            this.Write(@"()
	{
		if(m_owned && (m_what != nullptr)) LocalFree(m_what);
	}

	// std::exception::what
	//
	// Gets a pointer to the exception message text
	virtual char const* what(void) const 
	{ 
		return m_what;
	}

	// s_module
	//
	// Initialized to the module handle of this compilation unit
	static HMODULE const s_module;

protected:

	// messageid_t
	//
	// Message identifier data type
	using messageid_t = ");
            
            #line 78 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(m_messages.MessageIdTypedef));
            
            #line default
            #line hidden
            this.Write(";\r\n\r\n\tstatic_assert(sizeof(messageid_t) == sizeof(DWORD32), \"Size of message iden" +
                    "tifier type must be 32 bits for compatibility with FormatMessage\");\r\n\r\n\t// Insta" +
                    "nce Constructor\r\n\t//\r\n\ttemplate <typename... _insertions>\r\n\t__autogen_");
            
            #line 85 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(m_classname));
            
            #line default
            #line hidden
            this.Write("(messageid_t messageid, _insertions const&... insertions) : m_what(AllocateMessag" +
                    "e(messageid, insertions...)), m_owned((m_what != nullptr)) \r\n\t{\r\n\t}\r\n\r\nprivate:\r" +
                    "\n\r\n\t// is_charpointer<_type>\r\n\t//\r\n\t// Type traits used to determine if a templa" +
                    "te argument is of type char*\r\n\ttemplate<typename _type> struct is_charpointer : " +
                    "public std::false_type {};\r\n\ttemplate<> struct is_charpointer<char*> : public st" +
                    "d::true_type {};\r\n\ttemplate<> struct is_charpointer<char const*> : public std::t" +
                    "rue_type {};\r\n\r\n\t// is_wcharpointer<_type>\r\n\t//\r\n\t// Type traits used to determi" +
                    "ne if a template argument is of type wchar_t*\r\n\ttemplate<typename _type> struct " +
                    "is_wcharpointer : public std::false_type {};\r\n\ttemplate<> struct is_wcharpointer" +
                    "<wchar_t*> : public std::true_type {};\r\n\ttemplate<> struct is_wcharpointer<wchar" +
                    "_t const*> : public std::true_type {};\r\n\r\n\t// AllocateMessage\r\n\t//\r\n\t// Generate" +
                    "s the formatted exception message based on the message identifier and insertions" +
                    "\r\n\ttemplate<typename... _remaining>\r\n\tstatic char* AllocateMessage(messageid_t m" +
                    "essageid, _remaining const&... remaining)\r\n\t{\r\n\t\tstd::vector<DWORD_PTR> argument" +
                    "s;\r\n\t\treturn AllocateMessage(messageid, arguments, remaining...);\r\n\t}\r\n\r\n\t// All" +
                    "ocateMessage\r\n\t//\r\n\t// Intermediate variadic overload; converts a single inserti" +
                    "on argument into a DWORD_PTR value\r\n\ttemplate<typename _first, typename... _rema" +
                    "ining>\r\n\tstatic auto AllocateMessage(messageid_t messageid, std::vector<DWORD_PT" +
                    "R>& arguments, _first const& first, _remaining const&... remaining)\r\n\t\t-> typena" +
                    "me std::enable_if<!is_charpointer<_first>::value && !is_wcharpointer<_first>::va" +
                    "lue, char*>::type\r\n\t{\r\n\t\tstatic_assert(!std::is_floating_point<_first>::value, \r" +
                    "\n\t\t\t\"Floating point values cannot be specified as insertions to FormatMessage\");" +
                    "\r\n\t\t\r\n\t\tstatic_assert(!(std::is_integral<_first>::value && sizeof(_first) > size" +
                    "of(DWORD_PTR)), \r\n\t\t\t\"Integral values larger than 32 bits in size cannot be spec" +
                    "ified as insertions to FormatMessage\");\r\n\r\n\t\targuments.push_back((DWORD_PTR)firs" +
                    "t);\r\n\t\treturn AllocateMessage(messageid, arguments, remaining...);\r\n\t}\r\n\r\n\t// Al" +
                    "locateMessage\r\n\t//\r\n\t// Intermediate variadic overload; specialized for char* da" +
                    "ta types to handle null pointers\r\n\ttemplate<typename _first, typename... _remain" +
                    "ing>\r\n\tstatic auto AllocateMessage(messageid_t messageid, std::vector<DWORD_PTR>" +
                    "& arguments, _first const& first, _remaining const&... remaining) \r\n\t\t-> typenam" +
                    "e std::enable_if<is_charpointer<_first>::value, char*>::type\r\n\t{\r\n\t\targuments.pu" +
                    "sh_back(reinterpret_cast<DWORD_PTR>(first == nullptr ? s_null : first));\r\n\t\tretu" +
                    "rn AllocateMessage(messageid, arguments, remaining...);\r\n\t}\r\n\r\n\t// AllocateMessa" +
                    "ge\r\n\t//\r\n\t// Intermediate variadic overload; specialized for wchar_t* data type " +
                    "to handle null pointers\r\n\ttemplate<typename _first, typename... _remaining>\r\n\tst" +
                    "atic auto AllocateMessage(messageid_t messageid, std::vector<DWORD_PTR>& argumen" +
                    "ts, _first const& first, _remaining const&... remaining) \r\n\t\t-> typename std::en" +
                    "able_if<is_wcharpointer<_first>::value, char*>::type\r\n\t{\r\n\t\targuments.push_back(" +
                    "reinterpret_cast<DWORD_PTR>(first == nullptr ? s_widenull : first));\r\n\t\treturn A" +
                    "llocateMessage(messageid, arguments, remaining...);\r\n\t}\r\n\r\n\t// AllocateMessage\r\n" +
                    "\t//\r\n\t// Final variadic overload; generates the formatted message string with th" +
                    "e collected insertions\r\n\tstatic char* AllocateMessage(messageid_t messageid, std" +
                    "::vector<DWORD_PTR>& arguments)\r\n\t{\r\n\t\tLPTSTR message = nullptr;\t\t\t\t\t// Allocate" +
                    "d string from ::FormatMessage\r\n\r\n\t\t// Attempt to format the message from the cur" +
                    "rent module resources and provided insertions\r\n\t\tDWORD cchmessage = ::FormatMess" +
                    "age(FORMAT_MESSAGE_MAX_WIDTH_MASK | FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESS" +
                    "AGE_FROM_HMODULE | \r\n\t\t\tFORMAT_MESSAGE_ARGUMENT_ARRAY, s_module, static_cast<DWO" +
                    "RD>(messageid), ::GetThreadUILanguage(), reinterpret_cast<LPTSTR>(&message), \r\n\t" +
                    "\t\t0, reinterpret_cast<va_list*>(arguments.data())); \r\n\t\tif(cchmessage == 0) {\r\n\r" +
                    "\n\t\t\t// The message could not be looked up in the specified module; generate the " +
                    "default message instead\r\n\t\t\tif(message) { LocalFree(message); message = nullptr;" +
                    " }\r\n\t\t\tcchmessage = ::FormatMessage(FORMAT_MESSAGE_MAX_WIDTH_MASK | FORMAT_MESSA" +
                    "GE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_STRING | \r\n\t\t\t\tFORMAT_MESSAGE_ARGUMENT_" +
                    "ARRAY, s_defaultformat, 0, 0, reinterpret_cast<LPTSTR>(&message), 0, reinterpret" +
                    "_cast<va_list*>(&messageid));\r\n\t\t\tif(cchmessage == 0) {\r\n\r\n\t\t\t\t// The default me" +
                    "ssage could not be generated; give up\r\n\t\t\t\tif(message) ::LocalFree(message);\r\n\t\t" +
                    "\t\treturn nullptr;\r\n\t\t\t}\r\n\t\t}\r\n\r\n");
            
            #line 179 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
 if(m_unicode) { 
            
            #line default
            #line hidden
            this.Write(@"		// UNICODE projects need to convert the message string into CP_UTF8 or CP_ACP
		int convertcch = ::WideCharToMultiByte(CP_UTF8, 0, message, cchmessage, nullptr, 0, nullptr, nullptr);
		char* converted = reinterpret_cast<char*>(::LocalAlloc(LMEM_FIXED | LMEM_ZEROINIT, (convertcch + 1) * sizeof(char)));
		if(converted) ::WideCharToMultiByte(CP_UTF8, 0, message, cchmessage, converted, convertcch, nullptr, nullptr);

		LocalFree(message);
		return converted;
");
            
            #line 187 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
 } else { 
            
            #line default
            #line hidden
            this.Write(" \r\n\t\treturn message;\r\n");
            
            #line 189 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
 } 
            
            #line default
            #line hidden
            this.Write(@"	}

	// Member Variables
	//
	char* const							m_what;	
	bool const							m_owned;
	static constexpr char const*		s_null			= ""<null pointer>"";
	static constexpr wchar_t const*		s_widenull		= L""<null pointer>"";
	static constexpr LPCTSTR			s_defaultformat	= TEXT(""Exception code 0x%1!08lX! : The message for this exception could not be generated."");
};

// __autogen_");
            
            #line 201 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(m_classname));
            
            #line default
            #line hidden
            this.Write("::s_module\r\n//\r\n// Initialized to the module handle of this compilation unit\r\n__d" +
                    "eclspec(selectany)\r\nHMODULE const __autogen_");
            
            #line 205 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(m_classname));
            
            #line default
            #line hidden
            this.Write("::s_module = []() -> HMODULE {\r\n\r\n\tHMODULE module = nullptr;\r\n\r\n\t::GetModuleHandl" +
                    "eEx(GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS | GET_MODULE_HANDLE_EX_FLAG_UNCHANGED" +
                    "_REFCOUNT,\r\n\t\treinterpret_cast<LPCTSTR>(&__autogen_");
            
            #line 210 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(m_classname));
            
            #line default
            #line hidden
            this.Write("::s_module), &module);\r\n\r\n\treturn module;\r\n}();\r\n\r\n//----------------------------" +
                    "-------------------------------------------------\r\n// CUSTOM EXCEPTION CLASSES\r\n" +
                    "//-----------------------------------------------------------------------------\r" +
                    "\n\r\n");
            
            #line 219 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
 foreach(Message message in m_messages) { 
            
            #line default
            #line hidden
            this.Write("// ");
            
            #line 220 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(message.ClassName));
            
            #line default
            #line hidden
            this.Write(" (");
            
            #line 220 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(message.SymbolicName));
            
            #line default
            #line hidden
            this.Write(")\r\n//\r\n// ");
            
            #line 222 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(message.MessageText));
            
            #line default
            #line hidden
            this.Write("\r\nstruct ");
            
            #line 223 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(message.ClassName));
            
            #line default
            #line hidden
            this.Write(" : public __autogen_");
            
            #line 223 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(m_classname));
            
            #line default
            #line hidden
            this.Write("\r\n{\r\n\texplicit ");
            
            #line 225 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(message.ClassName));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 225 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
 for(int index = 0; index < message.Arguments.Count; index++) { 
            
            #line default
            #line hidden
            
            #line 225 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(message.Arguments[index].Key));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 225 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(message.Arguments[index].Value));
            
            #line default
            #line hidden
            
            #line 225 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
 if(index + 1 < message.Arguments.Count) { 
            
            #line default
            #line hidden
            this.Write(", ");
            
            #line 225 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
 } 
            
            #line default
            #line hidden
            
            #line 225 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
 } 
            
            #line default
            #line hidden
            this.Write(") : __autogen_");
            
            #line 225 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(m_classname));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 225 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(message.SymbolicName));
            
            #line default
            #line hidden
            
            #line 225 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
 for(int index = 0; index < message.Arguments.Count; index++) { 
            
            #line default
            #line hidden
            this.Write(", ");
            
            #line 225 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(message.Arguments[index].Value));
            
            #line default
            #line hidden
            
            #line 225 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
 } 
            
            #line default
            #line hidden
            this.Write(") {}\r\n\tvirtual ~");
            
            #line 226 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(message.ClassName));
            
            #line default
            #line hidden
            this.Write("()=default;\r\n};\r\n\r\n");
            
            #line 229 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
 } 
            
            #line default
            #line hidden
            this.Write("//-----------------------------------------------------------------------------\r\n" +
                    "\r\n#pragma warning(pop)\r\n\r\n#endif\t// __AUTOGEN_");
            
            #line 234 "D:\git\build-tools\mctoexception\MessageExceptions.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(m_classname.ToUpper()));
            
            #line default
            #line hidden
            this.Write("_H_");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public class MessageExceptionsBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
