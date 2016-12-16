using System;
using Cygni.DataTypes;
using Cygni.Errors;
namespace CygniLib.random
{
	public static class random_funcs
	{
		public static DynValue Random(DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 0 || args.Length == 1, "Random");
			if (args.Length == 0)
				return DynValue.FromUserData( new CygniLib.random.random ());
			else
				return DynValue.FromUserData( new CygniLib.random.random ((int)args [0].AsNumber ()));
		}
	}
}

