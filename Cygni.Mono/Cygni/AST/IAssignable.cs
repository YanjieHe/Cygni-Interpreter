using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST.Scopes;

namespace Cygni.AST
{
	public interface IAssignable
	{
		DynValue Assign (DynValue value,IScope scope);
	}
}

