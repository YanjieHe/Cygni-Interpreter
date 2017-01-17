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
	public class DefClosureEx: DefFuncEx 
	{
		public DefClosureEx (NameEx[] parameters, ASTNode body)
            : base(string.Empty, parameters, body)
		{
		}

		public override NodeType type{ get { return NodeType.DefClosure; } }

		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}

		public override DynValue Eval (IScope scope)
		{
			if (scope.type == ScopeType.Module) {
				ModuleScope GlobalScope = scope as ModuleScope;
				Symbols symbols = GlobalScope.GetSymbols ();
				LookUpVisitor visitor = new LookUpVisitor (symbols);
				this.Accept (visitor);
				FunctionScope arrayScope = new FunctionScope ("Anonymous Function", new DynValue[this.size], scope);

				DynValue func = new Closure (parameters.Length, body, arrayScope);
				return func;
			} else {
				FunctionScope arrayScope = new FunctionScope ("Anonymous Function", new DynValue[this.size], scope);
				DynValue func = new Closure (parameters.Length, body, arrayScope);
				return func;
			}
		}
	}
}

