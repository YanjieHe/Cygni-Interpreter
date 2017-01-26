using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;

namespace Cygni.AST
{
	/// <summary>
	/// Description of ReturnEx.
	/// </summary>
	public class ReturnEx:ASTNode
	{
		readonly ASTNode value;
		public ASTNode Value{ get { return value; } }

		public override NodeType type { get { return NodeType.Return; } }
		public ReturnEx(ASTNode value)
		{
			this.value = value;
		}
		public override DynValue Eval(IScope scope)
		{
			return new DynValue (DataType.Return, value.Eval (scope));
		}

		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}
