using System;
using System.Text.RegularExpressions;
using Cygni.DataTypes;
using Cygni.Errors;

namespace CygniLib.Text
{
	public class match:IDot
	{
		Match _match;
		public match(Match m){
			this._match = m;
		}

		public DynValue GetByDot (string fieldName)
		{
			switch (fieldName) {
			case "success":
				return this._match.Success;
			case "index":
				return this._match.Index;
			case "groups":
				return DynValue.FromDelegate ((args) => {
					RuntimeException.FuncArgsCheck (args.Length == 0 || args.Length == 1, "group");
					if (args.Length == 1) {
						int index = (int)args [0].AsNumber ();
						return DynValue.FromObject( new group( this._match.Groups [index] ));
					} else {
						var groups = new DynList (this._match.Groups.Count);
						for (int i = 0; i < this._match.Groups.Count; i++) {
							groups.Add ( DynValue.FromObject( new group( this._match.Groups [i] ) ));
						}
						return groups;
					}
				});
			case "nextMatch":
				return DynValue.FromDelegate ((args) =>  DynValue.FromObject( new match(this._match.NextMatch () )));
			default:
				throw RuntimeException.FieldNotExist ("match", fieldName);
			}
		}

		public string[] FieldNames{ get { return new string[] {
				"success","index","groups","nextMatch"
			}; } }

		public	DynValue SetByDot (string fieldName, DynValue value)
		{
			throw RuntimeException.FieldNotExist ("match", fieldName);
		}
		public override string ToString ()
		{
			return "(Native Class: match)";
		}
	}
}

