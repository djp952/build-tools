#build-tools
Common build tools and code generation utilities


##**MKVERSION**
This tool generates version resource source files for inclusion in Visual Studio projects of various types. Version 3 reverts the tool back to a console application, adds all available properties into the version.ini and as command line switches, and replaces the embedded resource text files with runtime text templates. License has been switched to the MIT license. Requires .NET Framework v4.5 or higher.

```
Usage:

ZUKI.BUILD.TOOLS.MKVERSION.EXE outdir [-ini:inifile] [-clean] [-rebuild] [-format:fmt[,fmt]] [-company:companyname]  
[-copyright:copyrightstring] [-product:productname] [-version:major[.minor[.build[.revision]]]]  

Automates creation of version resource file(s) for Visual Studio projects.

  outdir     : Output directory; will be created if it does not exist
  -ini       : Retrieve version properties from specified version.ini file
  -clean     : Clean the output directory, do not generate version file(s)
  -rebuild   : Rebuild existing file(s) in output directory
  -format    : Comma-delimited list of format specifiers (default: all)
  -company   : Company name property for generated version file(s)
  -copyright : Copyright string for generated file(s) (default: auto-generate)
  -product   : Product name property for generated version file(s)
  -version   : Version property for generated version file(s).  See below.

-version format:

  Version string must be of the format major[.minor[.build[.revision]]]

  major      : Major version number (default: 0)
  minor      : Minor version number (default: 0)
  build      : Build number (default: 0)
  revision   : Revision number (default is days since Jan. 1, 2000)

-format specifiers:

  all - Generate all supported formats
  cpp - Managed C++/CLI project (version.cpp)
  cs  - Managed C# project (version.cs)
  ini - Configuration file for this utility (version.ini)
  rc  - Unmanaged C/C++ project resource script (version.rc)
  txt - Generic text file (version.txt)
  vb  - Managed VB.NET project (version.vb)
  wxi - WiX installer project (version.wxi)
```
  
##MCTOEXCEPTION
This tool converts the contents of a Message Compiler .MC file into a set of C++ exception classes  
  
In the .MC file, the only customization is to put a special `;//ExceptionName=` tag in a message declaration to enable exception class generation and specify the name of the class.  An optional comma-delimited list of argument names can follow the class name:
  
`;//ExceptionName=MyException,argument1,argument2,argumentN`  
  
```
Usage:  
  
ZUKI.BUILD.TOOLS.MCTOEXCEPTION.EXE inputfile.mc outputfile.h [-unicode] [-include:"include.h"]  
  
  -unicode      - Generate for a project that will define _UNICODE  
  -include:     - Add an #include directive for the specified file(s)  
  inputfile.mc  - Input message compiler .MC text file  
  outputfile.h  - Output C++ header file  
```