using System;
using Cygni.DataTypes;
using Cygni.Errors;
using System.Collections.Generic;
using System.Linq;
using Cygni.DataTypes.Interfaces;

namespace CygniLib.Collections
{
	public class array:IIndexable,IDot
	{
		DynValue[] values;

		public array (int size)
		{
			values = new DynValue[size];
			for (int i = 0; i < size; i++) {
				values [i] = DynValue.Nil;
			}
		}

		public array (DynList list)
		{
			values = new DynValue[list.Count];
			for (int i = 0; i < values.Length; i++) {
				values [i] = list [i];
			}
		}

		public DynValue GetByIndex (DynValue index)
		{
			return values [(int)index.AsNumber ()];
		}

		public DynValue SetByIndex (DynValue index, DynValue value)
		{
			return values [(int)index.AsNumber ()] = value;
		}

		public DynValue GetByIndexes (DynValue[] indexes)
		{
			RuntimeException.IndexerArgsCheck (indexes.Length == 1, "array");
			return values [(int)indexes [0].AsNumber ()];
		}

		public DynValue SetByIndexes (DynValue[] indexes, DynValue value)
		{
			RuntimeException.IndexerArgsCheck (indexes.Length == 1, "array");
			return values [(int)indexes [0].AsNumber ()] = value;
		}

		public DynValue GetByDot (string fieldName)
		{
			switch (fieldName) {
			case "length":
				return (double)values.Length;
			default:
				throw RuntimeException.FieldNotExist ("array", fieldName);
			}
		}

		public string[] FieldNames{ get { return new string[] {
				"length" 
			}; } }

		public	DynValue SetByDot (string fieldName, DynValue value)
		{
			throw RuntimeException.FieldNotExist ("Array", fieldName);
		}

		public override string ToString ()
		{
			return string.Concat ("Array([", string.Join (", ", values.Select (i => i.Value)), "])");
		}
	}
}

