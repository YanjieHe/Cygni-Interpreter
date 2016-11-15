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
		public static DynValue append(DynList list, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "append");
			list.Add(args[0]);
			return DynValue.Null;
		}
		public static DynValue removeAt(DynList list, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "removeAt");
			list.RemoveAt((int)args[0].AsNumber());
			return DynValue.Null;
		}
		public static DynValue insert(DynList list, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "insert");
			list.Insert(
				(int)args[0].AsNumber(),
				(int)args[1].AsNumber());
			return DynValue.Null;
		}
		public static DynValue pop(DynList list, DynValue[] args){
			int n = list.Count - 1;
			var last = list[n];
			list.RemoveAt(n);
			return last;
		}
		public static DynValue clear(DynList list, DynValue[] args){
			list.Clear();
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
	}
}
