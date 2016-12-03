using System;
using Cygni.DataTypes;
using Cygni.Errors;
using System.Collections.Generic;
namespace CygniLib.Collections
{
	public static class collections_funcs
	{
		public static DynValue array(DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 1, "array");
			return DynValue.FromUserData (new CygniLib.Collections.array ((int)args [0].AsNumber ()));
		}
		public static DynValue array2(DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 2, "array2");
			return DynValue.FromUserData (new CygniLib.Collections.array2 (
				(int)args [0].AsNumber (),(int)args[1].AsNumber()
			)
			);
		}
		public static DynValue stack(DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 0, "stack");
			return DynValue.FromUserData (new CygniLib.Collections.stack ());
		}
		public static DynValue queue(DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 0, "queue");
			return DynValue.FromUserData (new CygniLib.Collections.queue ());
		}
		public static DynValue linkedList(DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 0, "linkedList");
			return DynValue.FromUserData (new CygniLib.Collections.linkedList ());
		}
	}
}

