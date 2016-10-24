using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST.Scopes;

namespace Cygni.AST
{
	/// <summary>
	/// Description of ReturnEx.
	/// </summary>
	public class ReturnEx:ASTNode
	{
		ASTNode value;
		public override NodeType type { get { return NodeType.Return; } }
		public ReturnEx(ASTNode value)
		{
			this.value = value;
		}
		public override DynValue Eval(IScope scope)
		{
			return DynValue.Return(value.Eval(scope));
		}
	}
}
