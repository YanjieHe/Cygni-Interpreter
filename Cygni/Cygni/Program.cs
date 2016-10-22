using System;
using Cygni.Executors;
namespace Cygni
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var engine = Engine.CreateInstance ();
			engine.ExecuteInConsole ();
		}
	}
}
