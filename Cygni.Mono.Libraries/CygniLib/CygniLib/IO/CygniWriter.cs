using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Cygni.DataTypes;
using Cygni.DataTypes.Interfaces;
using Cygni.Errors;

namespace CygniLib.IO
{
	public class CygniWriter: StreamWriter,IDot
	{
		public CygniWriter (Stream s, Encoding encoding) : base (s, encoding)
		{

		}

		public CygniWriter (string filePath, bool append, Encoding encoding) : base (filePath, append, encoding)
		{
		}

		public DynValue GetByDot (string fieldName)
		{
			switch (fieldName) {

			case "write":
				return DynValue.FromDelegate ("write",
					args => {
						RuntimeException.FuncArgsCheck (args.Length == 1, "write");
						this.Write (args [0].Value);
						return DynValue.Void;
					}
				);
			case "writeLine":
				return DynValue.FromDelegate ("writeLine",
					args => {
						RuntimeException.FuncArgsCheck (args.Length == 1, "writeLine");
						this.WriteLine (args [0].Value);
						return DynValue.Void;
					}
				);
			case "writeLines":
				return DynValue.FromDelegate ("writeLines",
					args => {
						RuntimeException.FuncArgsCheck (args.Length == 1, "writeLines");
						this.WriteLines (args [0].As <IEnumerable<DynValue>> ().Select (i => i.AsString ()));
						return DynValue.Void;
					}
				);
			case "close":
				return DynValue.FromDelegate ("close",
					args => {
						this.Close ();
						return DynValue.Void;
					});
			default :
				throw RuntimeException.FieldNotExist ("Writer", fieldName);
			}
		}

		public void WriteLines (IEnumerable<string> content)
		{
			foreach (string line in content) {
				this.WriteLine (line);
			}
		}

		public string[] FieldNames {
			get { 
				return new string[] {
					"write", "writeLine", "writeLines", "close"
				};
			}
		}

		public DynValue SetByDot (string fieldName, DynValue value)
		{
			throw RuntimeException.FieldNotExist ("Writer", fieldName);
		}

		public override string ToString ()
		{
			return "Native Class: Writer";
		}
	}
}
