using System;
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
		public DynValue GetByDot (string fieldName)
		{
			switch (fieldName) {

				case "write":
					return DynValue.FromDelegate (
							args => 
							{
							RuntimeException.FuncArgsCheck(args.Length == 1,"write");
							this.Write(args[0].Value);
							return DynValue.Null;
							}
							);
				case "writeLine":
					return DynValue.FromDelegate (
							args => {
							RuntimeException.FuncArgsCheck (args.Length == 1, "writeLine");
							this.WriteLine (args [0].Value);
							return DynValue.Null;
							}
							);
				case "close":
					return DynValue.FromDelegate (
							args => {
							this.Close ();
							return DynValue.Null;
							});
				default :
					throw RuntimeException.FieldNotExist ("writer", fieldName);
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
			throw RuntimeException.FieldNotExist ("writer", fieldName);
		}
		public override string ToString () {
			return "Native Class: writer";
		}
	}
}
