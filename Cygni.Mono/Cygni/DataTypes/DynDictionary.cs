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
		public DynValue this [DynValue[] indexes] {
			get { 
				RuntimeException.IndexerArgsCheck (indexes.Length == 1, "Dictionary");
				var key = indexes [0];
				switch (key.type) {
				case DataType.Number:
					return this [(int)(double)key.Value];
				case DataType.Boolean:
				case DataType.String:
					return this [key.Value];
				default :
					throw new NotSupportedException ("Dictionary only takes number, boolean and string as keys.");
				}
			}
			set { 
				RuntimeException.IndexerArgsCheck (indexes.Length == 1, "Dictionary");
				var key = indexes [0];
				switch (key.type) {
				case DataType.Number:
					this [(int)(double)key.Value] = value;
					return;
				case DataType.Boolean:
				case DataType.String:
					this [key.Value] = value;
					return;
				default :
					throw new NotSupportedException ("Dictionary only takes number, boolean and string as keys.");
				}
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
				throw new NotSupportedException ("Dictionary only takes number, boolean and string as keys.");
			}
		}

		public override string ToString ()
		{
			StringBuilder s = new StringBuilder ();
			var iterator = base.GetEnumerator();
			s.Append ("{ ");
			if (iterator.MoveNext ())
				s.Append (iterator.Current);
			while (iterator.MoveNext()) {
				s.Append (',').Append (iterator.Current);
			}
			s.Append (" }");
			return s.ToString ();
		}

		public new IEnumerator<DynValue> GetEnumerator ()
		{
			var iterator = base.GetEnumerator();
			while(iterator.MoveNext()){
				var kvp = new DynList (2);
				var key = iterator.Current.Key;
				if (key is int)
					kvp.Add ((double)(int)key);
				else if (key is bool)
					kvp.Add ((bool)key);
				else if (key is string)
					kvp.Add (key as string);
				else
					throw new NotSupportedException ("Dictionary only takes number, boolean and string as keys.");
				kvp.Add (iterator.Current.Value);
				yield return DynValue.FromList(kvp);
			}
		}

		 System.Collections.IEnumerator  System.Collections.IEnumerable.GetEnumerator ()
		{
			var iterator = this.GetEnumerator();
			while (iterator.MoveNext()) 
				yield return iterator.Current;
		}
		public DynValue GetByDot (string fieldName)
		{
			switch (fieldName) {
			case "hasKey":
				return DynValue.FromDelegate ((args) => DictionaryLib.hasKey (this, args));
			case "hasValue":
				return DynValue.FromDelegate ((args) => DictionaryLib.hasValue (this, args));
			case "remove":
				return DynValue.FromDelegate ((args) => DictionaryLib.remove (this, args));
			case "count":
				return (double)this.Count;
			case "keys":
				return DynValue.FromDelegate ((args) => DictionaryLib.keys (this, args));
			case "values":
				return DynValue.FromDelegate ((args) => DictionaryLib.values (this, args));
			case "add":
				return DynValue.FromDelegate ((args) => DictionaryLib.add (this, args));
			case "clear":
				return DynValue.FromDelegate ((args) => DictionaryLib.clear (this, args));
			default:
				throw RuntimeException.FieldNotExist ("Dictionary", fieldName);
			}
		}
		public string[] FieldNames{
			get{
				return new string[] {
					"hasKey", "hasValue", "remove", "count", "keys", "values", "add", "clear"
				};}}
		public DynValue SetByDot (string fieldName, DynValue value)
		{
				throw RuntimeException.FieldNotExist ("Dictionary", fieldName);
		}

	}
}
