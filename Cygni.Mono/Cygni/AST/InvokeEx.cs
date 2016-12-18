using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Errors;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.AST.Interfaces;
using Cygni.DataTypes.Interfaces;

namespace Cygni.AST
{
	/// <summary>
	/// Description of InvokeEx.
	/// </summary>
	public class InvokeEx:ASTNode
	{
		readonly ASTNode func;
		public ASTNode Func{ get { return func; } }

		readonly ASTNode[] arguments;
		public ASTNode[] Arguments{ get { return arguments; } }

		public override  NodeType type { get { return NodeType.Invoke; } }
		
		public InvokeEx(ASTNode func, ICollection<ASTNode> arguments)
		{
			this.func = func;
			this.arguments = new ASTNode[arguments.Count];
			arguments.CopyTo (this.arguments, 0);
		}
		
		public override DynValue Eval(IScope scope)
		{
			IFunction f = func.Eval(scope).Value as IFunction;
			if (f == null) {
				throw new RuntimeException("'{0}' is not a function.", func);
			}
			return f.DynEval (arguments, scope);
		}
		public override string ToString()
		{
			return string.Concat(func, "(", string.Join(", ", arguments.Select(i=>i.ToString())), ")");
		}
		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}
