using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.Errors;
namespace Cygni.Libraries
{
	/// <summary>
	/// Description of StrLib.
	/// </summary>
	public static class StrLib
	{
		public static DynValue strcat(DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length > 0, "strcat");
			return string.Concat(args.Map(i => i.AsString()));
		}
		public static DynValue strjoin(DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length > 1, "strjoin");
			return string.Join(args[0].AsString(), args.SkipMap(1, i => i.AsString()));
		}
		public static DynValue strformat(DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length > 1, "strformat");
			return string.Format(args[0].AsString(), args.SkipMap(1, i => i.Value));
		}
		public static DynValue strlen(DynValue[] args)
		{
			return (double)args[0].AsString().Length;
		}
		public static DynValue strsplit(DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length > 1, "strsplit");
			return DynValue.FromList(args[0].AsString()
			                         .Split(args.SkipMap(1, i => char.Parse(i.AsString())))
			                         .ToDynList(DynValue.FromString));
		}
		public static DynValue strrpl(DynValue[] args){/* string replace */
			RuntimeException.FuncArgsCheck (args.Length == 3, "strrpl");
			return args [0].AsString ().Replace (
				args [1].AsString (),
				args [2].AsString ());
		}
		public static DynValue strcmp(DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "strcmp");
			return (double)string.Compare (args [0].AsString (),
				args [1].AsString ());
		}
		public static DynValue strfind(DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 2, "strcmp");
			return args [0].AsString ().IndexOf (args [1].AsString ());
		}
	}
}
