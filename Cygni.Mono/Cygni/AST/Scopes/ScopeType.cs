using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Errors;
using System.Collections.Generic;
using System.Linq;
namespace Cygni.AST.Scopes
{
	public enum ScopeType
	{
		Basic,
		BuiltIn,
		Nested,
		Array,
		ResizableArray,
		Class,
	}
}

