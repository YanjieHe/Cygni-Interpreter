# Cygni
A lightweight, object-oriented script interpreter implemented in C#.

The Cygni language is inspired by many script languages, such as Lua, Python, etc. 

## How to run the Cygni interpreter?
Here are two options. 
The first option is to download the compiled interpreter.
- Download the program from 

https://github.com/JasonHe0727/Cygni/tree/master/Cygni.Mono.Test/Cygni_CLI/Cygni_CLI/bin/Release
- If you are using windows, just double click the Cygni_CLI.exe. 

- If you are using Linux, you need to install mono environment and input "mono Cygni_CLI.exe" in the terminal.


The second option is compiled one by yourself. 
- Download the Cygni Core from 

https://github.com/JasonHe0727/Cygni/tree/master/Cygni.Mono/Cygni/bin/Release

- Import the Cygni.dll to your project.

The API is very easy. Have a look at this example.
``` csharp
using System;
using Cygni.Executors;

namespace Cygni_CLI
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Engine engine = Engine.CreateInstance ();
			engine.ExecuteInConsole ();
		}
	}
}
```

## Some tutorial examples
For more information, see the short reference in https://github.com/JasonHe0727/Cygni/tree/master/Cygni.Doc/Cygni_short_ref.pdf


