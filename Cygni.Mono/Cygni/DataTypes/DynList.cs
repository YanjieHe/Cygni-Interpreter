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
		public DynValue GetByIndex(DynValue index){
			return base [(int)index.AsNumber ()];
		}
		public DynValue SetByIndex(DynValue index, DynValue value){
			return base [(int)index.AsNumber ()] = value;
		}

		public DynValue GetByIndexes(DynValue[] indexes){
			RuntimeException.IndexerArgsCheck (indexes.Length == 1, "list");
			return this [(int)indexes [0].AsNumber ()];
		}
		public DynValue SetByIndexes(DynValue[] indexes, DynValue value){
			RuntimeException.IndexerArgsCheck (indexes.Length == 1, "list");
			return this [(int)indexes [0].AsNumber ()] = value; 
		}


		public DynValue GetByDot (string fieldName)
		{
			switch (fieldName) {
			case "append":
				return DynValue.FromDelegate ((args) => ListLib.append (this, args));
			case "count":
				return (double)this.Count;
			case "removeAt":
				return DynValue.FromDelegate ((args) => ListLib.removeAt (this, args));
			case "insert":
				return DynValue.FromDelegate ((args) => ListLib.insert (this, args));
			case "sort":
				return DynValue.FromDelegate ((args) => ListLib.sort (this, args));
			case "max":
				return DynValue.FromDelegate ((args) => ListLib.max (this, args));
			case "min":
				return DynValue.FromDelegate ((args) => ListLib.min (this, args));
			case "bSearch":
				return DynValue.FromDelegate ((args) => ListLib.bSearch (this, args));
			case "find":
				return DynValue.FromDelegate ((args) => ListLib.find (this, args));
			case "concat":
				return DynValue.FromDelegate ((args) => ListLib.concat (this, args));
			case "slice":
				return DynValue.FromDelegate ((args) => ListLib.slice (this, args));
			case "pop":
				return DynValue.FromDelegate ((args) => ListLib.pop (this, args));
			case "clear":
				return DynValue.FromDelegate ((args) => ListLib.clear (this, args));
			default:
				throw RuntimeException.FieldNotExist ("list",fieldName);
			}
		}

		public string[] FieldNames{
			get{
				return new string[] {
					"append", "count", "removeAt", "insert", "sort", "max", "min", "bSearch", "find", "concat","slice","pop","clear"
				};
				}}
		public DynValue SetByDot (string fieldName, DynValue value)
		{
			throw RuntimeException.NotDefined (fieldName);
		}

		public override string ToString ()
		{
			return string.Concat ("[ ", string.Join (", ", this), " ]");
		}
	}
}
