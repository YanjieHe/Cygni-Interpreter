﻿using System;
using System.IO;
using System.Text;
using Cygni.DataTypes;
using Cygni.Errors;
namespace CygniLib.IO
{
	public class writer: StreamWriter,IDot
	{
		public writer(Stream s, Encoding encoding): base(s,encoding) {

		}
		public writer(string filePath,bool append, Encoding encoding):base(filePath,append, encoding){
		}
		public DynValue GetByDot (string fieldname)
		{
			switch (fieldname) {

			case "write":
				return DynValue.FromDelegate (args => 
					{
						RuntimeException.FuncArgsCheck(args.Length == 1,"write");
						this.Write(args[0].Value);
						return DynValue.Null;
					}
				);
			case "writeLine":
				return DynValue.FromDelegate (args => {
					RuntimeException.FuncArgsCheck (args.Length == 1, "writeLine");
					this.WriteLine (args [0].Value);
					return DynValue.Null;
				}
				);
			case "close":
				return DynValue.FromDelegate (args => {
					this.Close ();
					return DynValue.Null;
				});
			default :
				throw  RuntimeException.NotDefined (fieldname);
			}
		}

		public DynValue SetByDot (string fieldname, DynValue value)
		{
			throw RuntimeException.NotDefined (fieldname);
		}
	}
}
