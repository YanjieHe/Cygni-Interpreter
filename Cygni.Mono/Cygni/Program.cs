using System;
using Cygni.Executors;
using Cygni.AST;
using Cygni.DataTypes;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using Cygni.Settings;
using System.Text;
namespace Cygni
{
	class MainClass
	{
		public static void Main (string[] main_args)
		{
			var engine = Engine.CreateInstance ();
			GlobalSettings.CompleteErrorOutput = true;
			//engine.DoFile ("/home/jasonhe/MyCode/CygniCode/regex.cyg");
			//engine.DoFile ("/home/jasonhe/MyCode/CygniCode/random.cyg");
			//engine.DoFile ("/home/jasonhe/MyCode/CygniCode/File.cyg");
			//engine.DoFile ("/home/jasonhe/MyCode/CygniCode/reader.cyg");
			//engine.DoFile ("/home/jasonhe/MyCode/CygniCode/stopwatch.cyg");
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
