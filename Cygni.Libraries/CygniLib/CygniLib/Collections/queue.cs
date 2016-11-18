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
		public DynValue GetByDot(string fieldname){
			switch(fieldname){
			case "push":
				return DynValue.FromDelegate (args =>{
					RuntimeException.FuncArgsCheck(args.Length == 1,"push");
					this.Enqueue (args [0]);
					return DynValue.Null;
				});
			case "pop":
				return DynValue.FromDelegate (args => this.Dequeue ());
			case "peek":
				return DynValue.FromDelegate (args => this.Peek ());
			case "count":
				return (double)this.Count;
			default:
				throw RuntimeException.NotDefined (fieldname);
			}
		}
		public DynValue SetByDot(string fieldname, DynValue value){
			throw	RuntimeException.NotDefined (fieldname);
		}
		public override string ToString ()
		{
			return string.Concat ("queue([", string.Join (", ", this.Select (i => i.Value)), "])");
		}
	}
}

