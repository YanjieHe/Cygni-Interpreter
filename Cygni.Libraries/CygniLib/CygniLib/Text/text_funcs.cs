using System;
using System.Text.RegularExpressions;
using Cygni.DataTypes;
using Cygni.Errors;

namespace CygniLib.Text
{
	public static class text_funcs
	{
		public static DynValue regex(DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 1, "regex");
			return DynValue.FromUserData (new CygniLib.Text.regex (args [0].AsString ()));
		}
	}
}

