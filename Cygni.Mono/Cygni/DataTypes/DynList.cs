using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.Errors;
using Cygni.Libraries;
using Cygni.DataTypes.Interfaces;

namespace Cygni.DataTypes
{
	/// <summary>
	/// Description of DynList.
	/// </summary>
	public sealed class DynList:List<DynValue>, IIndexable, IDot,ICountable
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

		public DynValue GetByIndex (DynValue index)
		{
			if (index.type == DataType.Range) {
				return ListLibrary.Slice (this, index.Value as Range);
			} else {
				return base [index.AsInt32 ()];
			}
		}

		public DynValue SetByIndex (DynValue index, DynValue value)
		{
			if (index.type == DataType.Range) {
				return ListLibrary.SliceAssign (this, index.Value as Range, value);
			} else {
				return base [index.AsInt32 ()] = value;
			}
		}

		public DynValue GetByIndexes (DynValue[] indexes)
		{
			RuntimeException.IndexerArgsCheck (indexes.Length == 1, "list");
			return this [indexes [0].AsInt32 ()];
		}

		public DynValue SetByIndexes (DynValue[] indexes, DynValue value)
		{
			RuntimeException.IndexerArgsCheck (indexes.Length == 1, "list");
			return this [indexes [0].AsInt32 ()] = value; 
		}


		public DynValue GetByDot (string fieldName)
		{
			switch (fieldName) {
			case "append":
				return DynValue.FromDelegate ("append", (args) => ListLibrary.append (this, args));
			case "count":
				return (long)this.Count;
			case "removeAt":
				return DynValue.FromDelegate ("removeAt", (args) => ListLibrary.removeAt (this, args));
			case "insert":
				return DynValue.FromDelegate ("insert", (args) => ListLibrary.insert (this, args));
			case "sort":
				return DynValue.FromDelegate ("sort", (args) => ListLibrary.sort (this, args));
			case "max":
				return DynValue.FromDelegate ("max", (args) => ListLibrary.max (this, args));
			case "min":
				return DynValue.FromDelegate ("min", (args) => ListLibrary.min (this, args));
			case "bSearch":
				return DynValue.FromDelegate ("bSearch", (args) => ListLibrary.bSearch (this, args));
			case "find":
				return DynValue.FromDelegate ("find", (args) => ListLibrary.find (this, args));
			case "concat":
				return DynValue.FromDelegate ("concat", (args) => ListLibrary.concat (this, args));
			case "copy":
				return DynValue.FromDelegate ("copy", (args) => ListLibrary.copy (this, args));
			case "pop":
				return DynValue.FromDelegate ("pop", (args) => ListLibrary.pop (this, args));
			case "clear":
				return DynValue.FromDelegate ("clear", (args) => ListLibrary.clear (this, args));
			default:
				throw RuntimeException.FieldNotExist ("list", fieldName);
			}
		}

		public string[] FieldNames {
			get {
				return new [] {
					"append", "count", "removeAt", "insert", 
					"sort", "max", "min", "bSearch", "find", 
					"concat", "copy", "pop", "clear"
				};
			}
		}

		public DynValue SetByDot (string fieldName, DynValue value)
		{
			throw RuntimeException.FieldNotExist ("list", fieldName);
		}

		public override string ToString ()
		{
			return string.Concat ("[ ", string.Join (", ", this), " ]");
		}
	}
}
