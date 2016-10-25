using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST.Scopes;

namespace Cygni.AST
{
	/// <summary>
	/// Description of UnaryEx.
	/// </summary>
	public class UnaryEx:ASTNode,ISymbolLookUp
	{
		ASTNode operand;
		UnaryOp op;
		public ASTNode Operand{ get { return operand; } }
		public UnaryOp Op{ get { return op; } }
		public  override NodeType type { get { return NodeType.Unary; } }
		
		public UnaryEx(UnaryOp op, ASTNode operand)
		{
			this.op = op;
			this.operand = operand;
		}
		public override DynValue Eval(IScope scope)
		{
			var obj = Operand.Eval(scope);
			switch (op) {
				case UnaryOp.Plus:
					return obj.UnaryPlus();
				case UnaryOp.Minus:
					return obj.UnaryMinus();
				default: /* UnaryOp.Not */
					return (bool)obj.Value ? DynValue.False : DynValue.True;
			}
		}
		public override string ToString()
		{
			switch (op) {
				case UnaryOp.Plus:
					return "+" + Operand;
				case UnaryOp.Minus:
					return "-" + Operand;
				case UnaryOp.Not:
					return " not " + Operand;
				default:
					throw new NotSupportedException(op.ToString());
			}
		}
		public void LookUpForLocalVariable (List<NameEx>names)
		{
			if (operand is ISymbolLookUp)
				(operand as ISymbolLookUp).LookUpForLocalVariable (names);
		}
	}
	public enum UnaryOp:byte
	{
		Plus,
		Minus,
		Not
	}
}
