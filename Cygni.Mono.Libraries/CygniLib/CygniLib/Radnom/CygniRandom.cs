using System;
using Cygni.DataTypes;
using Cygni.Errors;
using System.Diagnostics;
using Cygni.DataTypes.Interfaces;

namespace CygniLib.Random
{
	public class CygniRandom:System.Random,IDot
	{
		public CygniRandom () : base ()
		{
		}

		public CygniRandom (int seed) : base (seed)
		{
		}

		public DynValue GetByDot (string fieldName)
		{
			switch (fieldName) {
			case "nextInt":
				return DynValue.FromDelegate ("nextInt",
					args => {
						RuntimeException.FuncArgsCheck (args.Length == 0 || args.Length == 2, "nextInt");
						if (args.Length == 0)
							return this.Next ();
						else
							return this.Next (
								args [0].AsInt32 (), args [1].AsInt32 ());
					});
			case "nextDouble":
				return DynValue.FromDelegate ("nextDouble", args => { 
					RuntimeException.FuncArgsCheck (args.Length == 0 || args.Length == 2, "nextInt");
					if (args.Length == 0) {
						return this.NextDouble ();	
					} else {
						double min = args [0].AsNumber ();
						double max = args [1].AsNumber ();
						return this.NextDouble () * (max - min) + min;
					}
				});
			default:
				throw RuntimeException.FieldNotExist ("Random", fieldName);
			}
		}

		public string[] FieldNames{ get { return new string[] {
				"nextInt", "nextDouble"
			}; } }

		public DynValue SetByDot (string fieldName, DynValue value)
		{
			throw RuntimeException.FieldNotExist ("Random", fieldName);
		}

		public override string ToString ()
		{
			return "(Native Class: Random)";
		}

	}
}

