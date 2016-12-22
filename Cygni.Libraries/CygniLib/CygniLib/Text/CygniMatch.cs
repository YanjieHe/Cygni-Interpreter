using System;
using System.Text.RegularExpressions;
using Cygni.DataTypes;
using Cygni.Errors;
using Cygni.DataTypes.Interfaces;

namespace CygniLib.Text
{
	public class CygniMatch:IDot
	{
		Match _match;

		public CygniMatch (Match m)
		{
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
				return DynValue.FromDelegate ("groups", (args) => {
					RuntimeException.FuncArgsCheck (args.Length == 0 || args.Length == 1, "group");
					if (args.Length == 1) {
						int index = args [0].AsInt32 ();
						return DynValue.FromObject (new CygniGroup (this._match.Groups [index]));
					} else {
						var groups = new DynList (this._match.Groups.Count);
						for (int i = 0; i < this._match.Groups.Count; i++) {
							groups.Add (DynValue.FromObject (new CygniGroup (this._match.Groups [i])));
						}
						return groups;
					}
				});
			case "nextMatch":
				return DynValue.FromDelegate ("nextMatch", (args) => DynValue.FromObject (new CygniMatch (this._match.NextMatch ())));
			default:
				throw RuntimeException.FieldNotExist ("Match", fieldName);
			}
		}

		public string[] FieldNames{ get { return new string[] {
				"success", "index", "groups", "nextMatch"
			}; } }

		public	DynValue SetByDot (string fieldName, DynValue value)
		{
			throw RuntimeException.FieldNotExist ("Match", fieldName);
		}

		public override string ToString ()
		{
			return "(Native Class: Match)";
		}
	}
}

