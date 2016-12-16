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
		string[] names;
		ASTNode[] values;
		public string[] Names { get { return this.names; } }
		public ASTNode[] Values { get { return this.values; } }

		public override NodeType type {
			get {
				return NodeType.Global;
			}
		}

		public GlobalEx (string[] names, ASTNode[] values)
		{
			this.names = names;
			this.values = values;
		}
		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
		public override DynValue Eval (IScope scope)
		{
			while (scope.type != ScopeType.Basic) {
				scope = scope.Parent;
			}
			for (int i = 0; i < names.Length; i++) {
				scope.Put (names[i], values[i].Eval(scope));
			}
			return DynValue.Nil;
		}
	}
}

