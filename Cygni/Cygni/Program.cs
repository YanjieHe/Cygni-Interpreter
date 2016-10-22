using System;
using Cygni.Executors;
using Cygni.AST;
using Cygni.DataTypes;
using System.Linq.Expressions;
namespace Cygni
{
	class MainClass
	{
		public static void Main (string[] main_args)
		{
			var engine = Engine.CreateInstance ();
			engine.ExecuteInConsole ();
		}
	}
}
