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
	/// Description of HashTableLib.
	/// </summary>
	public static class HashTableLib
	{
		public static DynValue hashtable (DynValue[] args)
		{
			var hashTable = new DynHashTable ();
			if (args.Length == 0)
				return DynValue.FromHashTable (hashTable);
			if ((args.Length & 1) == 0) {/* even */
				for (int i = 0; i < args.Length - 1; i += 2)
					hashTable.Add (args [i], args [i + 1]);
				return DynValue.FromHashTable (hashTable);
			}
			throw RuntimeException.BadArgsNum ("hashtable", "even");
		}

		public static DynValue hasKey (DynHashTable ht, DynValue[] args)
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
				throw new NotSupportedException ("HashTable only takes number, boolean and string as keys.");
			}
		}

		public static DynValue hasValue (DynHashTable ht, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "hasValue");
			var value = args [0];
			return ht.ContainsValue (value);
		}

		public static DynValue remove (DynHashTable ht, DynValue[] args)
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
				throw new NotSupportedException ("HashTable only takes number, boolean and string as keys.");
			}
		}

		public static DynValue keys (DynHashTable ht, DynValue[] args)
		{
			int n = ht.Count;
			DynList keys = new DynList(n);
			foreach (var key in ht.Keys){
				if (key is int)
					keys.Add((double)key);
				else if (key is bool)
					keys.Add((bool)key);
				else if (key is string)
					keys.Add(key as string);
				else
					throw new NotSupportedException ("HashTable only takes number, boolean and string as keys.");
			}
			return keys;
		}

		public static DynValue values (DynHashTable ht, DynValue[] args)
		{
			int n = ht.Count;
			DynList values = new DynList(n);
			foreach (var v in ht.Values){
				values.Add(v);
			}
			return values;
		}
		public static DynValue add (DynHashTable ht, DynValue[] args)
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
				throw new NotSupportedException ("HashTable only takes number, boolean and string as keys.");
			}
			return DynValue.Null;
		}
		public static DynValue clear (DynHashTable ht, DynValue[] args)
		{
			ht.Clear ();
			return DynValue.Null;
		}
	}
}
