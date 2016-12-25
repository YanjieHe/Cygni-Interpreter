using System;
using System.IO;
using System.Text;
using Cygni.DataTypes;
using Cygni.Errors;

namespace CygniLib.IO
{
	public static class io_funcs
	{
		public static DynValue Reader(DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 2, "Reader");
			Encoding encoding = Encoding.GetEncoding (args [1].AsString ());
			if( args[0].type == DataType.String)
				return DynValue.FromUserData (new CygniReader (args[0].AsString(), encoding));
			else
				return DynValue.FromUserData (new CygniReader (args[0].As<Stream>(), encoding));
		}

		public static DynValue Writer(DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 2 || args.Length == 3, "Writer");
			if (args.Length == 2) {
				Encoding encoding = Encoding.GetEncoding (args [1].AsString ());
				return DynValue.FromUserData (new CygniWriter (args[0].As<Stream>(), encoding));
			} else {
				bool append = args [1].AsBoolean ();
				Encoding encoding = Encoding.GetEncoding (args [2].AsString ());
				return DynValue.FromUserData (new CygniWriter (args[0].AsString(),append, encoding));
			}
		}

	}
}
