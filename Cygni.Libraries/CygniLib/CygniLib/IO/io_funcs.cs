using System;
using System.IO;
using System.Text;
using Cygni.DataTypes;
using Cygni.Errors;

namespace CygniLib.IO
{
	public static class io_funcs
	{
		public static DynValue reader(DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 2, "reader");
			Encoding encoding = Encoding.GetEncoding (args [1].AsString ());
			if( args[0].type == DataType.String)
				return DynValue.FromUserData (new CygniLib.IO.reader (args[0].AsString(), encoding));
			else
				return DynValue.FromUserData (new CygniLib.IO.reader (args[0].As<Stream>(), encoding));
		}

		public static DynValue writer(DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 2 || args.Length == 3, "writer");
			if (args.Length == 2) {
				Encoding encoding = Encoding.GetEncoding (args [1].AsString ());
				return DynValue.FromUserData (new CygniLib.IO.reader (args[0].As<Stream>(), encoding));
			} else {
				bool append = args [1].AsBoolean ();
				Encoding encoding = Encoding.GetEncoding (args [2].AsString ());
				return DynValue.FromUserData (new CygniLib.IO.writer (args[0].AsString(),append, encoding));
			}
		}

	}
}
