using System;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.Errors;

namespace Cygni.AST
{
	public class SetEx:ASTNode
	{
		ASTNode[] targets;
		public ASTNode[] Targets{ get { return this.targets; } }
		readonly ASTNode[] values;
		public ASTNode[] Values{ get { return this.values; } }

		public SetEx (ASTNode[] targets, ASTNode[] values)
		{
			this.targets = targets;
			this.values = values;
		}
		public override NodeType type {
			get {
				return NodeType.Set;
			}
		}
		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
		public override DynValue Eval (IScope scope)
		{
			for (int i = 0; i < targets.Length; i++) {
				IAssignable t = targets[i] as IAssignable;
				if (t == null) {
					throw new RuntimeException("left side '{0}' cannot be assigned to.", targets[i]);
				} else {
					t.Assign(values[i].Eval(scope), scope);
				}
			}
			return DynValue.Nil;
		}
	}
}

