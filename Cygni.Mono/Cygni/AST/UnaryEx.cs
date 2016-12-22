using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.DataTypes.Interfaces;

namespace Cygni.AST
{
	/// <summary>
	/// Description of UnaryEx.
	/// </summary>
	public class UnaryEx:ASTNode
	{
		readonly ASTNode operand;
		readonly UnaryOp op;

		public ASTNode Operand{ get { return operand; } }

		public UnaryOp Op{ get { return op; } }

		public override NodeType type { get { return NodeType.Unary; } }

		public UnaryEx (UnaryOp op, ASTNode operand)
		{
			this.op = op;
			this.operand = operand;
		}

		public override DynValue Eval (IScope scope)
		{
			DynValue obj = Operand.Eval (scope);
			switch (op) {
			case UnaryOp.Plus:
				{
					if (obj.type == DataType.Integer) {
						return new DynValue (DataType.Integer, +(long)obj.Value);
					}
					if (obj.type == DataType.Number) {
						return new DynValue (DataType.Number, +(double)obj.Value);
					} else {
						return ((IComputable)obj.Value).UnaryPlus ();
					}
				}
			case UnaryOp.Minus:
				{
					if (obj.type == DataType.Integer) {
						return new DynValue (DataType.Integer, -(long)obj.Value);
					}
					if (obj.type == DataType.Number) {
						return new DynValue (DataType.Number, -(double)obj.Value);
					} else {
						return ((IComputable)obj.Value).UnaryPlus ();
					}
				}
			default: /* UnaryOp.Not */
				return obj.AsBoolean () ? DynValue.False : DynValue.True;
			}
		}

		public override string ToString ()
		{
			switch (op) {
			case UnaryOp.Plus:
				return "+" + Operand;
			case UnaryOp.Minus:
				return "-" + Operand;
			default: /* UnaryOp.Not */
				return " not " + Operand;
			}
		}

		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
	}

	public enum UnaryOp:byte
	{
		Plus,
		Minus,
		Not
	}
}
