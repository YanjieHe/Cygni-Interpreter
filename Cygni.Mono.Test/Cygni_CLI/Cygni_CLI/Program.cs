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
