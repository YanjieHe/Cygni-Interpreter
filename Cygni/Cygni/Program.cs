using System;
using Cygni.Executors;
using Cygni.AST;
using Cygni.DataTypes;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Cygni
{
	class MainClass
	{
		public static void Main (string[] main_args)
		{
			var engine = Engine.CreateInstance ();
			var regex = ClassInfo.CreateClass (
				            null, "regex",
				new string[0], new DynValue[0]);
			ClassInfo.AddMethod (regex, "is_match", new string[]{ "input" }, "self_regex",
				(args) => args [0].As<Regex> ().IsMatch (args [1].AsString ()));
			engine.SetSymbol ("regex", DynValue.FromClass (regex));
			engine.ExecuteInConsole ();
		}
	}
}
