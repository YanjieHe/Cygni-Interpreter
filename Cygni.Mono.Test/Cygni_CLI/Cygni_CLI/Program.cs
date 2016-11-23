using System;
using Cygni.Executors;
using Cygni.Settings;
namespace Cygni_CLI
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Engine engine = Engine.CreateInstance ();
			if (args.Length == 1) {
				string filePath = args [0];
				engine.DoFile (filePath);
				return;
			}
			// GlobalSettings.CompleteErrorOutput = true;
			engine.ExecuteInConsole ();
		}
	}
}
