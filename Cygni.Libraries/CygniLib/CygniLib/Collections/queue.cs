using System;
using System.Linq;
using Cygni.DataTypes;
using Cygni.Errors;
using System.Collections.Generic;
namespace CygniLib.Collections
{
	public class queue:Queue<DynValue>,IDot
	{
		public queue ()
		{
		}
		public DynValue GetByDot(string fieldName){
			switch(fieldName){
				case "push":
					return DynValue.FromDelegate (args =>{
							RuntimeException.FuncArgsCheck(args.Length == 1,"push");
							this.Enqueue (args [0]);
							return DynValue.Nil;
							});
				case "pop":
					return DynValue.FromDelegate (args => this.Dequeue ());
				case "peek":
					return DynValue.FromDelegate (args => this.Peek ());
				case "count":
					return (double)this.Count;
				default:
					throw RuntimeException.FieldNotExist ("queue", fieldName);
			}
		}
		public string[] FieldNames{get{ return new string[] {
			"push","pop","peek","count"
		};
		}
		}
		public DynValue SetByDot(string fieldName, DynValue value){
			throw RuntimeException.FieldNotExist ("queue", fieldName);
		}
		public override string ToString ()
		{
			return string.Concat ("queue([", string.Join (", ", this.Select (i => i.Value)), "])");
		}
	}
}

