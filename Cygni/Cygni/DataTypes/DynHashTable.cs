using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Cygni.DataTypes
{
	/// <summary>
	/// Description of DynHashTable.
	/// </summary>
	public sealed class DynHashTable: Dictionary<object,DynValue>, IIndexable
	{
		public DynValue this[DynValue[] key] {
			get { 
				var k = key[0];
				if (k.type == DataType.Number) {
					return this[(int)(double)k.Value];
				}
				return this[k.Value];
			}
			set { 
				var k = key[0];
				if (k.type == DataType.Number) {
					this[(int)(double)k.Value] = value;
				}
				this[k.Value] = value; 
			}
		}
		public void Add(DynValue key, DynValue value)
		{
			if (key.type == DataType.Number) {
				Add((int)(double)key.Value, value);
			} else
				Add(key.Value, value);
		}
		public override string ToString()
		{
			return string.Concat("[ ", string.Join(", ", this), " ]");
		}
	}
}
