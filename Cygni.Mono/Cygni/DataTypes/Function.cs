using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.AST;
using Cygni.Errors;
using Cygni.AST.Scopes;

namespace Cygni.DataTypes
{
	/// <summary>
	/// Description of Function.
	/// </summary>
	public sealed class Function:IFunction
	{
		readonly string name;
		readonly BlockEx body;
		readonly ArrayScope funcScope;
		readonly int nArgs;

		public Function (string name, int nArgs, BlockEx body, ArrayScope funcScope)
		{
			this.name = name;
			this.body = body;
			this.funcScope = funcScope;
			this.nArgs = nArgs;
		}

		public Function Update (NestedScope ClassScope)
		{
			var newScope = new ArrayScope (funcScope.Count, ClassScope);
			return new Function (name, nArgs, body, newScope);
		}

		public DynValue Invoke ()
		{
			DynValue result = body.Eval (funcScope);
			return result.type == DataType.Return
				? (DynValue)result.Value
				: DynValue.Nil;
		}

		public DynValue DynInvoke (DynValue[] args)
		{
			if (args.Length > nArgs) {
				throw RuntimeException.BadArgsNum (name, nArgs);
			}
			DynValue[] values = new DynValue[funcScope.Count];
			int i = 0;
			while (i < args.Length) {
				values [i] = args [i];
				i++;
			}
			while (i < nArgs) {
				values [i] = DynValue.Nil;
				i++;
			}
			var newScope = new ArrayScope (values, funcScope.Parent);
			return new Function (name, nArgs, body, newScope).Invoke ();
		}

		public DynValue DynEval (ASTNode[] args, IScope scope)
		{
			if (args.Length > nArgs)
				throw RuntimeException.BadArgsNum (name, nArgs);
			DynValue[] values = new DynValue[funcScope.Count];
			int i = 0;
			while (i < args.Length) {
				values [i] = args [i].Eval (scope);
				i++;
			}
			while (i < nArgs) {
				values [i] = DynValue.Nil;
				i++;
			}
			var newScope = new ArrayScope (values, funcScope.Parent);
			return new Function (name, nArgs, body, newScope).Invoke ();
		}

		public Func<DynValue[],DynValue> AsDelegate ()
		{
			return this.DynInvoke;
		}

		public override string ToString ()
		{
			return "(Function: " + name + ")";
		}
	}
}
