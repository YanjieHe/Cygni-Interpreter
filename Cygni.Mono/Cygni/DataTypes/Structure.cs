using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.Errors;

namespace Cygni.DataTypes
{
	/// <summary>
	/// Description of Structure.
	/// </summary>
	public sealed class Structure:IDot,IIndexable
	{
		readonly StructureItem[] contents;

		public Structure (int size)
		{
			contents = new StructureItem[size];
		}
		internal Structure(params StructureItem[] contents){
			this.contents = contents;
		}

		public void SetAt (int i, string key, DynValue value)
		{
			contents [i] = new StructureItem (key, value);
		}

		#region IDot implementation

		public DynValue GetByDot (string fieldname)
		{
			for (int i = contents.Length - 1; i >= 0; i--) {
				if (string.Equals (contents [i].Key, fieldname))
					return contents [i].Value;
			}
			throw RuntimeException.NotDefined (fieldname);
		}

		public DynValue SetByDot (string fieldname, DynValue value)
		{
			for (int i = contents.Length - 1; i >= 0; i--) {
				if (string.Equals (contents [i].Key, fieldname))
					return contents [i].Value = value;
			}
			throw RuntimeException.NotDefined (fieldname);
		}

		#endregion

		public DynValue this [DynValue[] indexes]{ 
			get { return contents [(int)indexes [0].AsNumber ()].Value; } 
			set { contents [(int)indexes [0].AsNumber ()].Value = value; } }

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
			/*var s = new StringBuilder ("struct(");
			if (contents.Length >= 1) {
				s.Append (contents[0].Key);
				s.Append (": ");
				s.AppendLine (contents[0].Value.ToString());
			}
			for (int i = 1; i < contents.Length; i++) {
				s.Append (", ");
				s.Append (contents[i].Key);
				s.Append (": ");
				s.AppendLine (contents[i].Value.ToString());
			}
			s.Append (')');*/
			return s.ToString ();
		}
	}
}
