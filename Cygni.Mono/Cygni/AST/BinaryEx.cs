using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.Errors;
namespace Cygni.AST
{
	/// <summary>
	/// Description of BinaryEx.
	/// </summary>
	public class BinaryEx:ASTNode
	{
		readonly ASTNode left;
		readonly ASTNode right;

		public ASTNode Left{ get { return left; } }

		public ASTNode Right{ get { return right; } }

		readonly BinaryOp op;

		public BinaryOp Op{ get { return op; } }

		public override NodeType type { get { return NodeType.Binary; } }

		public BinaryEx (BinaryOp op, ASTNode left, ASTNode right)
		{
			this.op = op;
			this.left = left;
			this.right = right;
		}

		public override DynValue Eval (IScope scope)
		{
			var lvalue = left.Eval (scope);
			switch (op) {
				case BinaryOp.Add:
					if (lvalue.type == DataType.Number) {
						return new DynValue (DataType.Number, 
								(double)lvalue.Value + (double)right.Eval (scope).Value);
					} else {
						return ((IComputable)lvalue.Value).Add (right.Eval (scope));
					}
				case BinaryOp.Sub:
					if (lvalue.type == DataType.Number) {
						return new DynValue (DataType.Number, 
								(double)lvalue.Value - (double)right.Eval (scope).Value);
					} else {
						return ((IComputable)lvalue.Value).Add (right.Eval (scope));
					}
				case BinaryOp.Mul:
					if (lvalue.type == DataType.Number) {
						return new DynValue (DataType.Number, 
								(double)lvalue.Value * (double)right.Eval (scope).Value);
					} else {
						return ((IComputable)lvalue.Value).Add (right.Eval (scope));
					}
				case BinaryOp.Div:
					if (lvalue.type == DataType.Number) {
						return new DynValue (DataType.Number, 
								(double)lvalue.Value / (double)right.Eval (scope).Value);
					} else {
						return ((IComputable)lvalue.Value).Add (right.Eval (scope));
					}
				case BinaryOp.Mod:
					if (lvalue.type == DataType.Number) {
						return new DynValue (DataType.Number, 
								(double)lvalue.Value % (double)right.Eval (scope).Value);
					} else {
						return ((IComputable)lvalue.Value).Add (right.Eval (scope));
					}
				case BinaryOp.Pow:
					if (lvalue.type == DataType.Number) {
						return new DynValue (DataType.Number, 
								Math.Pow ((double)lvalue.Value ,(double)right.Eval (scope).Value));
					} else {
						return ((IComputable)lvalue.Value).Add (right.Eval (scope));
					}
				case BinaryOp.And:
					if ((bool)lvalue.Value) {
						return (bool)right.Eval (scope).Value;
					} else {
						return DynValue.False;/* shortcut evaluate*/
					}
				case BinaryOp.Or:
					if ((bool)lvalue.Value) {
						return DynValue.True;/* shortcut evaluate*/
					} else {
						return (bool)right.Eval (scope).Value;
					}
				case BinaryOp.Less:
					if (lvalue.type == DataType.Number) {
						return 	(double)lvalue.Value < (double)right.Eval (scope).Value 
							? DynValue.True : DynValue.False;
					} else {
						return lvalue.CompareTo (right.Eval (scope)) < 0;
					}
				case BinaryOp.Greater:
					if (lvalue.type == DataType.Number) {
						return 	(double)lvalue.Value > (double)right.Eval (scope).Value 
							? DynValue.True : DynValue.False;
					} else {
						return lvalue.CompareTo (right.Eval (scope)) > 0;
					}
				case BinaryOp.LessOrEqual:
					if (lvalue.type == DataType.Number) {
						return 	(double)lvalue.Value <= (double)right.Eval (scope).Value 
							? DynValue.True : DynValue.False;
					} else {
						return lvalue.CompareTo (right.Eval (scope)) <= 0;
					}
				case BinaryOp.GreaterOrEqual:
					if (lvalue.type == DataType.Number) {
						return 	(double)lvalue.Value >= (double)right.Eval (scope).Value 
							? DynValue.True : DynValue.False;
					} else {
						return lvalue.CompareTo (right.Eval (scope)) >= 0;
					}
				case BinaryOp.Equal:
					return lvalue.Equals (right.Eval (scope)) ? DynValue.True : DynValue.False;
				default:/* BinaryOp.NotEqual */
					return lvalue.Equals (right.Eval (scope)) ? DynValue.False : DynValue.True;

			}
		}

		public override string ToString ()
		{
			switch (op) {
				case BinaryOp.Add:
					return string.Concat ("(", left, "+", right, ")");
				case BinaryOp.Sub:
					return string.Concat ("(", left, "-", right, ")");
				case BinaryOp.Mul:
					return string.Concat ("(", left, "*", right, ")");
				case BinaryOp.Div:
					return string.Concat ("(", left, "/", right, ")");
				case BinaryOp.Mod:
					return string.Concat ("(", left, "%", right, ")");
				case BinaryOp.Pow:
					return string.Concat ("(", left, "^", right, ")");
				case BinaryOp.And:
					return string.Concat ("(", left, " and ", right, ")");
				case BinaryOp.Or:
					return string.Concat ("(", left, " or ", right, ")");
				case BinaryOp.Less:
					return string.Concat ("(", left, "<", right, ")");
				case BinaryOp.Greater:
					return string.Concat ("(", left, ">", right, ")");
				case BinaryOp.LessOrEqual:
					return string.Concat ("(", left, "<=", right, ")");
				case BinaryOp.GreaterOrEqual:
					return string.Concat ("(", left, ">=", right, ")");
				case BinaryOp.Equal:
					return string.Concat ("(", left, "==", right, ")");
				case BinaryOp.NotEqual:
					return string.Concat ("(", left, "!=", right, ")");
				default:
					throw new NotSupportedException (op.ToString ());
			}
		}

		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}

	}

	public enum BinaryOp:byte
	{
		Add,
		Sub,
		Mul,
		Div,
		Mod,
		Pow,
		And,
		Or,
		Equal,
		NotEqual,
		Less,
		Greater,
		LessOrEqual,
		GreaterOrEqual,
	}
}
