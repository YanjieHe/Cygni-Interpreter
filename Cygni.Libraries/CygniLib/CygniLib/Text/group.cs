using System;
using System.Text.RegularExpressions;
using Cygni.DataTypes;
using Cygni.Errors;

namespace CygniLib.Text
{
	public class group:IDot
	{
		Group _group;
		public group (Group g)
		{
			this._group = g;
		}
		public DynValue GetByDot (string fieldName)
		{
			switch (fieldName) {
			case "success":
				return this._group.Success;
			case "index":
				return this._group.Index;
			case "length":
				return this._group.Length;
			case "value":
				return this._group.Value;
			default:
				throw RuntimeException.FieldNotExist ("Group", fieldName);
			}
		}

		public string[] FieldNames{ get { return new string[] {
					"success","index","length","value"
				}; } }

		public	DynValue SetByDot (string fieldName, DynValue value)
		{
			throw RuntimeException.FieldNotExist ("Group", fieldName);
		}
		public override string ToString ()
		{
			return "(Native Class: Group)";
		}
	}
}

