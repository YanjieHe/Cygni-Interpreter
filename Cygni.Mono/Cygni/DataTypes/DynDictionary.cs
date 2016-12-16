using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.Errors;
using Cygni.Libraries;

namespace Cygni.DataTypes
{
	/// <summary>
	/// Description of DynDictionary.
	/// </summary>
	public sealed class DynDictionary: Dictionary<object,DynValue> ,IEnumerable<DynValue>, IIndexable,IDot
	{

		public DynValue GetByIndex (DynValue index)
		{
			switch (index.type) {
				case DataType.Number:
					return base [(int)(double)index.Value];
				case DataType.Boolean:
				case DataType.String:
					return base [index.Value];
				default:
					throw new RuntimeException ("Dictionary only takes number, boolean and string as keys.");
			}
		}

		public DynValue SetByIndex (DynValue index, DynValue value)
		{
			switch (index.type) {
				case DataType.Number:
					return base [(int)(double)index.Value] = value;
				case DataType.Boolean:
				case DataType.String:
					return base [index.Value] = value;
				default:
					throw new RuntimeException ("Dictionary only takes number, boolean and string as keys.");
			}

		}

		public DynValue GetByIndexes (DynValue[] indexes)
		{
			RuntimeException.IndexerArgsCheck (indexes.Length == 1, "Dictionary");
			var key = indexes [0];
			switch (key.type) {
				case DataType.Number:
					return base [(int)(double)key.Value];
				case DataType.Boolean:
				case DataType.String:
					return base [key.Value];
				default :
					throw new RuntimeException ("Dictionary only takes number, boolean and string as keys.");
			}

		}

		public DynValue SetByIndexes (DynValue[] indexes, DynValue value)
		{
			RuntimeException.IndexerArgsCheck (indexes.Length == 1, "Dictionary");
			var key = indexes [0];
			switch (key.type) {
				case DataType.Number:
					return base [(int)(double)key.Value] = value;
				case DataType.Boolean:
				case DataType.String:
					return base [key.Value] = value;
				default :
					throw new RuntimeException ("Dictionary only takes number, boolean and string as keys.");
			}
		}

		public void Add (DynValue key, DynValue value)
		{
			switch (key.type) {
				case DataType.Number:
					base.Add ((int)(double)key.Value, value);
					return;
				case DataType.Boolean:
				case DataType.String:
					base.Add (key.Value, value);
					return;
				default :
					throw new RuntimeException ("Dictionary only takes number, boolean and string as keys.");
			}
		}

		public override string ToString ()
		{
			StringBuilder s = new StringBuilder ();
			var iterator = base.GetEnumerator ();
			s.Append ("{ ");
			if (iterator.MoveNext ()) {
				s.Append (DynValue.FromObject(iterator.Current.Key))
					.Append (": ")
					.Append (iterator.Current.Value);
			}
			while (iterator.MoveNext ()) {
				s.Append (", ")
					.Append (DynValue.FromObject(iterator.Current.Key))
					.Append (": ")
					.Append (iterator.Current.Value);
			}
			s.Append (" }");
			return s.ToString ();
		}

		public new IEnumerator<DynValue> GetEnumerator ()
		{
			var iterator = base.GetEnumerator ();
			while (iterator.MoveNext ()) {
				var kvp = new StructureItem [2];
				object key = iterator.Current.Key;
				IConvertible iconv = key as IConvertible;
				if (iconv == null) {
					throw new RuntimeException ("Dictionary only takes number, boolean and string as keys.");
				} else {
					switch (iconv.GetTypeCode ()) {
						case TypeCode.Int32:
							kvp [0] = new StructureItem ("key", (double)(int)key);
							break;
						case TypeCode.Boolean:
							kvp [0] = new StructureItem ("key", (bool)key);
							break;
						case TypeCode.String:
							kvp [0] = new StructureItem ("key", key as string);
							break;
						default:
							throw new RuntimeException ("Dictionary only takes number, boolean and string as keys.");
					}
					kvp [1] = new StructureItem ("value", iterator.Current.Value);
				}
				yield return DynValue.FromStructure (new Structure (kvp));
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			var iterator = this.GetEnumerator ();
			while (iterator.MoveNext ())
				yield return iterator.Current;
		}

		public DynValue GetByDot (string fieldName)
		{
			switch (fieldName) {
				case "hasKey":
					return DynValue.FromDelegate ("hasKey",(args) => DictionaryLib.hasKey (this, args));
				case "hasValue":
				return DynValue.FromDelegate ("hasValue",(args) => DictionaryLib.hasValue (this, args));
				case "remove":
				return DynValue.FromDelegate ("remove",(args) => DictionaryLib.remove (this, args));
				case "count":
					return (double)this.Count;
				case "keys":
				return DynValue.FromDelegate ("keys",(args) => DictionaryLib.keys (this, args));
				case "values":
				return DynValue.FromDelegate ("values",(args) => DictionaryLib.values (this, args));
				case "add":
				return DynValue.FromDelegate ("add",(args) => DictionaryLib.add (this, args));
				case "clear":
				return DynValue.FromDelegate ("clear",(args) => DictionaryLib.clear (this, args));
				default:
					throw RuntimeException.FieldNotExist ("Dictionary", fieldName);
			}
		}

		public string[] FieldNames {
			get {
				return new string[] {
					"hasKey", "hasValue", "remove", "count", "keys", "values", "add", "clear"
				};
			}
		}

		public DynValue SetByDot (string fieldName, DynValue value)
		{
			throw RuntimeException.FieldNotExist ("Dictionary", fieldName);
		}

	}
}
