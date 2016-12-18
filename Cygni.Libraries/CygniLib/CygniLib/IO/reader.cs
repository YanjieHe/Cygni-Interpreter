using System;
using System.IO;
using System.Text;
using Cygni.DataTypes;
using Cygni.Errors;
using Cygni.DataTypes.Interfaces;

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
					return DynValue.FromDelegate ("peek",args => (double)this.Peek ());
				case "read":
				return DynValue.FromDelegate ("read",args => (double)this.Read ());
				case "readLine":
				return DynValue.FromDelegate ("readLine",args => this.ReadLine ());
				case "close":
				return DynValue.FromDelegate ("close",args => {
							this.Close ();
							return DynValue.Nil;
							});
				default :
					throw RuntimeException.FieldNotExist ("Reader", fieldName);
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
			throw RuntimeException.FieldNotExist ("Reader", fieldName);
		}
		public override string ToString () {
			return "Native Class: Reader";
		}
	}
}
