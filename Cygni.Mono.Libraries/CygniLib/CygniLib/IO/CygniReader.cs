using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cygni.DataTypes;
using Cygni.Errors;
using Cygni.DataTypes.Interfaces;

namespace CygniLib.IO
{
	public class CygniReader: StreamReader,IDot
	{
		public CygniReader (Stream s, Encoding encoding) : base (s, encoding)
		{

		}

		public CygniReader (string filePath, Encoding encoding) : base (filePath, encoding)
		{
		}

		public DynValue GetByDot (string fieldName)
		{
			switch (fieldName) {
			case "peek":
				return DynValue.FromDelegate ("peek", args => (double)this.Peek ());
			case "read":
				return DynValue.FromDelegate ("read", args => (double)this.Read ());
			case "readLine":
				return DynValue.FromDelegate ("readLine", args => this.ReadLine ());
			case "readLines":
				return DynValue.FromDelegate ("readLines",
					args => DynValue.FromUserData (this.ReadLines ().Select (DynValue.FromString)));
			case "close":
				return DynValue.FromDelegate ("close", args => {
					this.Close ();
					return DynValue.Void;
				});
			default :
				throw RuntimeException.FieldNotExist ("Reader", fieldName);
			}
		}

		public IEnumerable<string> ReadLines ()
		{
			while (this.Peek () != -1) {
				yield return this.ReadLine ();
			}
		}

		public string[] FieldNames {
			get { 
				return new string[] {
					"peek", "read", "readLine", "readLines", "close"
				};
			}
		}

		public DynValue SetByDot (string fieldName, DynValue value)
		{
			throw RuntimeException.FieldNotExist ("Reader", fieldName);
		}

		public override string ToString ()
		{
			return "Native Class: Reader";
		}
	}
}
