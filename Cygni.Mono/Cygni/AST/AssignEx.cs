using System;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.Errors;
namespace Cygni.AST
{
	public class AssignEx: ASTNode
	{
		ASTNode target;
		readonly ASTNode value;
		public ASTNode Target { get {return this.target;} }
		public ASTNode Value { get {return this.value;} }
		public AssignEx (ASTNode target, ASTNode value)
		{
			this.target = target;
			this.value = value;
		}

		public override NodeType type { get{ return NodeType.Assign;}}
		public override DynValue Eval (IScope scope) {
			IAssignable left = target as IAssignable;
			if (left == null)
				throw new RuntimeException("Left side {0} is not assignable.", left); 
			return left.Assign (value.Eval (scope), scope);
		}
		public void SetTarget(ASTNode target){
			this.target = target;
		}

		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
		public override string ToString() {
			return string.Concat ("(", target, "=", value, ")");
		}

	}
}

