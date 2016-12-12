using System;
using System.Collections.Generic;
using Cygni.AST.Scopes;
using Cygni.AST;
namespace Cygni.DataTypes
{
	public interface IFunction
	{
		DynValue DynInvoke (DynValue[] args);
		Func<DynValue[],DynValue> AsDelegate();
		DynValue DynEval (ASTNode[] args, IScope scope);
	}
}

