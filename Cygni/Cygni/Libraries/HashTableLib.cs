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

		public static DynValue hasKey (DynValue[] args)
		{
			var ht = args [0].As<DynHashTable> ();
			var key = args [1];
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

		public static DynValue hasValue (DynValue[] args)
		{
			var ht = args [0].As<DynHashTable> ();
			var value = args [1];
			return ht.ContainsValue (value);
		}

		public static DynValue ht_remove (DynValue[] args)
		{
			var ht = args [0].As<DynHashTable> ();
			var key = args [1];
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

		public static DynValue ht_keys (DynValue[] args)
		{
			var ht = args [0].As<DynHashTable> ();
			int n = ht.Count;
			return DynValue.FromList (ht.Keys.ToDynList (i => {
				if (i is int)
					return (double)(int)i;
				if (i is bool)
					return (bool)i;
				return i as string;
			}, n));
		}

		public static DynValue ht_values (DynValue[] args)
		{
			var ht = args [0].As<DynHashTable> ();
			int n = ht.Count;
			return DynValue.FromList (ht.Values.ToDynList (i => i, n));
		}
	}
}
