using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.Errors;
using Cygni.DataTypes.Interfaces;

namespace Cygni.DataTypes
{
	public sealed class Structure:IDot  
	{
		readonly StructureItem[] contents;

		public Structure (params StructureItem[] contents)
		{
			this.contents = contents;
		}

		public DynValue GetByDot (string fieldName)
		{
			foreach (var item in contents) {
				if(string.Equals(item.Key, fieldName))
					return item.Value;
			}
			throw RuntimeException.FieldNotExist ("struct", fieldName);
		}

		public DynValue SetByDot (string fieldName, DynValue value)
		{
			foreach (var item in contents) {
				if(string.Equals(item.Key, fieldName))
					return item.Value = value;
			}
			throw RuntimeException.FieldNotExist ("struct", fieldName);
		}

		public string[] FieldNames {
			get {
				string[] names = new string[contents.Length];
				for (int i = 0; i < names.Length; i++)
					names [i] = contents [i].Key;
				return names;
			}
		}

		public override string ToString ()
		{
			var s = new StringBuilder ("struct: {\r\n");
			foreach (var item in contents) {
				s
					.Append ('\t')
					.Append (item.Key)
					.Append (": ")
					.AppendLine (item.Value.ToString ());
			}
			s.Append ('}');
			return s.ToString ();
		}
	}
}
