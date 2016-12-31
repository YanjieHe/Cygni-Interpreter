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
			return DynValue.Nil;
		}

		public static DynValue removeAt (DynList list, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "removeAt");
			list.RemoveAt ((int)args [0].AsNumber ());
			return DynValue.Nil;
		}

		public static DynValue insert (DynList list, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "insert");
			list.Insert (
				(int)args [0].AsNumber (),
				(int)args [1].AsNumber ());
			return DynValue.Nil;
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
			return DynValue.Nil;
		}

		public static DynValue sort (DynList list, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 0 || args.Length == 2, "sort");
			if (args.Length == 0)
				list.Sort ();
			else {
				list.Sort ((int)args [0].AsNumber (), (int)args [1].AsNumber (), DynValue.Nil);
			}
			return DynValue.Nil;
		}

		public static DynValue bSearch (DynList list, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1 || args.Length == 3, "bSearch");
			if (args.Length == 1)
				return (double)list.BinarySearch (args [0]);
			else {
				return (double)list.BinarySearch (
					(int)args [0].AsNumber (), (int)args [1].AsNumber (), args [2], DynValue.Nil);
			}
		}

		public static DynValue max (DynList list, DynValue[] args)
		{
			return list.Max ();
		}

		public static DynValue min (DynList list, DynValue[] args)
		{
			return list.Min ();
		}

		public static DynValue find (DynList list, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1 || args.Length == 2 || args.Length == 3, "find");
			if (args.Length == 1)
				return list.IndexOf (args [0]);
			else if (args.Length == 2)
				return list.IndexOf (args [0], (int)args [1].AsNumber ());
			else
				return list.IndexOf (args [0], (int)args [1].AsNumber (), (int)args [2].AsNumber ());
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

		public static DynValue slice (DynList list, Range range)
		{
			int start = range.Start;
			int end = range.End;
			int step = range.Step;
			DynList newList;
			if (step > 0) {
				if (end < start) {
					throw new RuntimeException ("function 'slice': 'end' cannot be less than 'start' when the 'step' is positive.");
				} else {

					newList = new DynList ((end - start + 1) / step);
					for (int i = start; i < end; i += step) {
						newList.Add (list [i]);
					}
				}

			} else if (step < 0) {
				if (end > start) {
					throw new RuntimeException ("function 'slice': 'end' cannot be less than 'start' when the 'step' is negative.");
				} else {
					newList = new DynList ((end - start + 1) / step);
					for (int i = start; i > end; i += step) {
						newList.Add (list [i]);
					}
				}
			} else {
				throw new RuntimeException ("'step' of slice cannot be zero");
			}
			return newList;
		}

		public static DynValue slice (DynList list, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2 || args.Length == 3, "slice");
			int start = (int)args [0].AsNumber ();
			int end = (int)args [1].AsNumber ();
			int step;
			if (args.Length == 2) {
				step = 1;
			} else {
				step = (int)args [2].AsNumber ();
			}
			DynList newList;
			if (step > 0) {
				if (end < start) {
					throw new RuntimeException ("function 'slice': 'end' cannot be less than 'start' when the 'step' is positive.");
				} else {
					
					newList = new DynList ((end - start + 1) / step);
					for (int i = start; i < end; i += step) {
						newList.Add (list [i]);
					}
				}
					
			} else if (step < 0) {
				if (end > start) {
					throw new RuntimeException ("function 'slice': 'end' cannot be less than 'start' when the 'step' is negative.");
				} else {
					newList = new DynList ((end - start + 1) / step);
					for (int i = start; i > end; i += step) {
						newList.Add (list [i]);
					}
				}
			} else {
				throw new RuntimeException ("'step' of slice cannot be zero");
			}
			return newList;
		}

		public static DynValue copy (DynList list, DynValue[] args)
		{
			DynList newList = new DynList (list, list.Count);
			return newList;
		}

	}
}
