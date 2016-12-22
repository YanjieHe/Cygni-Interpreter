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
			switch (op) {
			case BinaryOp.Add:
				{
					DynValue rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						long a = (long)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return new DynValue (DataType.Integer, a + (long)rvalue.Value);
						} else if (rvalue.type == DataType.Number) {
							return new DynValue (DataType.Number, ((double)a) + (double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) {
						double a = (double)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return new DynValue (DataType.Number, a + (long)rvalue.Value);
						} else if (rvalue.type == DataType.Number) {
							return new DynValue (DataType.Number, a + (double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.As<IComputable> ().Add (right.Eval (scope));
					}
				}
			case BinaryOp.Sub:
				{
					DynValue rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						long a = (long)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return new DynValue (DataType.Integer, a - (long)rvalue.Value);
						} else if (rvalue.type == DataType.Number) {
							return new DynValue (DataType.Number, ((double)a) - (double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) {
						double a = (double)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return new DynValue (DataType.Number, a - (long)rvalue.Value);
						} else if (rvalue.type == DataType.Number) {
							return new DynValue (DataType.Number, a - (double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.As<IComputable> ().Subtract (right.Eval (scope));
					}
				}
			case BinaryOp.Mul:
				{
					DynValue rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						long a = (long)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return new DynValue (DataType.Integer, a * (long)rvalue.Value);
						} else if (rvalue.type == DataType.Number) {
							return new DynValue (DataType.Number, ((double)a) * (double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) {
						double a = (double)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return new DynValue (DataType.Number, a * (long)rvalue.Value);
						} else if (rvalue.type == DataType.Number) {
							return new DynValue (DataType.Number, a * (double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.As<IComputable> ().Multiply (right.Eval (scope));
					}
				}
			case BinaryOp.Div:
				{
					DynValue rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						long a = (long)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return new DynValue (DataType.Number, ((double)a) / ((double)(long)rvalue.Value));
						} else if (rvalue.type == DataType.Number) {
							return new DynValue (DataType.Number, ((double)a) / (double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) {
						double a = (double)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return new DynValue (DataType.Number, a / ((double)(long)rvalue.Value));
						} else if (rvalue.type == DataType.Number) {
							return new DynValue (DataType.Number, a / (double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.As<IComputable> ().Divide (right.Eval (scope));
					}
				}
			case BinaryOp.IntDiv:
				{
					DynValue rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						long a = (long)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return new DynValue (DataType.Integer, ((long)a) / ((long)rvalue.Value));
						} else if (rvalue.type == DataType.Number) {
							return new DynValue (DataType.Number, ((long)a) / (long)(double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) {
						double a = (double)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return new DynValue (DataType.Number, (long)a / ((long)rvalue.Value));
						} else if (rvalue.type == DataType.Number) {
							return new DynValue (DataType.Number, (long)a / (long)(double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.As<IComputable> ().Divide (right.Eval (scope));
					}
				}
			case BinaryOp.Mod:
				{
					DynValue rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						long a = (long)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return new DynValue (DataType.Integer, a % (long)rvalue.Value);
						} else if (rvalue.type == DataType.Number) {
							return new DynValue (DataType.Number, ((double)a) % (double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) {
						double a = (double)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return new DynValue (DataType.Number, a % (long)rvalue.Value);
						} else if (rvalue.type == DataType.Number) {
							return new DynValue (DataType.Number, a % (double)rvalue.Value);
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.As<IComputable> ().Modulo (right.Eval (scope));
					}
				}
			case BinaryOp.Pow:
				{
					DynValue rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						long a = (long)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return new DynValue (DataType.Integer, (long)Math.Pow (a, (long)rvalue.Value));
						} else if (rvalue.type == DataType.Number) {
							return new DynValue (DataType.Number, Math.Pow (((double)a), (double)rvalue.Value));
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) {
						double a = (double)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return new DynValue (DataType.Number, Math.Pow (a, (long)rvalue.Value));
						} else if (rvalue.type == DataType.Number) {
							return new DynValue (DataType.Number, Math.Pow (a, (double)rvalue.Value));
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.As<IComputable> ().Power (right.Eval (scope));
					}

				}
			case BinaryOp.Concatenate:
				{
					DynValue rvalue = right.Eval (scope);
					return lvalue.Value.ToString () + rvalue.Value.ToString ();
				}
			case BinaryOp.And:/* shortcut evaluate*/
				{
					if (lvalue.type == DataType.Boolean) {
						if ((bool)lvalue.Value) {
							return right.Eval (scope).AsBoolean ();
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
						return right.Eval (scope).AsBoolean ();
					}
				} else {
					goto BinaryOperationError;
				}

			case BinaryOp.Less:
				{
					DynValue rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						long a = (long)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return  (a < (long)rvalue.Value) ? DynValue.True : DynValue.False;
						} else if (rvalue.type == DataType.Number) {
							return ((double)a) < (double)rvalue.Value ? DynValue.True : DynValue.False;
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) {
						double a = (double)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return  a < (long)rvalue.Value ? DynValue.True : DynValue.False;
						} else if (rvalue.type == DataType.Number) {
							return  a < (double)rvalue.Value ? DynValue.True : DynValue.False;
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.CompareTo (right.Eval (scope)) < 0;
					}
				}

			case BinaryOp.Greater:
				{
					DynValue rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						long a = (long)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return  (a > (long)rvalue.Value) ? DynValue.True : DynValue.False;
						} else if (rvalue.type == DataType.Number) {
							return ((double)a) > (double)rvalue.Value ? DynValue.True : DynValue.False;
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) {
						double a = (double)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return  a > (long)rvalue.Value ? DynValue.True : DynValue.False;
						} else if (rvalue.type == DataType.Number) {
							return  a > (double)rvalue.Value ? DynValue.True : DynValue.False;
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.CompareTo (right.Eval (scope)) > 0;
					}
				}
			case BinaryOp.LessOrEqual:
				{
					DynValue rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						long a = (long)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return  (a <= (long)rvalue.Value) ? DynValue.True : DynValue.False;
						} else if (rvalue.type == DataType.Number) {
							return ((double)a) <= (double)rvalue.Value ? DynValue.True : DynValue.False;
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) {
						double a = (double)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return  a <= (long)rvalue.Value ? DynValue.True : DynValue.False;
						} else if (rvalue.type == DataType.Number) {
							return  a <= (double)rvalue.Value ? DynValue.True : DynValue.False;
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.CompareTo (right.Eval (scope)) <= 0;
					}
				}
			case BinaryOp.GreaterOrEqual:
				{
					DynValue rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						long a = (long)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return  (a >= (long)rvalue.Value) ? DynValue.True : DynValue.False;
						} else if (rvalue.type == DataType.Number) {
							return ((double)a) >= (double)rvalue.Value ? DynValue.True : DynValue.False;
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) {
						double a = (double)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return  a >= (long)rvalue.Value ? DynValue.True : DynValue.False;
						} else if (rvalue.type == DataType.Number) {
							return  a >= (double)rvalue.Value ? DynValue.True : DynValue.False;
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.CompareTo (right.Eval (scope)) >= 0;
					}
				}
			case BinaryOp.Equal:
				{
					DynValue rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						long a = (long)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return  (a == (long)rvalue.Value) ? DynValue.True : DynValue.False;
						} else if (rvalue.type == DataType.Number) {
							return ((double)a) == (double)rvalue.Value ? DynValue.True : DynValue.False;
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) {
						double a = (double)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return  a == (long)rvalue.Value ? DynValue.True : DynValue.False;
						} else if (rvalue.type == DataType.Number) {
							return  a == (double)rvalue.Value ? DynValue.True : DynValue.False;
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.Equals (right.Eval (scope)) ? DynValue.True : DynValue.False;
					}
				}
			
			default:/* BinaryOp.NotEqual */
				{
					DynValue rvalue = right.Eval (scope);
					if (lvalue.type == DataType.Integer) {
						long a = (long)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return  (a == (long)rvalue.Value) ? DynValue.False : DynValue.True;
						} else if (rvalue.type == DataType.Number) {
							return ((double)a) == (double)rvalue.Value ? DynValue.False : DynValue.True;
						} else {
							goto BinaryOperationError;
						}
					} else if (lvalue.type == DataType.Number) {
						double a = (double)lvalue.Value;
						if (rvalue.type == DataType.Integer) {
							return  a == (long)rvalue.Value ? DynValue.False : DynValue.True;
						} else if (rvalue.type == DataType.Number) {
							return  a == (double)rvalue.Value ? DynValue.False : DynValue.True;
						} else {
							goto BinaryOperationError;
						}
					} else {
						return lvalue.Equals (right.Eval (scope)) ? DynValue.False : DynValue.True;
					}
				}
			}
			BinaryOperationError:
			throw new RuntimeException ("cannot implement '{0}' to '{0}' and '{1}'", op, left, right);
		}


		protected RuntimeException BinaryError (string op)
		{
			return new RuntimeException ("cannot implement '{0}' to '{0}' and '{1}'", op, left, right);
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

	/*public class AddEx:BinaryEx
	{
		public AddEx (ASTNode left, ASTNode right) : base (left, right)
		{
		}

		public override DynValue Eval (IScope scope)
		{
			DynValue lvalue = right.Eval (scope);
			DynValue rvalue = right.Eval (scope);
			if (lvalue.type == DataType.Integer) {
				if (rvalue.type == DataType.Integer) {
					return new DynValue (DataType.Integer, (long)lvalue.Value + (long)rvalue.Value);
				} else if (rvalue.type == DataType.Number) {
					return new DynValue (DataType.Number, ((double)(long)lvalue.Value) + (double)rvalue.Value);
				} else {
					throw BinaryError ("+");
				}
			} else if (lvalue.type == DataType.Number) {
				if (rvalue.type == DataType.Integer) {
					return new DynValue (DataType.Number, (double)lvalue.Value + ((double)(long)rvalue.Value));
				} else if (rvalue.type == DataType.Number) {
					return new DynValue (DataType.Number, (double)lvalue.Value + (double)rvalue.Value);
				} else {
					throw BinaryError ("+");
				}
			} else {
				return lvalue.As<IComputable> ().Add (right.Eval (scope));
			}
		}
	}

	public class SubtractEx:BinaryEx
	{
		public SubtractEx (ASTNode left, ASTNode right) : base (left, right)
		{
		}

		public override DynValue Eval (IScope scope)
		{
			DynValue lvalue = right.Eval (scope);
			DynValue rvalue = right.Eval (scope);
			if (lvalue.type == DataType.Integer) {
				if (rvalue.type == DataType.Integer) {
					return new DynValue (DataType.Integer, (long)lvalue.Value - (long)rvalue.Value);
				} else if (rvalue.type == DataType.Number) {
					return new DynValue (DataType.Number, ((double)(long)lvalue.Value) - (double)rvalue.Value);
				} else {
					throw BinaryError ("-");
				}
			} else if (lvalue.type == DataType.Number) {
				if (rvalue.type == DataType.Integer) {
					return new DynValue (DataType.Number, (double)lvalue.Value - ((double)(long)rvalue.Value));
				} else if (rvalue.type == DataType.Number) {
					return new DynValue (DataType.Number, (double)lvalue.Value - (double)rvalue.Value);
				} else {
					throw BinaryError ("-");
				}
			} else {
				return lvalue.As<IComputable> ().Subtract (right.Eval (scope));
			}
		}
	}

	public class MultiplyEx:BinaryEx
	{
		public MultiplyEx (ASTNode left, ASTNode right) : base (left, right)
		{
		}

		public override DynValue Eval (IScope scope)
		{
			DynValue lvalue = right.Eval (scope);
			DynValue rvalue = right.Eval (scope);
			if (lvalue.type == DataType.Integer) {
				if (rvalue.type == DataType.Integer) {
					return new DynValue (DataType.Integer, (long)lvalue.Value * (long)rvalue.Value);
				} else if (rvalue.type == DataType.Number) {
					return new DynValue (DataType.Number, ((double)(long)lvalue.Value) * (double)rvalue.Value);
				} else {
					throw BinaryError ("*");
				}
			} else if (lvalue.type == DataType.Number) {
				if (rvalue.type == DataType.Integer) {
					return new DynValue (DataType.Number, (double)lvalue.Value * ((double)(long)rvalue.Value));
				} else if (rvalue.type == DataType.Number) {
					return new DynValue (DataType.Number, (double)lvalue.Value * (double)rvalue.Value);
				} else {
					throw BinaryError ("*");
				}
			} else {
				return lvalue.As<IComputable> ().Multiply (right.Eval (scope));
			}
		}
	}

	public class DivideEx:BinaryEx
	{
		public DivideEx (ASTNode left, ASTNode right) : base (left, right)
		{
		}

		public override DynValue Eval (IScope scope)
		{
			DynValue lvalue = right.Eval (scope);
			DynValue rvalue = right.Eval (scope);
			if (lvalue.type == DataType.Integer) {
				if (rvalue.type == DataType.Integer) {
					return new DynValue (DataType.Integer, (long)lvalue.Value / (long)rvalue.Value);
				} else if (rvalue.type == DataType.Number) {
					return new DynValue (DataType.Number, ((double)(long)lvalue.Value) / (double)rvalue.Value);
				} else {
					throw BinaryError ("/");
				}
			} else if (lvalue.type == DataType.Number) {
				if (rvalue.type == DataType.Integer) {
					return new DynValue (DataType.Number, (double)lvalue.Value / ((double)(long)rvalue.Value));
				} else if (rvalue.type == DataType.Number) {
					return new DynValue (DataType.Number, (double)lvalue.Value / (double)rvalue.Value);
				} else {
					throw BinaryError ("/");
				}
			} else {
				return lvalue.As<IComputable> ().Divide (right.Eval (scope));
			}
		}
	}

	public class IntDivideEx:BinaryEx
	{
		public IntDivideEx (ASTNode left, ASTNode right) : base (left, right)
		{
		}

		public override DynValue Eval (IScope scope)
		{
			DynValue lvalue = right.Eval (scope);
			DynValue rvalue = right.Eval (scope);
			if (lvalue.type == DataType.Integer) {
				if (rvalue.type == DataType.Integer) {
					return new DynValue (DataType.Integer, (long)lvalue.Value / (long)rvalue.Value);
				} else if (rvalue.type == DataType.Number) {
					return new DynValue (DataType.Number, (long)lvalue.Value / (long)(double)rvalue.Value);
				} else {
					throw BinaryError ("//");
				}
			} else if (lvalue.type == DataType.Number) {
				if (rvalue.type == DataType.Integer) {
					return new DynValue (DataType.Number, (long)(double)lvalue.Value + (long)rvalue.Value);
				} else if (rvalue.type == DataType.Number) {
					return new DynValue (DataType.Number, (long)(double)lvalue.Value + (long)(double)rvalue.Value);
				} else {
					throw BinaryError ("//");
				}
			} else {
				return lvalue.As<IComputable> ().Divide (right.Eval (scope));
			}
		}
	}

	public class ModuloEx:BinaryEx
	{
		public ModuloEx (ASTNode left, ASTNode right) : base (left, right)
		{
		}

		public override DynValue Eval (IScope scope)
		{
			DynValue lvalue = right.Eval (scope);
			DynValue rvalue = right.Eval (scope);
			if (lvalue.type == DataType.Integer) {
				if (rvalue.type == DataType.Integer) {
					return new DynValue (DataType.Integer, (long)lvalue.Value % (long)rvalue.Value);
				} else if (rvalue.type == DataType.Number) {
					return new DynValue (DataType.Number, ((double)(long)lvalue.Value) % (double)rvalue.Value);
				} else {
					throw BinaryError ("%");
				}
			} else if (lvalue.type == DataType.Number) {
				if (rvalue.type == DataType.Integer) {
					return new DynValue (DataType.Number, (double)lvalue.Value % ((double)(long)rvalue.Value));
				} else if (rvalue.type == DataType.Number) {
					return new DynValue (DataType.Number, (double)lvalue.Value % (double)rvalue.Value);
				} else {
					throw BinaryError ("%");
				}
			} else {
				return lvalue.As<IComputable> ().Modulo	(right.Eval (scope));
			}
		}
	}

	public class PowerEx:BinaryEx
	{
		public PowerEx (ASTNode left, ASTNode right) : base (left, right)
		{
		}

		public override DynValue Eval (IScope scope)
		{
			DynValue lvalue = right.Eval (scope);
			DynValue rvalue = right.Eval (scope);
			if (lvalue.type == DataType.Integer) {
				if (rvalue.type == DataType.Integer) {
					return new DynValue (DataType.Integer, (long)Math.Pow ((long)lvalue.Value, (long)rvalue.Value));
				} else if (rvalue.type == DataType.Number) {
					return new DynValue (DataType.Number, Math.Pow ((long)lvalue.Value, (double)rvalue.Value));
				} else {
					throw BinaryError ("^");
				}
			} else if (lvalue.type == DataType.Number) {
				if (rvalue.type == DataType.Integer) {
					return new DynValue (DataType.Number, Math.Pow ((double)lvalue.Value, ((double)(long)rvalue.Value)));
				} else if (rvalue.type == DataType.Number) {
					return new DynValue (DataType.Number, Math.Pow ((double)lvalue.Value, (double)rvalue.Value));
				} else {
					throw BinaryError ("^");
				}
			} else {
				return lvalue.As<IComputable> ().Power (right.Eval (scope));
			}
		}
	}

	public class LessEx:BinaryEx
	{
		public LessEx (ASTNode left, ASTNode right) : base (left, right)
		{
		}

		public override DynValue Eval (IScope scope)
		{			
			DynValue lvalue = right.Eval (scope);
			DynValue rvalue = right.Eval (scope);
			if (lvalue.type == DataType.Integer) {
				if (rvalue.type == DataType.Integer) {
					return  ((long)lvalue.Value < (long)rvalue.Value) ? DynValue.True : DynValue.False;
				} else if (rvalue.type == DataType.Number) {
					return ((double)lvalue.Value < (double)rvalue.Value) ? DynValue.True : DynValue.False;
				} else {
					throw BinaryError ("<");
				}
			} else if (lvalue.type == DataType.Number) {
				if (rvalue.type == DataType.Integer) {
					return  ((double)lvalue.Value < (long)rvalue.Value) ? DynValue.True : DynValue.False;
				} else if (rvalue.type == DataType.Number) {
					return  ((double)lvalue.Value < (double)rvalue.Value) ? DynValue.True : DynValue.False;
				} else {
					throw BinaryError ("<");
				}
			} else {
				return lvalue.CompareTo (right.Eval (scope)) < 0;
			}

		}
	}

	public class GreaterEx:BinaryEx
	{
		public GreaterEx (ASTNode left, ASTNode right) : base (left, right)
		{
		}

		public override DynValue Eval (IScope scope)
		{
			DynValue lvalue = right.Eval (scope);
			DynValue rvalue = right.Eval (scope);
			if (lvalue.type == DataType.Integer) {
				if (rvalue.type == DataType.Integer) {
					return  ((long)lvalue.Value > (long)rvalue.Value) ? DynValue.True : DynValue.False;
				} else if (rvalue.type == DataType.Number) {
					return ((double)lvalue.Value > (double)rvalue.Value) ? DynValue.True : DynValue.False;
				} else {
					throw BinaryError (">");
				}
			} else if (lvalue.type == DataType.Number) {
				if (rvalue.type == DataType.Integer) {
					return  ((double)lvalue.Value > (long)rvalue.Value) ? DynValue.True : DynValue.False;
				} else if (rvalue.type == DataType.Number) {
					return  ((double)lvalue.Value > (double)rvalue.Value) ? DynValue.True : DynValue.False;
				} else {
					throw BinaryError (">");
				}
			} else {
				return lvalue.CompareTo (right.Eval (scope)) > 0;
			}

		}
	}

	public class LessOrEqualEx:BinaryEx
	{
		public LessOrEqualEx (ASTNode left, ASTNode right) : base (left, right)
		{
		}

		public override DynValue Eval (IScope scope)
		{
			DynValue lvalue = right.Eval (scope);
			DynValue rvalue = right.Eval (scope);
			if (lvalue.type == DataType.Integer) {
				if (rvalue.type == DataType.Integer) {
					return  ((long)lvalue.Value <= (long)rvalue.Value) ? DynValue.True : DynValue.False;
				} else if (rvalue.type == DataType.Number) {
					return ((double)lvalue.Value <= (double)rvalue.Value) ? DynValue.True : DynValue.False;
				} else {
					throw BinaryError ("<=");
				}
			} else if (lvalue.type == DataType.Number) {
				if (rvalue.type == DataType.Integer) {
					return  ((double)lvalue.Value <= (long)rvalue.Value) ? DynValue.True : DynValue.False;
				} else if (rvalue.type == DataType.Number) {
					return  ((double)lvalue.Value <= (double)rvalue.Value) ? DynValue.True : DynValue.False;
				} else {
					throw BinaryError ("<=");
				}
			} else {
				return lvalue.CompareTo (right.Eval (scope)) <= 0;
			}
		}
	}

	public class GreaterOrEqualEx:BinaryEx
	{
		public GreaterOrEqualEx (ASTNode left, ASTNode right) : base (left, right)
		{
		}

		public override DynValue Eval (IScope scope)
		{
			DynValue lvalue = right.Eval (scope);
			DynValue rvalue = right.Eval (scope);
			if (lvalue.type == DataType.Integer) {
				if (rvalue.type == DataType.Integer) {
					return  ((long)lvalue.Value >= (long)rvalue.Value) ? DynValue.True : DynValue.False;
				} else if (rvalue.type == DataType.Number) {
					return ((double)lvalue.Value >= (double)rvalue.Value) ? DynValue.True : DynValue.False;
				} else {
					throw BinaryError (">=");
				}
			} else if (lvalue.type == DataType.Number) {
				if (rvalue.type == DataType.Integer) {
					return  ((double)lvalue.Value >= (long)rvalue.Value) ? DynValue.True : DynValue.False;
				} else if (rvalue.type == DataType.Number) {
					return  ((double)lvalue.Value >= (double)rvalue.Value) ? DynValue.True : DynValue.False;
				} else {
					throw BinaryError (">=");
				}
			} else {
				return lvalue.CompareTo (right.Eval (scope)) >= 0;
			}
		}
	}

	public class EqualEx:BinaryEx
	{
		public EqualEx (ASTNode left, ASTNode right) : base (left, right)
		{
		}

		public override DynValue Eval (IScope scope)
		{
			DynValue lvalue = right.Eval (scope);
			DynValue rvalue = right.Eval (scope);
			if (lvalue.type == DataType.Integer) {
				if (rvalue.type == DataType.Integer) {
					return  ((long)lvalue.Value == (long)rvalue.Value) ? DynValue.True : DynValue.False;
				} else if (rvalue.type == DataType.Number) {
					return ((double)(long)lvalue.Value == (double)rvalue.Value) ? DynValue.True : DynValue.False;
				} else {
					throw BinaryError ("==");
				}
			} else if (lvalue.type == DataType.Number) {
				if (rvalue.type == DataType.Integer) {
					return  ((double)lvalue.Value == (double)(long)rvalue.Value) ? DynValue.True : DynValue.False;
				} else if (rvalue.type == DataType.Number) {
					return  ((double)lvalue.Value == (double)rvalue.Value) ? DynValue.True : DynValue.False;
				} else {
					throw BinaryError ("==");
				}
			} else {
				return lvalue.Equals (right.Eval (scope)) ? DynValue.True : DynValue.False;
			}
		}
	}

	public class NotEqualEx:BinaryEx
	{
		public NotEqualEx (ASTNode left, ASTNode right) : base (left, right)
		{
		}

		public override DynValue Eval (IScope scope)
		{
			DynValue lvalue = right.Eval (scope);
			DynValue rvalue = right.Eval (scope);
			if (lvalue.type == DataType.Integer) {
				if (rvalue.type == DataType.Integer) {
					return  ((long)lvalue.Value == (long)rvalue.Value) ? DynValue.False : DynValue.True;
				} else if (rvalue.type == DataType.Number) {
					return ((double)(long)lvalue.Value == (double)rvalue.Value) ? DynValue.False : DynValue.True;
				} else {
					throw BinaryError ("==");
				}
			} else if (lvalue.type == DataType.Number) {
				if (rvalue.type == DataType.Integer) {
					return  ((double)lvalue.Value == (double)(long)rvalue.Value) ? DynValue.False : DynValue.True;
				} else if (rvalue.type == DataType.Number) {
					return  ((double)lvalue.Value == (double)rvalue.Value) ? DynValue.False : DynValue.True;
				} else {
					throw BinaryError ("==");
				}
			} else {
				return lvalue.Equals (right.Eval (scope)) ? DynValue.False : DynValue.True;
			}
		}
	}

	public class AndEx:BinaryEx
	{
		public AndEx (ASTNode left, ASTNode right) : base (left, right)
		{
		}

		public override DynValue Eval (IScope scope)
		{

			DynValue lvalue = right.Eval (scope);
			if (lvalue.type == DataType.Boolean) {
				if ((bool)lvalue.Value) {
					return right.Eval (scope).AsBoolean ();
				} else {
					return DynValue.False;
				}
			} else {
				throw BinaryError ("and");
			}

		}
	}

	public class OrEx:BinaryEx
	{
		public OrEx (ASTNode left, ASTNode right) : base (left, right)
		{
		}

		public override DynValue Eval (IScope scope)
		{

			DynValue lvalue = right.Eval (scope);
			if (lvalue.type == DataType.Boolean) {
				if ((bool)lvalue.Value) {
					return DynValue.True;
				} else {
					return right.Eval (scope).AsBoolean ();
				}
			} else {
				throw BinaryError ("or");
			}
		}
	}

	public class ConcatenateEx:BinaryEx
	{
		public ConcatenateEx (ASTNode left, ASTNode right) : base (left, right)
		{
		}

		public override DynValue Eval (IScope scope)
		{
			DynValue lvalue = right.Eval (scope);
			DynValue rvalue = right.Eval (scope);
			return lvalue.Value.ToString () + rvalue.Value.ToString ();
		}
}*/
}
