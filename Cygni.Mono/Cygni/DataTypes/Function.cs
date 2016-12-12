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

		public Function Update (DynValue[] arguments)
		{
			if (arguments.Length > nArgs)
				throw RuntimeException.BadArgsNum (name, nArgs);
			var newScope = new ArrayScope (funcScope.Count, funcScope.Parent);
			newScope.Fill (arguments); // arguments are at the head of the array scope
			return new Function (name,nArgs, body, newScope);
		}

		public Function Update(NestedScope ClassScope){
			var newScope = new ArrayScope (funcScope.Count, ClassScope);
			return new Function (name, nArgs, body, newScope);
		}

		public DynValue Invoke ()
		{
			var result = body.Eval (funcScope);
			return result.type == DataType.Return
				? (DynValue)result.Value
				: DynValue.Nil;
		}

		public DynValue DynInvoke (DynValue[] args)
		{
			return this.Update (args).Invoke ();
		}

		public DynValue DynEval (ASTNode [] args, IScope scope){
			if (args.Length > nArgs)
				throw RuntimeException.BadArgsNum (name, nArgs);
			DynValue[] values = new DynValue[funcScope.Count];
			for (int i = 0; i < args.Length; i++) {
				values [i] = args [i].Eval (scope);
			}
			var newScope = new ArrayScope (values, funcScope.Parent);
			return new Function (name,nArgs, body, newScope).Invoke();
		}

		public Func<DynValue[],DynValue> AsDelegate ()
		{
			return (args) => this.Update (args).Invoke ();
		}

		public override string ToString ()
		{
			return "(Function: "+name+")";
		}
	}
}
