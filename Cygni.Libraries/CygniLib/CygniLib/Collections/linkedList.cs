using System;
using Cygni.DataTypes;
using Cygni.Errors;
using System.Collections.Generic;
using System.Linq;
namespace CygniLib.Collections
{
	public class linkedList:LinkedList<DynValue>,IDot
	{
		public linkedList ():base()
		{
		}
		public DynValue GetByDot (string fieldName){
			switch (fieldName) {
			case "count":
				return (double)this.Count;
			case "first":
				return DynValue.FromUserData (new linkedListNode (this.First));
			case "last":
				return DynValue.FromUserData (new linkedListNode (this.Last));
			case "addFirst":
				return DynValue.FromDelegate (
					args => {
						RuntimeException.FuncArgsCheck (args.Length == 1, "addFirst");
						this.AddFirst (args [0]);
						return DynValue.Null;
					}
				);
			case "addLast":
				return DynValue.FromDelegate (
					args => {
						RuntimeException.FuncArgsCheck (args.Length == 1, "addLast");
						this.AddLast (args [0]);
						return DynValue.Null;
					}
				);
			case "removeFirst":
				return DynValue.FromDelegate (
					args => {
						this.RemoveFirst ();
						return DynValue.Null;
					});
			case "removeLast":
				return DynValue.FromDelegate (
					args => {
						this.RemoveLast ();
						return DynValue.Null;
					});
			default:
				throw RuntimeException.FieldNotExist ("linkedList", fieldName);
			}
		}
		public string[] FieldNames{get{ return new string[] {
					"count" ,"first","last","addFirst","addLast","removeFirst","removeLast"
				};
			}
		}

		public	DynValue SetByDot (string fieldName, DynValue value){
			throw RuntimeException.FieldNotExist ("linkedList", fieldName);
		}
	}
}

