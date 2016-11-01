using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.Errors;
using Cygni.Settings;
using System.Collections;
namespace Cygni.Libraries
{
	/// <summary>
	/// Description of ListLib.
	/// </summary>
	public static class ListLib
	{
		public static DynValue append(DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length >= 2, "append");
			args[0].As<DynList>().AddRange(args.Skip(1));
			return DynValue.Null;
		}
		public static DynValue len(DynValue[] args)
		{
			return (double)args[0].As<ICollection>().Count;
		}
		public static DynValue removeAt(DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "removeAt");
			args[0].As<DynList>().RemoveAt((int)args[1].AsNumber());
			return DynValue.Null;
		}
		public static DynValue insertAt(DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 3, "insertAt");
			args[0].As<DynList>().Insert((int)args[1].AsNumber(),args[2]);
			return DynValue.Null;
		}
		public static DynValue sort(DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 1, "sort");
			args [0].As <DynList> ().Sort ();
			return DynValue.Null;
		}
		public static DynValue bSearch(DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 2, "bSearch");
			return (double)args [0].As <DynList> ().BinarySearch (args[1]);
		}
		public static DynValue list_max(DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 1, "max");
			return args [0].As<DynList> ().Max ();
		}
		public static DynValue list_min(DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 1, "min");
			return args [0].As<DynList> ().Min ();
		}
		public static DynValue list_pop(DynValue [] args){
			RuntimeException.FuncArgsCheck (args.Length == 1, "pop");
			var list = args [0].As<DynList> ();
			var last = list [list.Count - 1];
			list.RemoveAt (list.Count - 1);
			return last;
		}
	}
}
