using System;
using Cygni.DataTypes;
using Cygni.Errors;
using System.Diagnostics;
using Cygni.DataTypes.Interfaces;
namespace CygniLib.random
{
	public class random:Random,IDot
	{
		public random () : base ()
		{
		}

		public random (int seed) : base (seed)
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
								(int)args [0].AsNumber (), (int)args [1].AsNumber ());
					});
			case "nextDouble":
				return DynValue.FromDelegate ("nextDouble",args => this.NextDouble ());
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

