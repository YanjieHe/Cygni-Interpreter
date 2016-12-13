using System;
using Cygni.Executors;
using Cygni.Settings;
using Cygni.AST.Scopes;
namespace Cygni_CLI
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			/* var str = Console.ReadLine ();
			var scope = new BasicScope ();
			var executor = new CompilerExecutor (scope, str);
			executor.Run (); */
			Engine engine = Engine.CreateInstance ();
			if (args.Length == 1) {
				string filePath = args [0];
				engine.DoFile (filePath);
				return;
			}
			GlobalSettings.IsDebug = true;
			// GlobalSettings.CompleteErrorOutput = true;
			engine.ExecuteInConsole ();
		}
	}
}
