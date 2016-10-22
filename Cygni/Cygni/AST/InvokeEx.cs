using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Errors;
namespace Cygni.AST
{
	/// <summary>
	/// Description of InvokeEx.
	/// </summary>
	public class InvokeEx:ASTNode
	{
		ASTNode func;
		ASTNode[] arguments;
		int nArgs;
		public override  NodeType type { get { return NodeType.Invoke; } }
		
		public InvokeEx(ASTNode func, ICollection<ASTNode>arguments)
		{
			this.func = func;
			this.arguments = new ASTNode[arguments.Count];
			nArgs = arguments.Count;
			arguments.CopyTo (this.arguments, 0);
		}
		
		public override DynValue Eval(IScope scope)
		{
			var f = func.Eval(scope);
			var args = new DynValue[nArgs];
			
			for (int i = 0; i < nArgs; i++)
				args[i] = arguments[i].Eval(scope);

			return f.As<IFunction> ().DynInvoke (args);
			/*switch (f.type) {
				case DataType.Function:
					return f.As<Function>().Update(args).Invoke();
				case DataType.NativeFunction:
					return f.As<NativeFunction>().Invoke(args);
				case DataType.Class:
					return DynValue.FromClass(f.As<ClassInfo>().Init(args));
				default:
					throw new RuntimeException("Error function type '{0}'.", f.type);
			}*/
		}
		public override string ToString()
		{
			return string.Concat(func, "(", string.Join(", ", arguments.Select(i=>i.ToString())), ")");
		}
	}
}
