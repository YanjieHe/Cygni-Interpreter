using System;
using Cygni.DataTypes;
using Cygni.Errors;
using System.Collections.Generic;
namespace CygniLib.Collections
{
	public static class collections_funcs
	{
		public static DynValue Array(DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 1, "Array");
			if (args[0].type ==DataType.Number) {
				return DynValue.FromUserData (new CygniLib.Collections.array ((int)args [0].AsNumber ()));
			} else if (args[0].type == DataType.List) {
				return DynValue.FromUserData (new CygniLib.Collections.array (args [0].As<DynList> ()));
			} else {
				throw new RuntimeException ("Wrong argument for construct array");
			}
		}
		public static DynValue Array2(DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 2, "Array2");
			return DynValue.FromUserData (new CygniLib.Collections.array2 (
				(int)args [0].AsNumber (),(int)args[1].AsNumber()
			)
			);
		}
		public static DynValue Stack(DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 0, "Stack");
			return DynValue.FromUserData (new CygniLib.Collections.stack ());
		}
		public static DynValue Queue(DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 0, "Queue");
			return DynValue.FromUserData (new CygniLib.Collections.queue ());
		}
		public static DynValue LinkedList(DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 0, "LinkedList");
			return DynValue.FromUserData (new CygniLib.Collections.linkedList ());
		}
	}
}

