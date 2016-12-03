using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Errors;
using Cygni.Extensions;

namespace Cygni.Libraries
{
	/// <summary>
	/// Description of DictionaryLib.
	/// </summary>
	public static class DictionaryLib
	{
		public static DynValue hasKey (DynDictionary ht, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "hasKey");
			var key = args [0];
			switch (key.type) {
			case DataType.Number:
				return ht.ContainsKey ((int)(double)key.Value);
			case DataType.Boolean:
			case DataType.String:
				return ht.ContainsKey (key.Value);
			default:
				throw new NotSupportedException ("Dictionary only takes number, boolean and string as keys.");
			}
		}

		public static DynValue hasValue (DynDictionary ht, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "hasValue");
			var value = args [0];
			return ht.ContainsValue (value);
		}

		public static DynValue remove (DynDictionary ht, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "hasValue");
			var key = args [0];
			switch (key.type) {
			case DataType.Number:
				return ht.Remove ((int)(double)key.Value);
			case DataType.Boolean:
			case DataType.String:
				return ht.Remove (key.Value);
			default:
				throw new NotSupportedException ("Dictionary only takes number, boolean and string as keys.");
			}
		}

		public static DynValue keys (DynDictionary ht, DynValue[] args)
		{
			int n = ht.Count;
			DynList keys = new DynList(n);
			foreach (var key in ht.Keys){
				IConvertible iconv = key as IConvertible;
				if (iconv == null) {
					throw new RuntimeException ("Dictionary only takes number, boolean and string as keys.");
				} else {
					switch (iconv.GetTypeCode ()) {
					case TypeCode.Int32:
						keys.Add((double)(int)key);
						break;
					case TypeCode.Boolean:
						keys.Add((bool)key);
						break;
					case TypeCode.String:
						keys.Add(key as string);
						break;
					default:
						throw new RuntimeException ("Dictionary only takes number, boolean and string as keys.");
					}
				}
			}
			return keys;
		}

		public static DynValue values (DynDictionary ht, DynValue[] args)
		{
			int n = ht.Count;
			DynList values = new DynList(n);
			foreach (var v in ht.Values){
				values.Add(v);
			}
			return values;
		}
		public static DynValue add (DynDictionary ht, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "add");
			var key = args [0];
			var value = args [1];
			switch (key.type) {
			case DataType.Number:
				ht.Add ((int)(double)key.Value, value);
				break;
			case DataType.Boolean:
			case DataType.String:
				ht.Add (key.Value, value);
				break;
			default:
				throw new NotSupportedException ("Dictionary only takes number, boolean and string as keys.");
			}
			return DynValue.Null;
		}
		public static DynValue clear (DynDictionary ht, DynValue[] args)
		{
			ht.Clear ();
			return DynValue.Null;
		}
	}
}
