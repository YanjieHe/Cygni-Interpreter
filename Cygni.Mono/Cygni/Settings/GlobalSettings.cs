using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.AST;
using Cygni.DataTypes;
using Cygni.Libraries;
using Cygni.Extensions;
using Cygni.AST.Scopes;
using System.Reflection;
using System.IO;
namespace Cygni.Settings
{
	/// <summary>
	/// Description of GlobalSettings.
	/// </summary>
	public static class GlobalSettings
	{
		public static Dictionary<string,Func<DynValue[],DynValue>> BuiltInFunctions 
			= new Dictionary<string, Func<DynValue[], DynValue>> {
			{ "print",BasicLib.print },
			{ "printf",BasicLib.printf },
			{ "input",BasicLib.input },
			{ "cast",BasicLib.cast },
			{ "getType",BasicLib.getType },
			{ "quiet",BasicLib.quiet },
			{ "struct",BasicLib.Struct },
			{ "scan",BasicLib.scan },
			{ "bit_or",BasicLib.bit_or },
			{ "CSharpDll",BasicLib.CSharpDll },
			{ "dispose",BasicLib.dispose },
			{ "throw",BasicLib.Throw },
			{ "exit",BasicLib.exit },
			{ "range",BasicLib.Range },
			{ "collect",BasicLib.collect },
			{ "len",BasicLib.len },
			{ "toNumber",BasicLib.toNumber },
			{ "toString",BasicLib.toString},
			{ "toList",BasicLib.toList},
			{ "tryCatch",BasicLib.tryCatch},
			{ "names",BasicLib.names},
			{ "getwd",BasicLib.getwd},
			{ "setwd",BasicLib.setwd},

			{"strcat",StrLib.strcat},

			{ "abs",MathLib.abs },
			{ "log",MathLib.log },
			{ "sqrt",MathLib.sqrt },
			{ "max",MathLib.max },
			{ "min",MathLib.min },
			{ "exp",MathLib.exp },
			{ "sign",MathLib.sign },
			{ "sin",MathLib.sin },
			{ "cos",MathLib.cos },
			{ "tan",MathLib.tan },
			{ "asin",MathLib.asin },
			{ "acos",MathLib.acos },
			{ "atan",MathLib.atan },
			{ "ceiling",MathLib.ceiling },
			{ "floor",MathLib.floor },
			{ "round",MathLib.round },

		};
		public static Dictionary<string,DynValue> BuiltInVariables 
			= new Dictionary<string, DynValue> {
			{ "pi",Math.PI },
			{ "inf",double.PositiveInfinity },
			{ "realmax",double.MaxValue },
			{ "realmin",double.MinValue },
			{ "warranty",warranty },
		};
		public static bool Quiet = false;
		//quiet output
		public static bool CompleteErrorOutput = false;
		public static string CurrentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
		const string warranty = 
			@"
Cygni, version 1.0.0
Copyright (C) <2016> <He Yanjie>

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the ""Software""), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
　　
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
";

		public static Dictionary<string,Structure> BuiltInStructures = 
			new Dictionary<string, Structure> { {"os",new Structure(
					new StructureItem("clock",BasicLib.os_clock) )
			},
			{"Console",
					new Structure (
					new StructureItem( "clear",BasicLib.console_clear ),
					new StructureItem( "write",BasicLib.console_write ),
					new StructureItem( "read",BasicLib.console_read ),
					new StructureItem( "readLine",BasicLib.console_readLine ) )

				}, {"string",
					new Structure (
					new StructureItem( "concat",StrLib.strcat ),
					new StructureItem( "compare",StrLib.strcmp ),
					new StructureItem( "empty",string.Empty ) 
				)
				},
			};

		public static void SetBuiltInFunctions (BuiltInScope scope)
		{
			foreach (var element in BuiltInFunctions)
				scope.BuiltIn (element.Key, DynValue.FromNativeFunction (new  NativeFunction (element.Value)));
		}

		public static void SetBuiltInVariables (BuiltInScope scope)
		{
			foreach (var element in BuiltInVariables)
				scope.BuiltIn (element.Key, element.Value);
		}

		public static void SetBuiltInStructures (BuiltInScope scope)
		{
			foreach (var element in BuiltInStructures)
				scope.BuiltIn (element.Key, DynValue.FromStructure (element.Value));
		}

		public static BuiltInScope CreateBuiltInScope ()
		{
			var scope = new BuiltInScope ();
			SetBuiltInFunctions (scope);
			SetBuiltInVariables (scope);
			SetBuiltInStructures (scope);
			return scope;
		}

	}
}
