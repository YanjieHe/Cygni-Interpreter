using System;
using System.Collections.Generic;
using Cygni.DataTypes;
using Cygni.Settings;
using Cygni.AST;
using Cygni.AST.Scopes;
using Cygni.Errors;
using Cygni.DataTypes.Interfaces;
using System.Linq;

namespace Cygni.Libraries
{
	public static class FunctionalProgrammingLibrary
	{
		public static DynValue Map (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "map");
			IFunction func = args [0].As<IFunction> ();
			IEnumerable<DynValue> collection = args [1].As<IEnumerable<DynValue>> ();
			return DynValue.FromUserData (collection.Select (
				arg => func.DynInvoke (new DynValue[]{ arg })));
		}

		public static DynValue Filter (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "filter");
			IFunction func = args [0].As<IFunction> ();
			IEnumerable<DynValue> collection = args [1].As<IEnumerable<DynValue>> ();
			return DynValue.FromUserData (collection.Where (
				arg => func.DynInvoke (new DynValue[]{ arg }).AsBoolean ()));
		}

		public static DynValue Reduce (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "reduce");
			IFunction func = args [0].As<IFunction> ();
			IEnumerable<DynValue> collection = args [1].As<IEnumerable<DynValue>> ();
			return DynValue.FromUserData (collection.Aggregate (
				(arg1, arg2) => func.DynInvoke (new DynValue[]{ arg1, arg2 })));
		}
	}
}

