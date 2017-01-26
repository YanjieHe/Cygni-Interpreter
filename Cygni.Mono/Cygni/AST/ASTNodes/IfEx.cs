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
	/// Description of IfEx.
	/// </summary>
	public class IfEx:ASTNode
	{
		ASTNode condition;
		ASTNode ifTrue;
		ASTNode ifFalse;
		public ASTNode Condition { get { return condition; } }
		public ASTNode IfTrue { get { return ifTrue; } }
		public ASTNode IfFalse { get { return ifFalse; } }

        public bool HasElsePart { get { return this.ifFalse != null; } }

		public override NodeType type { get { return NodeType.If; } }
		
		public IfEx(ASTNode condition, ASTNode ifTrue, ASTNode ifFalse)
		{
			this.condition = condition;
			this.ifTrue = ifTrue;
			this.ifFalse = ifFalse;
		}
		public override DynValue Eval(IScope scope)
		{
			if ((bool)condition.Eval(scope).Value)
				return ifTrue.Eval(scope);
			return ifFalse == null ? DynValue.Nil : ifFalse.Eval(scope);
		}

		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}
