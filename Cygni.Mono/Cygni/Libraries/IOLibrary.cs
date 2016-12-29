using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.AST;
using Cygni.DataTypes;
using Cygni.Errors;
using System.IO;

namespace Cygni
{
	public static class IOLibrary
	{
		public static DynValue readLines (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1 || args.Length == 2, "readLines");
			string path = args [0].AsString ();
			Encoding encoding;
			if (args.Length == 2) {
				encoding = Encoding.GetEncoding (args [1].AsString ());
			} else {
				encoding = Encoding.UTF8;
			}
			return DynValue.FromUserData (File.ReadLines (path, encoding).Select (DynValue.FromString));
		}

		public static DynValue writeLines (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (
				args.Length == 1 || args.Length == 2 || args.Length == 3, "writeLines");
			string path = args [0].AsString ();
			IEnumerable<DynValue> contents = args [1].As<IEnumerable<DynValue>> ();
			Encoding encoding;
			if (args.Length == 3) {
				encoding = Encoding.GetEncoding (args [2].AsString ());
			} else {
				encoding = Encoding.UTF8;
			}
			File.WriteAllLines (path, contents.Select (i => i.AsString ()), encoding);
			return DynValue.Nil;
		}

		public static DynValue exists (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "exists");
			string path = args [0].AsString ();
			return File.Exists (path);
		}

		public static DynValue getCurrentDir (DynValue[] args)
		{
			return Directory.GetCurrentDirectory ();
		}

		public static DynValue setCurrentDir (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "setCurrentDir");
			string path = args [0].AsString ();
			Directory.SetCurrentDirectory (path);
			return DynValue.Nil;
		}

	}
}

