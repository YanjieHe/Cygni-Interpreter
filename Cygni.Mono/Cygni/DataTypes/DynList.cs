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
	/// Description of DynList.
	/// </summary>
	public sealed class DynList:List<DynValue>, IIndexable,IDot
	{
		public DynList (int capacity)
			: base (capacity)
		{
		}

		public DynList (IEnumerable<DynValue>collection)
			: base (collection)
		{
		}

		public DynList (IEnumerable<DynValue>collection, int capacity)
			: base (capacity)
		{
			AddRange (collection);
		}

		public DynValue this [DynValue[] indexes] {
			get{ return this [(int)indexes [0].AsNumber ()]; }
			set{ this [(int)indexes [0].AsNumber ()] = value; }
		}

		public DynValue GetByDot (string fieldname)
		{
			switch (fieldname) {
			case "append":
				return DynValue.FromDelegate ((args) => ListLib.append (this, args));
			case "count":
				return (double)this.Count;
			case "removeAt":
				return DynValue.FromDelegate ((args) => ListLib.removeAt (this, args));
			case "insert":
				return DynValue.FromDelegate ((args) => ListLib.insert (this, args));
			default:
				throw RuntimeException.NotDefined (fieldname);
			}
		}

		public DynValue SetByDot (string fieldname, DynValue value)
		{
			throw RuntimeException.NotDefined (fieldname);
		}

		public override string ToString ()
		{
			return string.Concat ("[ ", string.Join (", ", this), " ]");
		}
	}
}
