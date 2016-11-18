using System;
using Cygni.DataTypes;
using Cygni.Errors;

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

		public	DynValue GetByDot (string fieldname)
		{
			switch (fieldname) {
			case "nextInt":
				return DynValue.FromDelegate (
					args => {
						RuntimeException.FuncArgsCheck (args.Length == 0 || args.Length == 2, "nextInt");
						if (args.Length == 0)
							return this.Next ();
						else
							return this.Next (
								(int)args [0].AsNumber (), (int)args [1].AsNumber ());
					});
			case "nextDouble":
				return DynValue.FromDelegate (args => this.NextDouble ());
			default:
				throw RuntimeException.NotDefined (fieldname);
			}
		}

		public string[] FieldNames{get{ return new string[] {
					"nextInt","nextDouble"
			};
			}
		}
		public DynValue SetByDot (string fieldname, DynValue value)
		{
			throw RuntimeException.NotDefined (fieldname);
		}

	}
}

