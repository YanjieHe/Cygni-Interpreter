using System;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.Errors;
namespace Cygni.AST
{
	public class GlobalEx:ASTNode
	{
		string name;
		ASTNode value;
		public string Name { get { return this.name; } }
		public ASTNode Value { get { return this.value; } }

		public override NodeType type {
			get {
				return NodeType.Global;
			}
		}

		public GlobalEx (string name, ASTNode value)
		{
			this.name = name;
			this.value = value;
		}
		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
		public override DynValue Eval (IScope scope)
		{
			DynValue v = value.Eval (scope);
			while (scope.type != ScopeType.Basic) {
				scope = scope.Parent;
			}
			return scope.Put (name, v);
		}
	}
}

