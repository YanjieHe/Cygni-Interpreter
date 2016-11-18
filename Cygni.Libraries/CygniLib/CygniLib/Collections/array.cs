using System;
using Cygni.DataTypes;
using Cygni.Errors;
using System.Collections.Generic;
using System.Linq;
namespace CygniLib.Collections
{
	public class array:IIndexable,IDot
	{
		DynValue[] values;
		public array (int size)
		{
			values = new DynValue[size];
			for (int i = 0; i < size; i++) {
				values [i] = DynValue.Null;
			}
		}
		public DynValue this [DynValue[] indexes] {
			get {
				RuntimeException.IndexerArgsCheck (indexes.Length == 1, "array");
				return values [(int)indexes [0].AsNumber ()];
			}
			set {
				RuntimeException.IndexerArgsCheck (indexes.Length == 1, "array");
				values [(int)indexes [0].AsNumber ()] = value;
			}
		}
		public DynValue GetByDot (string fieldname){
			switch (fieldname) {
			case "length":
				return (double)values.Length;
			default:
				throw RuntimeException.NotDefined (fieldname);
			}
		}
		public string[] FieldNames{get{ return new string[] {
				"length" 
			};
			}
		}

		public	DynValue SetByDot (string fieldname, DynValue value){
			throw RuntimeException.NotDefined (fieldname);
		}
		public override string ToString ()
		{
			return string.Concat ("array([", string.Join (", ", values.Select (i => i.Value)), "])");
		}
	}
}

