using System;
using Cygni.Executors;
using Cygni.Settings;
using Cygni.AST.Scopes;
using System.IO;

namespace Cygni_CLI
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			/* GlobalSettings.CompleteErrorOutput = true;
			var str = File.ReadAllText ("fact.cyg");
			var scope = new BasicScope ();
			BasicScope.builtInScope = GlobalSettings.CreateBuiltInScope ();
			var executor = new CompilerExecutor (scope, str);
			executor.Run (); */

			Engine engine = Engine.CreateInstance ();
			if (args.Length == 1) {
				string filePath = args [0];
				engine.DoFile (filePath);
			} else {
				GlobalSettings.IsDebug = true;
				// GlobalSettings.CompleteErrorOutput = true;
				engine.ExecuteInConsole (); 
			}
		}
	}
}
