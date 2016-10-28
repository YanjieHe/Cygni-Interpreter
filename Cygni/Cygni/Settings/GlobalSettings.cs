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

			{ "abs",MathLib.abs },
			{ "log",MathLib.log },
			{ "sqrt",MathLib.sqrt },
			{ "max",MathLib.max },
			{ "min",MathLib.min },
			{ "exp",MathLib.exp },
			{ "sign",MathLib.sign },
			
			
			{ "strcat",StrLib.strcat },
			{ "strjoin",StrLib.strjoin },
			{ "strformat",StrLib.strformat },
			{ "strlen",StrLib.strlen },
			{ "strsplit",StrLib.strsplit },
			{ "strrpl",StrLib.strrpl },
			{ "strcmp",StrLib.strcmp },
			{ "strfind",StrLib.strfind },



			{ "append",ListLib.append },
			{ "len",ListLib.len },
			{ "removeAt",ListLib.removeAt },
			{ "insertAt",ListLib.insertAt },
			
			{ "hashtable",HashTableLib.hashtable },
			{ "hasKey",HashTableLib.hasKey },
			{ "hasValue",HashTableLib.hasValue },
			{ "ht_remove",HashTableLib.ht_remove },
			{ "ht_keys",HashTableLib.ht_keys },
			{ "ht_values",HashTableLib.ht_values },
			
			
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
					new KeyValuePair<string, DynValue>("clock",BasicLib.os_clock) )
			},
			{"Console",
					new Structure (
					new KeyValuePair<string, DynValue>( "clear",BasicLib.console_clear ),
					new KeyValuePair<string, DynValue>( "write",BasicLib.console_write ),
					new KeyValuePair<string, DynValue>( "read",BasicLib.console_read ),
					new KeyValuePair<string, DynValue>( "readLine",BasicLib.console_readLine ) )

				}, {"string",
					new Structure (
					new KeyValuePair<string, DynValue>( "concat",StrLib.strcat ),
					new KeyValuePair<string, DynValue>( "join",StrLib.strjoin ),
					new KeyValuePair<string, DynValue>( "format",StrLib.strformat ),
					new KeyValuePair<string, DynValue>( "len",StrLib.strlen ),
					new KeyValuePair<string, DynValue>( "split",StrLib.strsplit ),
					new KeyValuePair<string, DynValue>( "replace",StrLib.strrpl ),
					new KeyValuePair<string, DynValue>( "compare",StrLib.strcmp ),
					new KeyValuePair<string, DynValue>( "find",StrLib.strfind ),
					new KeyValuePair<string, DynValue>( "lower",StrLib.tolower ),
					new KeyValuePair<string, DynValue>( "upper",StrLib.toupper ),
					new KeyValuePair<string, DynValue>( "char",StrLib._char ),
					new KeyValuePair<string, DynValue>( "empty",string.Empty ) ,
					new KeyValuePair<string, DynValue>( "trim",StrLib.Trim ) ,
					new KeyValuePair<string, DynValue>( "subString",StrLib.SubString ) 
				)
				},
			{"List",new Structure(
					new KeyValuePair<string, DynValue>(  "append",ListLib.append ),
					new KeyValuePair<string, DynValue>(  "len",ListLib.len ),
					new KeyValuePair<string, DynValue>(  "removeAt",ListLib.removeAt ),
					new KeyValuePair<string, DynValue>(  "insertAt",ListLib.insertAt ),
					new KeyValuePair<string, DynValue>(  "sort",ListLib.sort ),
					new KeyValuePair<string, DynValue>(  "bSearch",ListLib.bSearch ) )
			},
			{"htable",
				new Structure(
					new KeyValuePair<string, DynValue>(  "hasKey",HashTableLib.hasKey ),
					new KeyValuePair<string, DynValue>(  "hasValue",HashTableLib.hasValue ),
					new KeyValuePair<string, DynValue>(  "remove",HashTableLib.ht_remove ),
					new KeyValuePair<string, DynValue>(  "keys",HashTableLib.ht_keys ),
					new KeyValuePair<string, DynValue>(  "values",HashTableLib.ht_values ) )
			},
			};

		public static void SetBuiltInFunctions (IScope GlobalScope)
		{
			foreach (var element in BuiltInFunctions)
				GlobalScope.Put (element.Key, DynValue.FromNativeFunction (new  NativeFunction (element.Value)));
		}

		public static void SetBuiltInVariables (IScope GlobalScope)
		{
			foreach (var element in BuiltInVariables)
				GlobalScope.Put (element.Key, element.Value);
		}

		public static void SetBuiltInStructures (IScope GlobalScope)
		{
			foreach (var element in BuiltInStructures)
				GlobalScope.Put (element.Key, DynValue.FromStructure (element.Value));
		}
		public static void BuiltIn (string name, Func<DynValue[],DynValue> f)
		{
			BuiltInFunctions.Add (name, f);
		}

		public static void AddVariable (string name, DynValue value)
		{
			BuiltInVariables.Add (name, value);
		}

		public static BasicScope CreateGlobalScope ()
		{
			var scope = new BasicScope ();
			SetBuiltInFunctions (scope);
			SetBuiltInVariables (scope);
			SetBuiltInStructures (scope);
			return scope;
		}

	}
}
