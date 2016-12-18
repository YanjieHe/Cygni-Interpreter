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
	/// <summary>
	/// Description of DefFuncEx.
	/// </summary>
	public class DefFuncEx:ASTNode
	{
		string name;
		BlockEx body;

		public BlockEx Body{ get { return body; } }

		string[] parameters;

		public string[] Parameters{ get { return parameters; } }

		public override  NodeType type { get { return NodeType.DefFunc; } }

		public string Name{ get { return this.name; } }

		public DefFuncEx (string name, string[] parameters, BlockEx body)
		{
			this.name = name;
			this.parameters = parameters;
			this.body = body;
		}

		public override DynValue Eval (IScope scope)
		{
			if (!scope.HasName (name)) {
				scope.Put (name, DynValue.Nil);
			}
			if (scope.type != ScopeType.ResizableArray) {
				throw new Exception ("Scope Error");
			} else {
				ResizableArrayScope GlobalScope = scope as ResizableArrayScope;
				Symbols symbols = new Symbols (GlobalScope.GetSymbols ());
				LookUpVisitor visitor = new LookUpVisitor (symbols);
				this.Accept (visitor);
				ArrayScope arrayScope = new ArrayScope (new DynValue[symbols.Count], scope);

				DynValue func = DynValue.FromFunction (
					                new Function (name, parameters.Length, body, arrayScope));
				return scope.Put (name, func);
			}
		
		}

		public override string ToString ()
		{
			return string.Concat (" def ", name, "(", string.Join (", ", parameters), ")", body);
		}

		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}
