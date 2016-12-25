using System;
using Cygni.DataTypes;
using Cygni.Errors;

namespace CygniLib.Random
{
	public static class random_funcs
	{
		public static DynValue Random (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 0 || args.Length == 1, "Random");
			if (args.Length == 0)
				return DynValue.FromUserData (new CygniRandom ());
			else
				return DynValue.FromUserData (new CygniRandom (args [0].AsInt32 ()));
		}
	}
}

