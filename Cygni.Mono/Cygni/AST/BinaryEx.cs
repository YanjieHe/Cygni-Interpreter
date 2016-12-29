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
using Cygni.Libraries;
using Cygni.DataTypes.Interfaces;

namespace Cygni.AST
{
	/// <summary>
	/// Description of BinaryEx.
	/// </summary>
	public class BinaryEx:ASTNode
	{
		readonly ASTNode left;
		readonly ASTNode right;

		readonly BinaryOp op;

		public BinaryOp Op{ get { return op; } }

		public ASTNode Left{ get { return left; } }

		public ASTNode Right{ get { return right; } }

		public override NodeType type { get { return NodeType.Binary; } }

		public BinaryEx (BinaryOp op, ASTNode left, ASTNode right)
		{
			this.op = op;
			this.left = left;
			this.right = right;
		}

		public override DynValue Eval (IScope scope)
		{
			DynValue lvalue = left.Eval (scope);
			DynValue rvalue;
			switch (op) {
			case BinaryOp.Add:
				{

					rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {

						if (rvalue.type == DataType.Integer) { /* integer + integer */
							return new DynValue (DataType.Integer, (long)lvalue.Value + (long)rvalue.Value);
						} else if (rvalue.type == DataType.Number) { /* integer + number */
							return new DynValue (DataType.Number, (double)(long)lvalue.Value + (double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}

					} else if (lvalue.type == DataType.Number) {

						if (rvalue.type == DataType.Integer) { /* number + integer */
							return new DynValue (DataType.Number, (double)lvalue.Value + (long)rvalue.Value);
						} else if (rvalue.type == DataType.Number) { /* number + number */
							return new DynValue (DataType.Number, (double)lvalue.Value + (double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}

					} else {
						return lvalue.As<IComputable> ().Add (rvalue);
					}

				}
			case BinaryOp.Sub:
				{

					rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {

						if (rvalue.type == DataType.Integer) { /* integer - integer */
							return new DynValue (DataType.Integer, (long)lvalue.Value - (long)rvalue.Value);
						} else if (rvalue.type == DataType.Number) { /* integer - number */
							return new DynValue (DataType.Number, (double)(long)lvalue.Value - (double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}

					} else if (lvalue.type == DataType.Number) {

						if (rvalue.type == DataType.Integer) { /* number - integer */
							return new DynValue (DataType.Number, (double)lvalue.Value - (long)rvalue.Value);
						} else if (rvalue.type == DataType.Number) { /* number - number */
							return new DynValue (DataType.Number, (double)lvalue.Value - (double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}

					} else {
						return lvalue.As<IComputable> ().Subtract (rvalue);
					}

				}
			case BinaryOp.Mul:
				{

					rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {

						if (rvalue.type == DataType.Integer) { /* integer * integer */
							return new DynValue (DataType.Integer, (long)lvalue.Value * (long)rvalue.Value);
						} else if (rvalue.type == DataType.Number) { /* integer * number */
							return new DynValue (DataType.Number, (double)(long)lvalue.Value * (double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}

					} else if (lvalue.type == DataType.Number) {

						if (rvalue.type == DataType.Integer) { /* number * integer */
							return new DynValue (DataType.Number, (double)lvalue.Value * (long)rvalue.Value);
						} else if (rvalue.type == DataType.Number) { /* number * number */
							return new DynValue (DataType.Number, (double)lvalue.Value * (double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}

					} else {
						return lvalue.As<IComputable> ().Multiply (rvalue);
					}

				}
			case BinaryOp.Div:
				{
					rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						if (rvalue.type == DataType.Integer) { /* integer / integer */
							return new DynValue (DataType.Number, ((double)(long)lvalue.Value) / ((double)(long)rvalue.Value));
						} else if (rvalue.type == DataType.Number) { /* integer / number */
							return new DynValue (DataType.Number, ((double)(long)lvalue.Value / (double)rvalue.Value));
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) {
						if (rvalue.type == DataType.Integer) { /* number / integer */
							return new DynValue (DataType.Number, ((double)lvalue.Value) / ((double)(long)rvalue.Value));
						} else if (rvalue.type == DataType.Number) { /* number / number */
							return new DynValue (DataType.Number, ((double)lvalue.Value) / (double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.As<IComputable> ().Divide (rvalue);
					}
				}
			case BinaryOp.IntDiv:
				{
					rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						if (rvalue.type == DataType.Integer) { /* integer // integer */
							return new DynValue (DataType.Integer, ((long)lvalue.Value) / ((long)rvalue.Value));
						} else if (rvalue.type == DataType.Number) { /* integer // number */
							return new DynValue (DataType.Integer, ((long)lvalue.Value) / (long)(double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) {
						if (rvalue.type == DataType.Integer) { /* number // integer */
							return new DynValue (DataType.Integer, ((long)(double)lvalue.Value) / ((long)rvalue.Value));
						} else if (rvalue.type == DataType.Number) { /* number // number */
							return new DynValue (DataType.Integer, ((long)(double)lvalue.Value) / (long)(double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.As<IComputable> ().Divide (rvalue);
					}
				}
			case BinaryOp.Mod:
				{
					rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						if (rvalue.type == DataType.Integer) { /* integer % integer */
							return new DynValue (DataType.Integer, (long)lvalue.Value % (long)rvalue.Value);
						} else if (rvalue.type == DataType.Number) { /* integer % number */
							return new DynValue (DataType.Number, ((double)(long)lvalue.Value) % (double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) {
						if (rvalue.type == DataType.Integer) { /* number % integer */
							return new DynValue (DataType.Number, ((double)lvalue.Value) % ((double)(long)rvalue.Value));
						} else if (rvalue.type == DataType.Number) { /* number % number */
							return new DynValue (DataType.Number, ((double)lvalue.Value) % (double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.As<IComputable> ().Modulo (rvalue);
					}
				}
			case BinaryOp.Pow:
				{
					rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						if (rvalue.type == DataType.Integer) { /* integer ^ integer */
							return new DynValue (DataType.Integer, MathLib.IntegerPow ((long)lvalue.Value, (int)(long)rvalue.Value));
						} else if (rvalue.type == DataType.Number) { /* integer ^ number */
							return new DynValue (DataType.Number, Math.Pow ((double)lvalue.Value, (double)rvalue.Value));
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) {
						if (rvalue.type == DataType.Integer) { /* number ^ integer */
							return new DynValue (DataType.Number, Math.Pow ((double)lvalue.Value, (long)rvalue.Value));
						} else if (rvalue.type == DataType.Number) { /* number ^ number */
							return new DynValue (DataType.Number, Math.Pow ((double)lvalue.Value, (double)rvalue.Value));
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.As<IComputable> ().Power (rvalue);
					}

				}
			case BinaryOp.Concatenate:
				{
					rvalue = right.Eval (scope);
					return new DynValue (DataType.String, lvalue.Value.ToString () + rvalue.Value.ToString ());
				}
			case BinaryOp.And:/* shortcut evaluate*/
				{
					if (lvalue.type == DataType.Boolean) {
						if ((bool)lvalue.Value) {
							rvalue = right.Eval (scope);	
							if (rvalue.type == DataType.Boolean) {
								return (bool)rvalue.Value ? DynValue.True : DynValue.False;
							} else {
								goto BinaryOperationError;
							}
						} else {
							return DynValue.False;
						}
					} else {
						goto BinaryOperationError;
					}
				}
			case BinaryOp.Or:
				if (lvalue.type == DataType.Boolean) {
					if ((bool)lvalue.Value) {
						return DynValue.True;/* shortcut evaluate*/
					} else {
						rvalue = right.Eval (scope);
						if (rvalue.type == DataType.Boolean) {
							return (bool)rvalue.Value ? DynValue.True : DynValue.False;
						} else {
							goto BinaryOperationError;
						}
					}
				} else {
					goto BinaryOperationError;
				}

			case BinaryOp.Less:
				{
					rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						if (rvalue.type == DataType.Integer) { /* integer < integer */
							return  (long)lvalue.Value < (long)rvalue.Value ? DynValue.True : DynValue.False;
						} else if (rvalue.type == DataType.Number) { /* integer < number */
							return ((double)(long)lvalue.Value) < (double)rvalue.Value ? DynValue.True : DynValue.False;
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) { /* number < integer */
						if (rvalue.type == DataType.Integer) {
							return  ((double)lvalue.Value) < ((double)(long)rvalue.Value) ? DynValue.True : DynValue.False;
						} else if (rvalue.type == DataType.Number) { /* number < number */
							return  ((double)lvalue.Value) < ((double)rvalue.Value) ? DynValue.True : DynValue.False;
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.As<IComparable<DynValue>> ().CompareTo (rvalue) < 0;
					}
				}

			case BinaryOp.Greater:
				{
					rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						if (rvalue.type == DataType.Integer) { /* integer > integer */
							return  (long)lvalue.Value > (long)rvalue.Value ? DynValue.True : DynValue.False;
						} else if (rvalue.type == DataType.Number) { /* integer > number */
							return ((double)(long)lvalue.Value) > (double)rvalue.Value ? DynValue.True : DynValue.False;
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) { /* number > integer */
						if (rvalue.type == DataType.Integer) {
							return  ((double)lvalue.Value) > ((double)(long)rvalue.Value) ? DynValue.True : DynValue.False;
						} else if (rvalue.type == DataType.Number) { /* number > number */
							return  ((double)lvalue.Value) > ((double)rvalue.Value) ? DynValue.True : DynValue.False;
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.As<IComparable<DynValue>> ().CompareTo (rvalue) > 0;
					}
				}
			case BinaryOp.LessOrEqual:
				{
					rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						if (rvalue.type == DataType.Integer) { /* integer <= integer */
							return  (long)lvalue.Value <= (long)rvalue.Value ? DynValue.True : DynValue.False;
						} else if (rvalue.type == DataType.Number) { /* integer <= number */
							return ((double)(long)lvalue.Value) <= (double)rvalue.Value ? DynValue.True : DynValue.False;
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) { /* number <= integer */
						if (rvalue.type == DataType.Integer) {
							return  ((double)lvalue.Value) <= ((double)(long)rvalue.Value) ? DynValue.True : DynValue.False;
						} else if (rvalue.type == DataType.Number) { /* number <= number */
							return  ((double)lvalue.Value) <= ((double)rvalue.Value) ? DynValue.True : DynValue.False;
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.As<IComparable<DynValue>> ().CompareTo (rvalue) <= 0;
					}
				}
			case BinaryOp.GreaterOrEqual:
				{
					rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						if (rvalue.type == DataType.Integer) { /* integer >= integer */
							return  (long)lvalue.Value >= (long)rvalue.Value ? DynValue.True : DynValue.False;
						} else if (rvalue.type == DataType.Number) { /* integer >= number */
							return ((double)(long)lvalue.Value) >= (double)rvalue.Value ? DynValue.True : DynValue.False;
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) { /* number >= integer */
						if (rvalue.type == DataType.Integer) {
							return  ((double)lvalue.Value) >= ((double)(long)rvalue.Value) ? DynValue.True : DynValue.False;
						} else if (rvalue.type == DataType.Number) { /* number >= number */
							return  ((double)lvalue.Value) >= ((double)rvalue.Value) ? DynValue.True : DynValue.False;
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.As<IComparable<DynValue>> ().CompareTo (rvalue) >= 0;
					}

				}
			case BinaryOp.Equal:
				{
					rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						if (rvalue.type == DataType.Integer) { /* integer == integer */
							return  ((long)lvalue.Value == (long)rvalue.Value) ? DynValue.True : DynValue.False;
						} else if (rvalue.type == DataType.Number) { /* integer == number */
							return ((double)(long)lvalue.Value) == (double)rvalue.Value ? DynValue.True : DynValue.False;
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) {
						if (rvalue.type == DataType.Integer) { /* number == integer */
							return  (double)lvalue.Value == (double)(long)rvalue.Value ? DynValue.True : DynValue.False;
						} else if (rvalue.type == DataType.Number) { /* number == number */
							return  (double)lvalue.Value == (double)rvalue.Value ? DynValue.True : DynValue.False;
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.Equals (rvalue) ? DynValue.True : DynValue.False;
					}
				}
			
			default:/* BinaryOp.NotEqual */
				{
					rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						if (rvalue.type == DataType.Integer) { /* integer != integer */
							return  ((long)lvalue.Value == (long)rvalue.Value) ? DynValue.False : DynValue.True;
						} else if (rvalue.type == DataType.Number) { /* integer != number */
							return ((double)(long)lvalue.Value) == (double)rvalue.Value ? DynValue.False : DynValue.True;
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) {
						if (rvalue.type == DataType.Integer) { /* number != integer */
							return  (double)lvalue.Value == (double)(long)rvalue.Value ? DynValue.False : DynValue.True;
						} else if (rvalue.type == DataType.Number) { /* number != number */
							return  (double)lvalue.Value == (double)rvalue.Value ? DynValue.False : DynValue.True;
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.Equals (rvalue) ? DynValue.False : DynValue.True;
					}

				}
			}
			BinaryOperationError:
			throw RuntimeException.Throw (
				string.Format ("cannot implement binary operator '{0}' to '{1}' and '{2}'", op, left, right), scope);
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
			case BinaryOp.IntDiv:
				return string.Concat ("(", left, "//", right, ")");
			case BinaryOp.Mod:
				return string.Concat ("(", left, "%", right, ")");
			case BinaryOp.Pow:
				return string.Concat ("(", left, "^", right, ")");
			case BinaryOp.And:
				return string.Concat ("(", left, " and ", right, ")");
			case BinaryOp.Or:
				return string.Concat ("(", left, " or ", right, ")");
			case BinaryOp.Concatenate:
				return string.Concat ("(", left, " .. ", right, ")");
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
		IntDiv,
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

		Concatenate
	}
}
