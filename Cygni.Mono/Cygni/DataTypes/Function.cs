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
			if (nArgs != arguments.Length)
				throw RuntimeException.BadArgsNum (name, nArgs);
			var args = new DynValue[funcScope.Count];
			Array.Copy (arguments, args, nArgs);
			var newScope = new ArrayScope (args, funcScope.Parent);
			return new Function (name,nArgs, body, newScope);
		}

		public DynValue Invoke ()
		{
			var result = body.Eval (funcScope);
			if (result.type == DataType.Return)
				return (DynValue)result.Value;
			return result;
		}

		public DynValue DynInvoke (DynValue[] args)
		{
			return this.Update (args).Invoke ();
		}

		public Func<DynValue[],DynValue> AsDelegate ()
		{
			return (args) => this.Update (args).Invoke ();
		}

		public override string ToString ()
		{
			return "(Function)";
		}
	}
}
