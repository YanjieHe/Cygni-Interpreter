using System;
using Cygni.Executors;
using Cygni.AST;
using Cygni.DataTypes;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

using System.Reflection;
using System.Reflection.Emit;
namespace Cygni
{
	class MainClass
	{
		public static void Main (string[] main_args)
		{
			var engine = Engine.CreateInstance ();
			engine.DoFile ("/home/jasonhe/MyCode/CygniCode/regex.cyg");
			engine.DoFile ("/home/jasonhe/MyCode/CygniCode/random.cyg");
			engine.DoFile ("/home/jasonhe/MyCode/CygniCode/file.cyg");
			engine.DoFile ("/home/jasonhe/MyCode/CygniCode/reader.cyg");
			engine.DoFile ("/home/jasonhe/MyCode/CygniCode/stopwatch.cyg");
			engine.DoFile ("/home/jasonhe/MyCode/CygniCode/fib.cyg");
			//engine.DoFile ("/home/jasonhe/MyCode/CygniCode/fibtest.cyg");

			engine.ExecuteInConsole ();
		}
	}
}
