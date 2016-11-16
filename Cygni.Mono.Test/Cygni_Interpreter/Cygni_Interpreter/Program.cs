using System;
using Cygni.Executors;
namespace Cygni_Interpreter
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var engine = Engine.CreateInstance ();
			//GlobalSettings.CompleteErrorOutput = true;
			//engine.DoFile ("/home/jasonhe/MyCode/CygniCode/regex.cyg");
			//engine.DoFile ("/home/jasonhe/MyCode/CygniCode/random.cyg");
			//engine.DoFile ("/home/jasonhe/MyCode/CygniCode/File.cyg");
			//engine.DoFile ("/home/jasonhe/MyCode/CygniCode/reader.cyg");
			//			engine.DoFile ("/home/jasonhe/MyCode/CygniCode/stopwatch.cyg");
			//			engine.DoFile ("/home/jasonhe/MyCode/CygniCode/append_test.cyg");

			//engine.DoFile ("/home/jasonhe/MyCode/CygniCode/MySQL.cyg");

			//engine.DoFile ("/home/jasonhe/MyCode/CygniCode/post_data.cyg");
			//engine.DoFile ("/home/jasonhe/MyCode/CygniCode/fib.cyg");
			//engine.DoFile ("/home/jasonhe/MyCode/CygniCode/fibtest.cyg");
			//engine.DoFile ("/home/jasonhe/MyCode/CygniCode/word2vec.cyg");
			//engine.DoFile ("/home/jasonhe/MyCode/CygniCode/get_data.cyg");
			engine.ExecuteInConsole ();
		}
	}
}
