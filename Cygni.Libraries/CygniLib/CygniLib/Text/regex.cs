using System;
using System.Text.RegularExpressions;
using Cygni.DataTypes;
using Cygni.Errors;
using Cygni.DataTypes.Interfaces;

namespace CygniLib.Text
{
	public class regex:Regex,IDot
	{
		public regex (string pattern) : base (pattern)
		{
			
		}

		public regex (string pattern, RegexOptions option) : base (pattern, option)
		{

		}

		public DynValue GetByDot (string fieldName)
		{
			switch (fieldName) {
			case "match":
				return DynValue.FromDelegate ("match",
					(args) => {
						RuntimeException.FuncArgsCheck (args.Length == 1 || args.Length == 2 || args.Length == 3, "match");
						string input = args [0].AsString ();
						if (args.Length == 1)
							return	DynValue.FromUserData (new match (this.Match (input)));
						else if (args.Length == 2) {
							int startat = (int)args [1].AsNumber ();
							return DynValue.FromUserData (new match (this.Match (input, startat)));
						} else {
							int startat = (int)args [1].AsNumber ();
							int length = (int)args [2].AsNumber ();
							return DynValue.FromUserData (new match (this.Match (input, startat, length)));
						}
					});
			case "isMatch":
				return DynValue.FromDelegate ("isMatch",
					(args) => {
						RuntimeException.FuncArgsCheck (args.Length == 1 || args.Length == 2, "isMatch");
						string input = args [0].AsString ();
						if (args.Length == 1)
							return	this.IsMatch (input);
						else {
							int startat = (int)args [1].AsNumber ();
							return this.IsMatch (input, startat);
						}
					});
			case "replace":
				return DynValue.FromDelegate ("replace",
					(args) => {
						RuntimeException.FuncArgsCheck (args.Length == 2, "replace");
						string input = args [0].AsString ();
						string replacement = args [1].AsString ();
						return this.Replace (input, replacement);
					});
			case "split":
				return DynValue.FromDelegate ("split",
					args => {
						RuntimeException.FuncArgsCheck (args.Length == 1, "split");
						string input = args [0].AsString ();
						string[] arr = this.Split (input);
						DynList result = new DynList (arr.Length);
						for (int i = 0; i < arr.Length; i++) {
							result.Add (arr [i]);
						}
						return result;
					});

			default:
				throw RuntimeException.FieldNotExist ("Regex", fieldName);
			}
		}

		public string[] FieldNames{ get { return new string[] {
				"match", "isMatch", "replace", "split"
			}; } }

		public	DynValue SetByDot (string fieldName, DynValue value)
		{
			throw RuntimeException.FieldNotExist ("Regex", fieldName);
		}

		public override string ToString ()
		{
			return "(Native Class: Regex)";
		}
	}
}

