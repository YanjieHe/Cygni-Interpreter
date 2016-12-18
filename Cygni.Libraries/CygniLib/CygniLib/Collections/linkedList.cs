using System;
using Cygni.DataTypes;
using Cygni.Errors;
using System.Collections.Generic;
using System.Linq;
using Cygni.DataTypes.Interfaces;

namespace CygniLib.Collections
{
	public class linkedList:LinkedList<DynValue>,IDot
	{
		public linkedList () : base ()
		{
		}

		public DynValue GetByDot (string fieldName)
		{
			switch (fieldName) {
			case "count":
				return (double)this.Count;
			case "first":
				return DynValue.FromUserData (new linkedListNode (this.First));
			case "last":
				return DynValue.FromUserData (new linkedListNode (this.Last));
			case "addFirst":
				return DynValue.FromDelegate ("addFitst",
					args => {
						RuntimeException.FuncArgsCheck (args.Length == 1, "addFirst");
						this.AddFirst (args [0]);
						return DynValue.Nil;
					}
				);
			case "addLast":
				return DynValue.FromDelegate ("addLast",
					args => {
						RuntimeException.FuncArgsCheck (args.Length == 1, "addLast");
						this.AddLast (args [0]);
						return DynValue.Nil;
					}
				);
			case "removeFirst":
				return DynValue.FromDelegate ("removeFirst",
					args => {
						this.RemoveFirst ();
						return DynValue.Nil;
					});
			case "removeLast":
				return DynValue.FromDelegate ("removeLast",
					args => {
						this.RemoveLast ();
						return DynValue.Nil;
					});
			default:
				throw RuntimeException.FieldNotExist ("LinkedList", fieldName);
			}
		}

		public string[] FieldNames{ get { return new string[] {
				"count", "first", "last", "addFirst", "addLast", "removeFirst", "removeLast"
			}; } }

		public	DynValue SetByDot (string fieldName, DynValue value)
		{
			throw RuntimeException.FieldNotExist ("LinkedList", fieldName);
		}

		public override string ToString ()
		{
			return string.Format ("LinkedList([0])", string.Join (", ", this.AsEnumerable ()));
		}
	}
}

