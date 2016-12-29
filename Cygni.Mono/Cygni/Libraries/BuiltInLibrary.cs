using System;
using System.Collections.Generic;
using Cygni.DataTypes;
using Cygni.Settings;
using Cygni.AST;
using Cygni.AST.Scopes;

namespace Cygni.Libraries
{
	public static class BuiltInLibrary
	{
		public static void BuiltIn (ResizableArrayScope scope)
		{
			foreach (var element in BuiltInFunctions)
				scope.Put (element.Key, DynValue.FromDelegate (element.Key, element.Value));
			foreach (var element in BuiltInVariables)
				scope.Put (element.Key, element.Value);
			foreach (var element in BuiltInStructures)
				scope.Put (element.Key, DynValue.FromStructure (element.Value));
			foreach (var element in BuiltInCommands)
				scope.Put (element.Key, DynValue.FromDelegate (element.Key, element.Value));
		}

		private static readonly Dictionary<string,Func<DynValue[],DynValue>> BuiltInFunctions 
		= new Dictionary<string, Func<DynValue[], DynValue>> {
			
			{ "print",BasicLib.print },
			{ "printf",BasicLib.printf },
			{ "input",BasicLib.input },
			{ "type",BasicLib.type },
			{ "quiet",BasicLib.quiet },
			{ "struct",BasicLib.Struct },
			{ "tuple",BasicLib.tuple },
			{ "LoadLibrary",BasicLib.LoadLibrary },

			{ "exit",BasicLib.exit },
			{ "range",BasicLib.Range },
			{ "len",BasicLib.len },

			{ "int",BasicLib.toInteger },
			{ "number",BasicLib.toNumber },
			{ "str",BasicLib.toString },
			{ "list",BasicLib.toList },

			{ "pCall",BasicLib.pCall },
			{ "xpCall",BasicLib.xpCall },

			{ "names",BasicLib.names },

			{ "require",BasicLib.require },

			{ "map",FunctionalProgrammingLibrary.Map },
			{ "filter",FunctionalProgrammingLibrary.Filter },
			{ "reduce",FunctionalProgrammingLibrary.Reduce },

		};

		private static readonly Dictionary<string,DynValue> BuiltInVariables 
		= new Dictionary<string, DynValue> {
			{ "NaN",double.NaN },
			{ "inf",double.PositiveInfinity },
			{ "intmax",long.MaxValue },
			{ "intmin",long.MinValue },
			{ "realmax",double.MaxValue },
			{ "realmin",double.MinValue },
			{ "warranty",GlobalSettings.warranty },
		};


		private static readonly Dictionary<string,Structure> BuiltInStructures = 
			new Dictionary<string, Structure> { {"console",
					new Structure (
						new StructureItem ("clear", DynValue.FromDelegate ("clear", BasicLib.console_clear)),
						new StructureItem ("write", DynValue.FromDelegate ("write", BasicLib.console_write)),
						new StructureItem ("writeLine", DynValue.FromDelegate ("writeLine", BasicLib.console_writeLine)),
						new StructureItem ("read", DynValue.FromDelegate ("read", BasicLib.console_read)),
						new StructureItem ("readLine", DynValue.FromDelegate ("readLine", BasicLib.console_readLine)),
						new StructureItem ("readKey", DynValue.FromDelegate ("readKey", BasicLib.console_readKey)) 

					)
				}, {"string",
					new Structure (
						new StructureItem ("concat", DynValue.FromDelegate ("concat", StrLib.strcat)),
						new StructureItem ("compare", DynValue.FromDelegate ("compare", StrLib.compare)),
						new StructureItem ("empty", string.Empty) 
					)
				}, {"math",
					new Structure (
						new StructureItem ("abs", DynValue.FromDelegate ("abs", MathLib.abs)),
						new StructureItem ("log", DynValue.FromDelegate ("log", MathLib.log)),
						new StructureItem ("log10", DynValue.FromDelegate ("log10", MathLib.log10)),
						new StructureItem ("sqrt", DynValue.FromDelegate ("sqrt", MathLib.sqrt)),
						new StructureItem ("max", DynValue.FromDelegate ("max", MathLib.max)),
						new StructureItem ("min", DynValue.FromDelegate ("min", MathLib.min)),
						new StructureItem ("exp", DynValue.FromDelegate ("exp", MathLib.exp)),
						new StructureItem ("sign", DynValue.FromDelegate ("sign", MathLib.sign)),
						new StructureItem ("sin", DynValue.FromDelegate ("sin", MathLib.sin)),
						new StructureItem ("cos", DynValue.FromDelegate ("cos", MathLib.cos)),
						new StructureItem ("tan", DynValue.FromDelegate ("tan", MathLib.tan)),
						new StructureItem ("asin", DynValue.FromDelegate ("asin", MathLib.asin)),
						new StructureItem ("acos", DynValue.FromDelegate ("acos", MathLib.acos)),
						new StructureItem ("atan", DynValue.FromDelegate ("atan", MathLib.atan)),
						new StructureItem ("sinh", DynValue.FromDelegate ("sinh", MathLib.sinh)),
						new StructureItem ("cosh", DynValue.FromDelegate ("cosh", MathLib.cosh)),
						new StructureItem ("tanh", DynValue.FromDelegate ("tanh", MathLib.tanh)),
						new StructureItem ("ceiling", DynValue.FromDelegate ("ceiling", MathLib.ceiling)),
						new StructureItem ("floor", DynValue.FromDelegate ("floor", MathLib.floor)),
						new StructureItem ("round", DynValue.FromDelegate ("round", MathLib.round)),
						new StructureItem ("truncate", DynValue.FromDelegate ("truncate", MathLib.truncate)),
						new StructureItem ("e", Math.E),
						new StructureItem ("pi", Math.PI) 
					)
				}, {"file",
				new Structure (
					new StructureItem ("readLines", DynValue.FromDelegate ("readLines", IOLibrary.readLines)),
					new StructureItem ("writeLines", DynValue.FromDelegate ("writeLines", IOLibrary.writeLines)),
					new StructureItem ("exists", DynValue.FromDelegate ("exists", IOLibrary.exists))
				)
			}

			};

		private static readonly Dictionary<string,Func<ASTNode[],IScope,DynValue>> BuiltInCommands 
		= new Dictionary<string, Func<ASTNode[], IScope, DynValue>> {
			{ "source", Commands.source },
			{ "cond", Commands.cond },
			{ "assert", Commands.assert },
			{ "import", Commands.import },
			{ "error", Commands.error },
		};

	}
}

