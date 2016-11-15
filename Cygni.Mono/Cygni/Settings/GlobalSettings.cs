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
			{ "cast",BasicLib.cast },
			{ "typeinfo",BasicLib.typeinfo },
			{ "quiet",BasicLib.quiet },
			{ "struct",BasicLib.Struct },
			{ "scan",BasicLib.scan },
			{ "bit_or",BasicLib.bit_or },
			{ "CSharpDll",BasicLib.CSharpDll },
			{ "dispose",BasicLib.dispose },
			{ "throw",BasicLib.Throw },
			{ "exit",BasicLib.exit },
			{ "range",BasicLib.Range },
			{ "collect",BasicLib.Collect },

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

			
			
			{ "strcat",StrLib.strcat },
			{ "strjoin",StrLib.strjoin },
			{ "strformat",StrLib.strformat },
			{ "strlen",StrLib.strlen },
			{ "strsplit",StrLib.strsplit },
			{ "strrpl",StrLib.strrpl },
			{ "strcmp",StrLib.strcmp },
			{ "strfind",StrLib.strfind },



/*			{ "append",ListLib.append },
			{ "len",ListLib.len },
			{ "removeAt",ListLib.removeAt },
			{ "insertAt",ListLib.insertAt }, */

			{ "hashtable",HashTableLib.hashtable },
/*			{ "hasKey",HashTableLib.hasKey },
			{ "hasValue",HashTableLib.hasValue },
			{ "ht_remove",HashTableLib.ht_remove },
			{ "ht_keys",HashTableLib.ht_keys },
			{ "ht_values",HashTableLib.ht_values }, */
			
			
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
					new StructureItem( "join",StrLib.strjoin ),
					new StructureItem( "format",StrLib.strformat ),
					new StructureItem( "len",StrLib.strlen ),
					new StructureItem( "split",StrLib.strsplit ),
					new StructureItem( "replace",StrLib.strrpl ),
					new StructureItem( "compare",StrLib.strcmp ),
					new StructureItem( "find",StrLib.strfind ),
					new StructureItem( "lower",StrLib.tolower ),
					new StructureItem( "upper",StrLib.toupper ),
					new StructureItem( "char",StrLib._char ),
					new StructureItem( "empty",string.Empty ) ,
					new StructureItem( "trim",StrLib.Trim ) ,
					new StructureItem( "subString",StrLib.SubString ) 
				)
				},
			/*{"List",new Structure(
					new StructureItem(  "append",ListLib.append ),
					new StructureItem(  "len",ListLib.len ),
					new StructureItem(  "removeAt",ListLib.removeAt ),
					new StructureItem(  "insertAt",ListLib.insertAt ),
					new StructureItem(  "sort",ListLib.sort ),
					new StructureItem(  "bSearch",ListLib.bSearch ) ,
					new StructureItem(  "max",ListLib.list_max ),
					new StructureItem(  "min",ListLib.list_min ) ,
					new StructureItem(  "pop",ListLib.list_pop ) ,
					new StructureItem(  "clear",ListLib.list_clear ) 
					)
			}, */
			/* {"htable",
				new Structure(
					new StructureItem(  "hasKey",HashTableLib.hasKey ),
					new StructureItem(  "hasValue",HashTableLib.hasValue ),
					new StructureItem(  "remove",HashTableLib.ht_remove ),
					new StructureItem(  "keys",HashTableLib.ht_keys ),
					new StructureItem(  "values",HashTableLib.ht_values ) ,
					new StructureItem(  "add",HashTableLib.ht_add ) ,
					new StructureItem(  "clear",HashTableLib.ht_clear )
				)
			},*/
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
