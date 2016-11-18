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
			// GlobalSettings.CompleteErrorOutput = true;
			engine.ExecuteInConsole ();
		}
	}
}
