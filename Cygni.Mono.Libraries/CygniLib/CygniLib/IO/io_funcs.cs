using System;
using System.IO;
using System.Text;
using Cygni.DataTypes;
using Cygni.Errors;

namespace CygniLib.IO
{
	public static class io_funcs
	{
		public static DynValue Reader (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "Reader");
			Encoding encoding = Encoding.GetEncoding (args [1].AsString ());
			if (args [0].IsString)
				return DynValue.FromUserData (new CygniReader (args [0].AsString (), encoding));
			else
				return DynValue.FromUserData (new CygniReader (args [0].As<Stream> (), encoding));
		}

		public static DynValue Writer (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2 || args.Length == 3, "Writer");
			if (args [0].IsString) {
				string filePath = args [0].AsString ();
				Encoding encoding = Encoding.GetEncoding (args [1].AsString ());
				bool append;
				if (args.Length == 2) {
					append = false;
				} else {
					append = args [2].AsBoolean ();
				}
				return DynValue.FromUserData (new CygniWriter (filePath, append, encoding));
			} else {
				RuntimeException.FuncArgsCheck (args.Length == 2, "Writer");
				Encoding encoding = Encoding.GetEncoding (args [1].AsString ());
				return DynValue.FromUserData (new CygniWriter (args [0].As<Stream> (), encoding));
			}
		}

	}
}
