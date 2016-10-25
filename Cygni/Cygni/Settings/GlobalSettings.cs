﻿using System.Collections.Generic;
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
		const string warranty = 
			@"
Cygni, version 1.0.0
Copyright (C) <2016> <He Yanjie>

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the ""Software""), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
　　
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
";

		public static Dictionary<string,Structure> BuiltInStructures = 
			new Dictionary<string, Structure> { 
			{"os",new Structure(){
					{"clock",BasicLib.os_clock}
				}
			},
			{"console",
					new Structure () {
					{ "clear",BasicLib.console_clear },
					{ "write",BasicLib.console_write },
					{ "read",BasicLib.console_read },
					{ "readLine",BasicLib.console_readLine },

					}
				}, {"string",
					new Structure () {
						{ "concat",StrLib.strcat },
						{ "join",StrLib.strjoin },
						{ "format",StrLib.strformat },
						{ "len",StrLib.strlen },
						{ "split",StrLib.strsplit },
						{ "replace",StrLib.strrpl },
						{ "compare",StrLib.strcmp },
						{ "find",StrLib.strfind },
						{ "lower",StrLib.tolower },
						{ "upper",StrLib.toupper },
						{ "char",StrLib._char },
						{ "empty",string.Empty },
					}
				},
			{"List",new Structure(){
					{ "append",ListLib.append },
					{ "len",ListLib.len },
					{ "removeAt",ListLib.removeAt },
					{ "insertAt",ListLib.insertAt },
					{ "sort",ListLib.sort },
					{ "bSearch",ListLib.bSearch },
				}
			},
			{"htable",
				new Structure() {
					{ "hasKey",HashTableLib.hasKey },
					{ "hasValue",HashTableLib.hasValue },
					{ "remove",HashTableLib.ht_remove },
					{ "keys",HashTableLib.ht_keys },
					{ "values",HashTableLib.ht_values },
				}
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
