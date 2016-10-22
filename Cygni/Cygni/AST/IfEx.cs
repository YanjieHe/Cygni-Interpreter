using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
namespace Cygni.AST
{
	/// <summary>
	/// Description of IfEx.
	/// </summary>
	public class IfEx:ASTNode
	{
		ASTNode condition;
		ASTNode IfTrue;
		ASTNode IfFalse;
		public  override NodeType type { get { return NodeType.If; } }
		
		public IfEx(ASTNode condition, ASTNode IfTrue, ASTNode IfFalse)
		{
			this.condition = condition;
			this.IfTrue = IfTrue;
			this.IfFalse = IfFalse;
		}
		public override DynValue Eval(IScope scope)
		{
			if ((bool)condition.Eval(scope).Value)
				return IfTrue.Eval(scope);
			return IfFalse == null ? DynValue.Null : IfFalse.Eval(scope);
		}
		public override string ToString()
		{
			if (IfFalse == null) {
				return string.Concat(" if ", condition, IfTrue);
			} else {
				return string.Concat(" if ", condition, IfTrue, " else ", IfFalse);
			}
		}
	}
}
