using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Errors;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.AST.Optimizers;

namespace Cygni.AST
{
	/// <summary>
	/// Description of DefFuncEx.
	/// </summary>
	public class DefFuncEx:ASTNode
	{
		readonly string name;
		readonly BlockEx body;
		readonly NameEx[] parameters;
		int size;

		public string Name{ get { return this.name; } }

		public BlockEx Body{ get { return body; } }

		public NameEx[] Parameters{ get { return parameters; } }

		internal int Size{ get { return this.size; } set { this.size = value; } }

		public override NodeType type { get { return NodeType.DefFunc; } }

		public DefFuncEx (string name, NameEx[] parameters, BlockEx body)
		{
			this.name = name;
			this.parameters = parameters;
			this.body = body;
		}

		public override DynValue Eval (IScope scope)
		{
			if (scope.type == ScopeType.ResizableArray) {
				ResizableArrayScope GlobalScope = scope as ResizableArrayScope;
				if (GlobalScope.HasName (name)) {
					GlobalScope.Put (name, DynValue.Nil);
				}
				Symbols symbols = GlobalScope.GetSymbols ();
				LookUpVisitor visitor = new LookUpVisitor (symbols);
				this.Accept (visitor);

				ArrayScope arrayScope = new ArrayScope (this.name, new DynValue[this.size], scope);
				DynValue func = new Function (parameters.Length, body, arrayScope);
				return scope.Put (name, func);

			} else if (scope.type == ScopeType.Class) {
				ArrayScope arrayScope = new ArrayScope (this.name, new DynValue[this.size], scope);
				DynValue func = new Function (parameters.Length, body, arrayScope);

				return scope.Put (name, func);
			} else {
				throw new RuntimeException("Function '{0}' can only be declared in global scope or in a class.", name);
			}
		}

		public override string ToString ()
		{
			return string.Concat (" def ", name, "(", string.Join<NameEx> (", ", parameters), ")", body);
		}

		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}
