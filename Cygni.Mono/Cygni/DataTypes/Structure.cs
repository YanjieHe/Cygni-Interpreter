using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.Errors;

namespace Cygni.DataTypes
{
	public sealed class Structure:IDot  
	{
		readonly StructureItem[] contents;

		public Structure (params StructureItem[] contents)
		{
			this.contents = contents;
			// SortByFields ();
		}

		public DynValue GetByDot (string fieldName)
		{
			foreach (var item in contents) {
				if(string.Equals(item.Key, fieldName))
					return item.Value;
			}
			throw RuntimeException.FieldNotExist ("struct", fieldName);
			// return BinarySearch (0, contents.Length - 1, fieldName).Value;
		}

		public DynValue SetByDot (string fieldName, DynValue value)
		{
			foreach (var item in contents) {
				if(string.Equals(item.Key, fieldName))
					return item.Value = value;
			}
			throw RuntimeException.FieldNotExist ("struct", fieldName);
			// return BinarySearch (0, contents.Length - 1, fieldName).Value = value;
		}

		public string[] FieldNames {
			get {
				string[] names = new string[contents.Length];
				for (int i = 0; i < names.Length; i++)
					names [i] = contents [i].Key;
				return names;
			}
		}

		/* private void SortByFields ()
		{
			// Insertion Sort
			for (int i = 1; i < contents.Length; i++)
				if (string.CompareOrdinal (contents [i].Key, contents [i - 1].Key) < 0) {  
					StructureItem t = contents [i];  
					int j = i - 1;  
					while (j >= 0 && string.CompareOrdinal (contents [j].Key, t.Key) > 0) {
						contents [j + 1] = contents [j];
						j--;
					}
					contents [j + 1] = t;  
				}
		}

		private StructureItem BinarySearch (int low, int high, string field)
		{
			int mid;
			while (low <= high) {
				mid = (low + high) >> 1;
				int cmp = string.CompareOrdinal (contents [mid].Key, field);
				if (cmp > 0) {
					high = mid - 1;
				} else if (cmp < 0) {
					low = mid + 1;
				} else
					return contents [mid];
			}
			throw RuntimeException.FieldNotExist ("struct", field);
		} */

		public override string ToString ()
		{
			var s = new StringBuilder ("struct: {\r\n");
			foreach (var item in contents) {
				s
					.Append ('\t')
					.Append (item.Key)
					.Append (": ")
					.AppendLine (item.Value.ToString ());
			}
			s.Append ('}');
			return s.ToString ();
		}
	}
}
