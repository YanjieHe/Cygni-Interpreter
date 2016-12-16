using System;
using System.Text.RegularExpressions;
using Cygni.DataTypes;
using Cygni.Errors;

namespace CygniLib.Text
{
	public static class text_funcs
	{
		public static DynValue Regex (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1 || args.Length == 2, "Regex");
			if (args.Length == 1) {
				return DynValue.FromUserData (new CygniLib.Text.regex (args [0].AsString ()));
			} else {
				string option = args [1].AsString ();
				RegexOptions op;
				if (Enum.TryParse (option, true, out op)) {
					return DynValue.FromUserData (new CygniLib.Text.regex (args [0].AsString (), op));
				} else {
					throw new RuntimeException ("Wrong argument '{0}' for constructor 'regex', cannot parse RegexOptions.", option);
				}
			}
		}
	}
}

