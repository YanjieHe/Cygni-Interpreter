using System;
using System.IO;
using System.Text;
using Cygni.DataTypes;
using Cygni.DataTypes.Interfaces;
using Cygni.Errors;
namespace CygniLib.IO
{
	public class writer: StreamWriter,IDot
	{
		public writer(Stream s, Encoding encoding): base(s,encoding) {

		}
		public writer(string filePath,bool append, Encoding encoding):base(filePath,append, encoding){
		}
		public DynValue GetByDot (string fieldName)
		{
			switch (fieldName) {

				case "write":
				return DynValue.FromDelegate ("write",
							args => 
							{
							RuntimeException.FuncArgsCheck(args.Length == 1,"write");
							this.Write(args[0].Value);
							return DynValue.Nil;
							}
							);
				case "writeLine":
				return DynValue.FromDelegate ("writeLine",
							args => {
							RuntimeException.FuncArgsCheck (args.Length == 1, "writeLine");
							this.WriteLine (args [0].Value);
							return DynValue.Nil;
							}
							);
				case "close":
				return DynValue.FromDelegate ("close",
							args => {
							this.Close ();
							return DynValue.Nil;
							});
				default :
					throw RuntimeException.FieldNotExist ("Writer", fieldName);
			}
		}

		public string[] FieldNames{
			get{ 
				return new string[] {
					"write", "writeLine", "close"
				};
			}
		}
		public DynValue SetByDot (string fieldName, DynValue value)
		{
			throw RuntimeException.FieldNotExist ("Writer", fieldName);
		}
		public override string ToString () {
			return "Native Class: Writer";
		}
	}
}
