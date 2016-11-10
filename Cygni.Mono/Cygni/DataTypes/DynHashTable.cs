﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Cygni.DataTypes
{
	/// <summary>
	/// Description of DynHashTable.
	/// </summary>
	public sealed class DynHashTable: Dictionary<object,DynValue> ,IEnumerable<DynValue>, IIndexable
	{
		public DynValue this [DynValue[] key] {
			get { 
				var k = key [0];
				switch (k.type) {
				case DataType.Number:
					return this [(int)(double)k.Value];
				case DataType.Boolean:
				case DataType.String:
					return this [k.Value];
				default :
					throw new NotSupportedException ("HashTable only takes number, boolean and string as keys.");
				}
			}
			set { 
				var k = key [0];
				switch (k.type) {
				case DataType.Number:
					this [(int)(double)k.Value] = value;
					return;
				case DataType.Boolean:
				case DataType.String:
					this [k.Value] = value;
					return;
				default :
					throw new NotSupportedException ("HashTable only takes number, boolean and string as keys.");
				}
			}
		}

		public void Add (DynValue key, DynValue value)
		{
			switch (key.type) {
			case DataType.Number:
				Add ((int)(double)key.Value, value);
				return;
			case DataType.Boolean:
			case DataType.String:
				Add (key.Value, value);
				return;
			default :
				throw new NotSupportedException ("HashTable only takes number, boolean and string as keys.");
			}
		}

		public override string ToString ()
		{
			StringBuilder s = new StringBuilder ();
			var iterator = base.GetEnumerator();
			s.Append ("[ ");
			if (iterator.MoveNext ())
				s.Append (iterator.Current);
			while (iterator.MoveNext()) {
				s.Append (',').Append (iterator.Current);
			}
			s.Append (" ]");
			return s.ToString ();
		}

		public new IEnumerator<DynValue> GetEnumerator ()
		{
			var iterator = base.GetEnumerator();
			while(iterator.MoveNext()){
				var kvp = new DynList (2);
				kvp.Add (DynValue.FromObject (iterator.Current.Key));
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
	}
}