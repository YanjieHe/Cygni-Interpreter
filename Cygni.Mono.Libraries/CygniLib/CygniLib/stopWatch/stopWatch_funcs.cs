using System;
using System.Diagnostics;
using Cygni.DataTypes;
using Cygni.Errors;

namespace CygniLib.stopWatch
{
	public static class stopWatch_funcs
	{
		public static DynValue StopWatch (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 0, "StopWatch");
			return DynValue.FromUserData (new CygniLib.stopWatch.stopWatch ());
		}
	}
}

