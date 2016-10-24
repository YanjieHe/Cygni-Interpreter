using System;
using Cygni.Executors;
using Cygni.AST;
using Cygni.DataTypes;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.IO;
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
			engine.DoFile ("/home/jasonhe/MyCode/CygniCode/File.cyg");
			engine.DoFile ("/home/jasonhe/MyCode/CygniCode/reader.cyg");
			engine.DoFile ("/home/jasonhe/MyCode/CygniCode/stopwatch.cyg");
			engine.DoFile ("/home/jasonhe/MyCode/CygniCode/MySQL.cyg");

			//engine.DoFile ("/home/jasonhe/MyCode/CygniCode/post_data.cyg");
			//engine.DoFile ("/home/jasonhe/MyCode/CygniCode/fib.cyg");
			//engine.DoFile ("/home/jasonhe/MyCode/CygniCode/fibtest.cyg");
			//engine.Evaluate("path = '/home/jasonhe/文档/公司资料/训练集.txt'");

			//.DoFile ("/home/jasonhe/文档/公司资料/read_data.cyg");
			engine.ExecuteInConsole ();
		}
	}
}
