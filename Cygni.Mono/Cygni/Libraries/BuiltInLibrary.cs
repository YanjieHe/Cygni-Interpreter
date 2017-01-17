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
        public static void BuiltIn(ResizableArrayScope scope)
        {
            foreach (var element in BuiltInFunctions)
                scope.Put(element.Key, DynValue.FromDelegate(element.Key, element.Value));
            foreach (var element in BuiltInVariables)
                scope.Put(element.Key, element.Value);
            foreach (var element in BuiltInStructures)
                scope.Put(element.Key, DynValue.FromStructure(element.Value));
            foreach (var element in BuiltInCommands)
                scope.Put(element.Key, DynValue.FromDelegate(element.Key, element.Value));
        }

        private static readonly Dictionary<string,Func<DynValue[],DynValue>> BuiltInFunctions 
		= new Dictionary<string, Func<DynValue[], DynValue>>
        {
			
            { "print",BasicLibrary.print },
            { "printf",BasicLibrary.printf },
            { "input",BasicLibrary.input },
            { "type",BasicLibrary.type },
            { "quiet",BasicLibrary.quiet },
            { "struct",BasicLibrary.Struct },
            { "tuple",BasicLibrary.tuple },
            { "LoadLibrary",BasicLibrary.LoadLibrary },

            { "exit",BasicLibrary.exit },
            { "len",BasicLibrary.len },

            { "int",BasicLibrary.toInteger },
            { "number",BasicLibrary.toNumber },
            { "bool",BasicLibrary.toBoolean },
            { "str",BasicLibrary.toString },
            { "list",BasicLibrary.toList },

            { "pCall",BasicLibrary.pCall },
            { "xpCall",BasicLibrary.xpCall },

            { "fields",BasicLibrary.fields },

            { "require",BasicLibrary.require },

            { "map",FunctionalProgrammingLibrary.Map },
            { "filter",FunctionalProgrammingLibrary.Filter },
            { "reduce",FunctionalProgrammingLibrary.Reduce },

        };

        private static readonly Dictionary<string,DynValue> BuiltInVariables 
		= new Dictionary<string, DynValue>
        {
            { "NaN",double.NaN },
            { "inf",double.PositiveInfinity },
            { "intmax",long.MaxValue },
            { "intmin",long.MinValue },
            { "realmax",double.MaxValue },
            { "realmin",double.MinValue },
            { "warranty",GlobalSettings.warranty },
        };


        private static readonly Dictionary<string,Structure> BuiltInStructures = 
            new Dictionary<string, Structure>
            {
                {"console",
                    new Structure(
                        new StructureItem("clear", DynValue.FromDelegate("clear", ConsoleLibrary.clear)),
                        new StructureItem("write", DynValue.FromDelegate("write", ConsoleLibrary.write)),
                        new StructureItem("writeLine", DynValue.FromDelegate("writeLine", ConsoleLibrary.writeLine)),
                        new StructureItem("read", DynValue.FromDelegate("read", ConsoleLibrary.read)),
                        new StructureItem("readLine", DynValue.FromDelegate("readLine", ConsoleLibrary.readLine)),
                        new StructureItem("readKey", DynValue.FromDelegate("readKey", ConsoleLibrary.readKey)) 

                    )
                },
                {"string",
                    new Structure(
                        new StructureItem("concat", DynValue.FromDelegate("concat", StringLibrary.concat)),
                        new StructureItem("compare", DynValue.FromDelegate("compare", StringLibrary.compare)),
                        new StructureItem("compareOrdinal", DynValue.FromDelegate("compareOrdinal", StringLibrary.compareOrdinal)),
                        new StructureItem("empty", string.Empty) 
                    )
                },
                {"math",
                    new Structure(
                        new StructureItem("abs", DynValue.FromDelegate("abs", MathLibrary.abs)),
                        new StructureItem("log", DynValue.FromDelegate("log", MathLibrary.log)),
                        new StructureItem("log10", DynValue.FromDelegate("log10", MathLibrary.log10)),
                        new StructureItem("sqrt", DynValue.FromDelegate("sqrt", MathLibrary.sqrt)),
                        new StructureItem("max", DynValue.FromDelegate("max", MathLibrary.max)),
                        new StructureItem("min", DynValue.FromDelegate("min", MathLibrary.min)),
                        new StructureItem("exp", DynValue.FromDelegate("exp", MathLibrary.exp)),
                        new StructureItem("sign", DynValue.FromDelegate("sign", MathLibrary.sign)),
                        new StructureItem("sin", DynValue.FromDelegate("sin", MathLibrary.sin)),
                        new StructureItem("cos", DynValue.FromDelegate("cos", MathLibrary.cos)),
                        new StructureItem("tan", DynValue.FromDelegate("tan", MathLibrary.tan)),
                        new StructureItem("asin", DynValue.FromDelegate("asin", MathLibrary.asin)),
                        new StructureItem("acos", DynValue.FromDelegate("acos", MathLibrary.acos)),
                        new StructureItem("atan", DynValue.FromDelegate("atan", MathLibrary.atan)),
                        new StructureItem("sinh", DynValue.FromDelegate("sinh", MathLibrary.sinh)),
                        new StructureItem("cosh", DynValue.FromDelegate("cosh", MathLibrary.cosh)),
                        new StructureItem("tanh", DynValue.FromDelegate("tanh", MathLibrary.tanh)),
                        new StructureItem("ceiling", DynValue.FromDelegate("ceiling", MathLibrary.ceiling)),
                        new StructureItem("floor", DynValue.FromDelegate("floor", MathLibrary.floor)),
                        new StructureItem("round", DynValue.FromDelegate("round", MathLibrary.round)),
                        new StructureItem("truncate", DynValue.FromDelegate("truncate", MathLibrary.truncate)),
                        new StructureItem("e", Math.E),
                        new StructureItem("pi", Math.PI) 
                    )
                },

            };

        private static readonly Dictionary<string,Func<ASTNode[],IScope,DynValue>> BuiltInCommands 
		= new Dictionary<string, Func<ASTNode[], IScope, DynValue>>
        {
            { "source", Commands.source },
            { "cond", Commands.cond },
            { "assert", Commands.assert },
            { "import", Commands.import },
            { "error", Commands.error },
        };

    }
}

