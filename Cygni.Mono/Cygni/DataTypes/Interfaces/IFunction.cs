using System;
using System.Collections.Generic;
using Cygni.AST.Scopes;
using Cygni.AST;
namespace Cygni.DataTypes.Interfaces
{
	public interface IFunction
	{
		DynValue DynInvoke (DynValue[] args);
		DynValue DynEval (ASTNode[] args, IScope scope);
	}
}

