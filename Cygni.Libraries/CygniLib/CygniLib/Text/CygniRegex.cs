using System;
using System.Text.RegularExpressions;
using Cygni.DataTypes;
using Cygni.Errors;
using Cygni.DataTypes.Interfaces;

namespace CygniLib.Text
{
	public class CygniRegex:Regex,IDot
	{
		public CygniRegex (string pattern) : base (pattern)
		{
			
		}

		public CygniRegex (string pattern, RegexOptions option) : base (pattern, option)
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
							return	DynValue.FromUserData (new CygniMatch (this.Match (input)));
						else if (args.Length == 2) {
							int startat = args [1].AsInt32 ();
							return DynValue.FromUserData (new CygniMatch (this.Match (input, startat)));
						} else {
							int startat = args [1].AsInt32 ();
							int length = args [2].AsInt32 ();
							return DynValue.FromUserData (new CygniMatch (this.Match (input, startat, length)));
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
							int startat = args [1].AsInt32 ();
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

