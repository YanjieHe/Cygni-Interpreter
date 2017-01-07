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
		public static DynValue append (DynList list, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "append");
			list.Add (args [0]);
			return DynValue.Void;
		}

		public static DynValue removeAt (DynList list, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "removeAt");
			list.RemoveAt (args [0].AsInt32 ());
			return DynValue.Void;
		}

		public static DynValue insert (DynList list, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "insert");
			list.Insert (index: args [1].AsInt32 (), item: args [0]);
			return DynValue.Void;
		}

		public static DynValue pop (DynList list, DynValue[] args)
		{
			int n = list.Count - 1;
			var last = list [n];
			list.RemoveAt (n);
			return last;
		}

		public static DynValue clear (DynList list, DynValue[] args)
		{
			list.Clear ();
			return DynValue.Void;
		}

		public static DynValue sort (DynList list, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 0 || args.Length == 2, "sort");
			if (args.Length == 0)
				list.Sort ();
			else {
				list.Sort (args [0].AsInt32 (), args [1].AsInt32 (), DynValue.Nil);
			}
			return DynValue.Void;
		}

		public static DynValue bSearch (DynList list, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1 || args.Length == 3, "bSearch");
			if (args.Length == 1)
				return list.BinarySearch (args [0]);
			else {
				return list.BinarySearch (
					index: args [1].AsInt32 (), 
					count: args [2].AsInt32 (), 
					item: args [0], 
					comparer: DynValue.Nil);
			}
		}

		public static DynValue max (DynList list, DynValue[] args)
		{
			return list.Max<DynValue> ();
		}

		public static DynValue min (DynList list, DynValue[] args)
		{
			return list.Min<DynValue> ();
		}

		public static DynValue find (DynList list, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1 || args.Length == 2 || args.Length == 3, "find");
			if (args.Length == 1)
				return list.IndexOf (args [0]);
			else if (args.Length == 2)
				return list.IndexOf (args [0], args [1].AsInt32 ());
			else
				return list.IndexOf (args [0], args [1].AsInt32 (), args [2].AsInt32 ());
		}

		public static DynValue concat (DynList list, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "concat");
			DynList list2 = args [0].As<DynList> ();
			DynList newList = new DynList (list.Count + list2.Count);
			foreach (DynValue item in list.Concat(list2)) {
				newList.Add (item);
			}
			return newList;
		}

		public static DynValue Slice (DynList list, Range range)
		{
			int start = range.Start;
			int end = range.End;
			int step = range.Step;
			RuntimeException.SliceCheck (list.Count, range);
			DynList newList = new DynList ((end - start + 1) / step);
			if (range.IsForward) {
				for (int i = start; i < end; i += step) {
					newList.Add (list [i]);
				}
			} else {
				for (int i = start; i > end; i += step) {
					newList.Add (list [i]);
				}
			}
			return newList;
		}

		public static DynValue SliceAssign (DynList list, Range range, DynValue value)
		{
			int start = range.Start;
			int end = range.End;
			int step = range.Step;
			RuntimeException.SliceCheck (list.Count, range);
			if (range.IsForward) {
				for (int i = start; i < end; i += step) {
					list [i] = value;
				}
			} else {
				for (int i = start; i > end; i += step) {
					list [i] = value;
				}
			}
			return value;
		}

		public static DynValue copy (DynList list, DynValue[] args)
		{
			DynList newList = new DynList (list, list.Count);
			return newList;
		}

	}
}
