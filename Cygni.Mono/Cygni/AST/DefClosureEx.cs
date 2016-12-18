using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.AST.Optimizers;

namespace Cygni.AST
{
	public class DefClosureEx: ASTNode
	{
		readonly NameEx[] parameters;
		readonly ASTNode body;
		int size;
		internal int Size { get{ return this.size; } set { this.size = value; } }

		public NameEx[] Parameters { get { return this.parameters; } }
		public ASTNode Body { get { return this.body; } }
		public DefClosureEx (NameEx[] parameters, ASTNode body)
		{
			this.parameters = parameters;
			this.body = body;
			this.size = -1;
		}
		public override NodeType type{ get { return NodeType.DefClosure; } }

		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit(this);
		}
		public override DynValue Eval (IScope scope)
		{
			if (scope.type == ScopeType.ResizableArray) {
				ResizableArrayScope GlobalScope = scope as ResizableArrayScope;
				Symbols symbols = GlobalScope.GetSymbols ();
				LookUpVisitor visitor = new LookUpVisitor (symbols);
				this.Accept (visitor);
				ArrayScope arrayScope = new ArrayScope (new DynValue[this.size], scope);

				DynValue func = DynValue.FromClosure (
						new Closure (parameters.Length, body, arrayScope));
				return func;
			} else {
				ArrayScope arrayScope = new ArrayScope (new DynValue[this.size], scope);
				DynValue func = DynValue.FromClosure (
						new Closure (parameters.Length, body, arrayScope));
				return func;
			}
		}
	}
}

