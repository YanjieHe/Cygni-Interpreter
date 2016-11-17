using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Errors;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;

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
			var f = func.Eval(scope);
			int n = arguments.Length;
			var args = new DynValue[n];
			
			for (int i = 0; i < n; i++)
				args[i] = arguments[i].Eval(scope);
			
			return f.As<IFunction> ().DynInvoke (args);
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
