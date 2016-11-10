using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
namespace Cygni.AST
{
	/// <summary>
	/// Description of BinaryEx.
	/// </summary>
	public class BinaryEx:ASTNode
	{
		ASTNode left;
		ASTNode right;

		public ASTNode Left{ get { return left; } }

		public ASTNode Right{ get { return right; } }

		BinaryOp op;

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
			if (op == BinaryOp.Assign) {
				return (left as IAssignable).Assign (right.Eval (scope), scope);
			}
			var _left = left.Eval (scope);
			//var _right = right.Eval(scope);
			switch (op) {
			case BinaryOp.Add:
				{
					if (_left.type == DataType.Number)
						return new DynValue (DataType.Number, (double)_left.Value + (double)right.Eval (scope).Value);
					return ((IComputable)_left.Value).Add (right.Eval (scope));
				}
			case BinaryOp.Sub:
				{
					if (_left.type == DataType.Number)
						return new DynValue (DataType.Number, (double)_left.Value - (double)right.Eval (scope).Value);
					return ((IComputable)_left.Value).Subtract (right.Eval (scope));
				}
			case BinaryOp.Mul:
				{
					if (_left.type == DataType.Number)
						return new DynValue (DataType.Number, (double)_left.Value * (double)right.Eval (scope).Value);
					return ((IComputable)_left.Value).Subtract (right.Eval (scope));
				}
			case BinaryOp.Div:
				{
					if (_left.type == DataType.Number)
						return new DynValue (DataType.Number, (double)_left.Value / (double)right.Eval (scope).Value);
					return ((IComputable)_left.Value).Subtract (right.Eval (scope));
				}
			case BinaryOp.Mod:
				{
					if (_left.type == DataType.Number)
						return new DynValue (DataType.Number, (double)_left.Value % (double)right.Eval (scope).Value);
					return ((IComputable)_left.Value).Subtract (right.Eval (scope));
				}
			case BinaryOp.Pow:
				{
					if (_left.type == DataType.Number)
						return new DynValue (DataType.Number, Math.Pow ((double)_left.Value, (double)right.Eval (scope).Value));
					return ((IComputable)_left.Value).Subtract (right.Eval (scope));
				}
			case BinaryOp.And:
				if ((bool)_left.Value)
					return (bool)right.Eval (scope).Value;
				return DynValue.False;/* shortcut evaluate*/
			case BinaryOp.Or:
				if ((bool)_left.Value)
					return DynValue.True;/* shortcut evaluate*/
				return (bool)right.Eval (scope).Value;
			case BinaryOp.Less:
				{
					if (_left.type == DataType.Number)
						return 	(double)_left.Value < (double)right.Eval (scope).Value ? DynValue.True : DynValue.False;
					return _left.CompareTo (right.Eval (scope)) < 0;
				}
			case BinaryOp.Greater:
				{
					if (_left.type == DataType.Number)
						return 	(double)_left.Value > (double)right.Eval (scope).Value ? DynValue.True : DynValue.False;
					return _left.CompareTo (right.Eval (scope)) > 0;
				}
			case BinaryOp.LessOrEqual:
				{
					if (_left.type == DataType.Number)
						return 	(double)_left.Value <= (double)right.Eval (scope).Value ? DynValue.True : DynValue.False;
					return _left.CompareTo (right.Eval (scope)) <= 0;
				}
			case BinaryOp.GreaterOrEqual:
				{
					if (_left.type == DataType.Number)
						return 	(double)_left.Value >= (double)right.Eval (scope).Value ? DynValue.True : DynValue.False;
					return _left.CompareTo (right.Eval (scope)) >= 0;
				}
			case BinaryOp.Equal:
				return _left.Equals (right.Eval (scope)) ? DynValue.True : DynValue.False;
			default: /* BinaryOp.NotEqual */
				return _left.Equals (right.Eval (scope)) ? DynValue.False : DynValue.True;
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
			case BinaryOp.Assign:
				return string.Concat ("(", left, "=", right, ")");
			default:
				throw new NotSupportedException (op.ToString ());
			}
		}

		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
		internal void SetLeft(ASTNode left){
			this.left = left;
		}
		internal void SetRight(ASTNode right){
			this.right = right;
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
		Assign,
	}
}
