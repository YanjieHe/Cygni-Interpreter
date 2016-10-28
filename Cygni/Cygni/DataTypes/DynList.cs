using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Cygni.DataTypes
{
	/// <summary>
	/// Description of DynList.
	/// </summary>
	public sealed class DynList:List<DynValue>, IIndexable
	{
		public DynList(int capacity)
			: base(capacity)
		{
		}
		public DynList(IEnumerable<DynValue>collection)
			: base(collection)
		{
		}
		public DynList(IEnumerable<DynValue>collection, int capacity)
			: base(capacity)
		{
			AddRange(collection);
		}
		public DynValue this[DynValue[] indexes] {
			get{ return this[(int)indexes[0].AsNumber()]; }
			set{ this[(int)indexes[0].AsNumber()] = value; }
		}
		public override string ToString()
		{
			return string.Concat("[ ", string.Join(", ", this), " ]");
		}
	}
}
