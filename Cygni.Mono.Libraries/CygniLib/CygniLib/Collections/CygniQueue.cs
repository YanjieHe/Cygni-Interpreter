using System;
using System.Linq;
using Cygni.DataTypes;
using Cygni.Errors;
using System.Collections.Generic;
using Cygni.DataTypes.Interfaces;
namespace CygniLib.Collections
{
	public class CygniQueue:Queue<DynValue>,IDot
	{
		public CygniQueue ()
		{
		}
		public DynValue GetByDot(string fieldName){
			switch(fieldName){
				case "push":
				return DynValue.FromDelegate ("push",args =>{
							RuntimeException.FuncArgsCheck(args.Length == 1,"push");
							this.Enqueue (args [0]);
							return DynValue.Nil;
							});
				case "pop":
				return DynValue.FromDelegate ("pop",args => this.Dequeue ());
				case "peek":
				return DynValue.FromDelegate ("peek",args => this.Peek ());
				case "count":
					return (double)this.Count;
				default:
					throw RuntimeException.FieldNotExist ("Queue", fieldName);
			}
		}
		public string[] FieldNames{get{ return new string[] {
			"push","pop","peek","count"
		};
		}
		}
		public DynValue SetByDot(string fieldName, DynValue value){
			throw RuntimeException.FieldNotExist ("Queue", fieldName);
		}
		public override string ToString ()
		{
			return string.Concat ("Queue([", string.Join (", ", this.Select (i => i.Value)), "])");
		}
	}
}

