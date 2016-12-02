using System;
using System.Text.RegularExpressions;
using Cygni.DataTypes;
using Cygni.Errors;

namespace CygniLib.Text
{
	public class regex:Regex,IDot
	{
		public regex (string pattern) : base (pattern)
		{
			
		}

		public DynValue GetByDot (string fieldName)
		{
			switch (fieldName) {
			case "match":
				return DynValue.FromDelegate ((args) => {
					RuntimeException.FuncArgsCheck (args.Length == 1 || args.Length == 2 || args.Length == 3, "match");
					string input = args [0].AsString ();
					if (args.Length == 1)
						return	DynValue.FromUserData(new match( this.Match (input)));
					else if (args.Length == 2) {
						int startat = (int)args [1].AsNumber ();
						return DynValue.FromUserData(new match( this.Match (input, startat) ));
					} else {
						int startat = (int)args [1].AsNumber ();
						int length = (int)args [2].AsNumber ();
						return DynValue.FromUserData(new match( this.Match (input, startat, length) ));
					}
				});
			case "isMatch":
				return DynValue.FromDelegate ((args) => {
					RuntimeException.FuncArgsCheck (args.Length == 1 || args.Length == 2 , "isMatch");
					string input = args [0].AsString ();
					if (args.Length == 1)
						return	this.IsMatch (input);
					else  {
						int startat = (int)args [1].AsNumber ();
						return this.IsMatch (input, startat) ;
					}
				});
			case "replace":
				return DynValue.FromDelegate ((args) => {
					RuntimeException.FuncArgsCheck (args.Length == 2, "replace");
					string input = args [0].AsString ();
					string replacement = args [1].AsString ();
					return this.Replace (input, replacement);
				});
			default:
				throw RuntimeException.FieldNotExist ("regex", fieldName);
			}
		}

		public string[] FieldNames{ get { return new string[] {
				"match","isMatch","replace"
			}; } }

		public	DynValue SetByDot (string fieldName, DynValue value)
		{
			throw RuntimeException.FieldNotExist ("regex", fieldName);
		}
		public override string ToString ()
		{
			return "(Native Class: regex)";
		}
	}
}

