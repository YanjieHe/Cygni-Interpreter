using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.AST;
using Cygni.Errors;
namespace Cygni.DataTypes
{
	/// <summary>
	/// Description of Function.
	/// </summary>
	public sealed class Function
	{
		string name;
		BlockEx body;
		string[] parameters;
		NestedScope funcScope;
		int nArgs;
		public Function(string name, string[] parameters, BlockEx body, NestedScope funcScope)
		{
			this.name = name;
			this.parameters = parameters;
			this.body = body;
			this.funcScope = funcScope;
			this.nArgs = parameters.Length;
		}
		
		public Function Update(DynValue[] arguments)
		{
			if (nArgs != arguments.Length)
				throw RuntimeException.BadArgsNum(name, nArgs);
			var newScope = new NestedScope(funcScope.Parent);
			
			for (int i = 0; i < nArgs; i++)
				newScope[parameters[i]] = arguments[i];
			
			return new Function(name, parameters, body, newScope);
		}

		public DynValue Invoke()
		{
			var result = body.Eval(funcScope);
			if (result.type == DataType.Return) 
				return result.Value as DynValue;
			return result;
		}

		public override string ToString()
		{
			return "(Function)";
		}
	}
}
