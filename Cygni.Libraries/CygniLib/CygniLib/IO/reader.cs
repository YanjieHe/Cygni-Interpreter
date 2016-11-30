using System;
using System.IO;
using System.Text;
using Cygni.DataTypes;
using Cygni.Errors;

namespace CygniLib.IO
{
	public class reader: StreamReader,IDot
	{
		public reader(Stream s, Encoding encoding): base(s,encoding) {

		}
		public reader(string filePath, Encoding encoding):base(filePath,encoding){

		}
		public DynValue GetByDot (string fieldName)
		{
			switch (fieldName) {
				case "peek":
					return DynValue.FromDelegate (args => (double)this.Peek ());
				case "read":
					return DynValue.FromDelegate (args => (double)this.Read ());
				case "readLine":
					return DynValue.FromDelegate (args => this.ReadLine ());
				case "close":
					return DynValue.FromDelegate (args => {
							this.Close ();
							return DynValue.Null;
							});
				default :
					throw RuntimeException.FieldNotExist ("reader", fieldName);
			}
		}
		public string[] FieldNames{
			get{ 
				return new string[] {
					"peek", "read", "readLine", "close"
				};
			}
		}
		public DynValue SetByDot (string fieldName, DynValue value)
		{
			throw RuntimeException.FieldNotExist ("reader", fieldName);
		}
		public override string ToString () {
			return "Native Class: reader";
		}
	}
}
