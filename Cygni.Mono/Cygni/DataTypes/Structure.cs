using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.Errors;

namespace Cygni.DataTypes
{
	public sealed class Structure:IDot  // ,IIndexable
	{
		readonly StructureItem[] contents;

		public Structure (int size)
		{
			contents = new StructureItem[size];
		}
		internal Structure(params StructureItem[] contents){
			this.contents = contents;
		}

		public void SetAt (int i, string key, DynValue value)
		{
			contents [i] = new StructureItem (key, value);
		}

		public DynValue GetByDot (string fieldName)
		{
			for (int i = contents.Length - 1; i >= 0; i--) {
				if (string.Equals (contents [i].Key, fieldName))
					return contents [i].Value;
			}
			throw RuntimeException.NotDefined (fieldName);
		}

		public DynValue SetByDot (string fieldName, DynValue value)
		{
			for (int i = contents.Length - 1; i >= 0; i--) {
				if (string.Equals (contents [i].Key, fieldName))
					return contents [i].Value = value;
			}
			throw RuntimeException.NotDefined (fieldName);
		}
		public string[] FieldNames{
			get{
				string[] names = new string[contents.Length];
				for (int i = 0; i < names.Length; i++)
					names [i] = contents [i].Key;
				return names;
				}}


		// public DynValue this [DynValue[] indexes]{ 
		// 	get { return contents [(int)indexes [0].AsNumber ()].Value; } 
		// 	set { contents [(int)indexes [0].AsNumber ()].Value = value; } }

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
