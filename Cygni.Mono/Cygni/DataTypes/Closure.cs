using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.AST;
using Cygni.Errors;
using Cygni.AST.Scopes;
using Cygni.DataTypes.Interfaces;

namespace Cygni.DataTypes
{
	public sealed class Closure:IFunction
	{
		readonly int nArgs;
		readonly ArrayScope funcScope;
		readonly ASTNode body;

		public Closure (int nArgs, ASTNode body, ArrayScope funcScope)
		{
			this.nArgs = nArgs;
			this.body = body;
			this.funcScope = funcScope;
		}

		public DynValue Invoke ()
		{
			DynValue result = body.Eval (funcScope);
			return result.type == DataType.Return
				? result.Value as DynValue
					: DynValue.Nil;
		}

		public DynValue DynInvoke (DynValue[] args)
		{
			if (args.Length > nArgs) {
				throw RuntimeException.BadArgsNum ("Anonymous Function", nArgs);
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
			return new Closure (nArgs, body, newScope).Invoke ();
		}

		public DynValue DynEval (ASTNode[] args, IScope scope)
		{
			if (args.Length > nArgs)
				throw RuntimeException.BadArgsNum ("Anonymous Function", nArgs);
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
			return new Closure (nArgs, body, newScope).Invoke ();
		}

		public override string ToString ()
		{
			return "(Anonymous Function)";
		}
	}
}

