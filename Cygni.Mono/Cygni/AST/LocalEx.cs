using System;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.Errors;
using Cygni.AST.Interfaces;

namespace Cygni.AST
{
	public class LocalEx:ASTNode
	{
		NameEx[] variables;
		ASTNode[] values;

		public NameEx[] Variables { get { return this.variables; } }

		public ASTNode[] Values { get { return this.values; } }

		public override NodeType type {
			get {
				return NodeType.Local;
			}
		}

		public LocalEx (NameEx[] variables, ASTNode[] values)
		{
			this.variables = variables;
			this.values = values;
		}

		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}

		public override DynValue Eval (IScope scope)
		{
			for (int i = 0; i < values.Length; i++) {
				this.variables [i].Assign (this.values [i].Eval (scope), scope);
			}
			return DynValue.Nil;
		}
	}
}

