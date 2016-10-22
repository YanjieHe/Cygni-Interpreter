using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Extensions;
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
		public BinaryEx(BinaryOp op, ASTNode left, ASTNode right)
		{
			this.op = op;
			this.left = left;
			this.right = right;
		}
		public DynValue ComputeAssign(IScope scope, DynValue _right)
		{
			switch (left.type) {
				case NodeType.Name:
					var _left = left as NameEx;
					scope[_left.Name] = _right;
					return _right;
					
				case NodeType.Dot:
					var dotEx = left as DotEx;
					var target = dotEx.Obj.Eval(scope);
					return dotEx.Set(target.As<IDot>(), _right);
					
				case NodeType.Index:
					var indexEx = left as IndexEx;
					var list = indexEx.list.Eval(scope);
					return list.As<IIndexable>()[indexEx.indexes.Map(i => i.Eval(scope))] = _right;
				default:
					throw new Exception("leftside cannot be assigned to.");
			}
		}
		public override DynValue Eval(IScope scope)
		{
			if (op == BinaryOp.Assign) {
				return ComputeAssign(scope, right.Eval(scope));
			}
			var _left = left.Eval(scope);
			//var _right = right.Eval(scope);
			switch (op) {
				case BinaryOp.Add:
					return _left.Add(right.Eval(scope));
				case BinaryOp.Sub:
					return _left.Subtract(right.Eval(scope));
				case BinaryOp.Mul:
					return _left.Multiply(right.Eval(scope));
				case BinaryOp.Div:
					return _left.Divide(right.Eval(scope));
				case BinaryOp.Mod:
					return _left.Modulo(right.Eval(scope));
				case BinaryOp.Pow:
					return _left.Power(right.Eval(scope));
				case BinaryOp.And:
					if ((bool)_left.Value)
						return (bool)right.Eval(scope).Value;
					return DynValue.False;/* shortcut evaluate*/
				case BinaryOp.Or:
					if ((bool)_left.Value)
						return DynValue.True;/* shortcut evaluate*/
					return (bool)right.Eval(scope).Value;
				case BinaryOp.Less:
					return _left.CompareTo(right.Eval(scope)) < 0;
				case BinaryOp.Greater:
					return _left.CompareTo(right.Eval(scope)) > 0;
				case BinaryOp.LessOrEqual:
					return _left.CompareTo(right.Eval(scope)) <= 0;
				case BinaryOp.GreaterOrEqual:
					return _left.CompareTo(right.Eval(scope)) >= 0;
					
				case BinaryOp.Equal:
					return _left.Equals(right.Eval(scope)) ? DynValue.True : DynValue.False;
				default: /* BinaryOp.NotEqual */
					return _left.Equals(right.Eval(scope)) ? DynValue.False : DynValue.True;
			}
		}
		public override string ToString()
		{
			switch (op) {
				case BinaryOp.Add:
					return string.Concat("(", left, "+", right, ")");
				case BinaryOp.Sub:
					return string.Concat("(", left, "-", right, ")");
				case BinaryOp.Mul:
					return string.Concat("(", left, "*", right, ")");
				case BinaryOp.Div:
					return string.Concat("(", left, "/", right, ")");
				case BinaryOp.Mod:
					return string.Concat("(", left, "%", right, ")");
				case BinaryOp.Pow:
					return string.Concat("(", left, "^", right, ")");
				case BinaryOp.And:
					return string.Concat("(", left, " and ", right, ")");
				case BinaryOp.Or:
					return string.Concat("(", left, " or ", right, ")");
				case BinaryOp.Less:
					return string.Concat("(", left, "<", right, ")");
				case BinaryOp.Greater:
					return string.Concat("(", left, ">", right, ")");
				case BinaryOp.LessOrEqual:
					return string.Concat("(", left, "<=", right, ")");
				case BinaryOp.GreaterOrEqual:
					return string.Concat("(", left, ">=", right, ")");
				case BinaryOp.Equal:
					return string.Concat("(", left, "==", right, ")");
				case BinaryOp.NotEqual:
					return string.Concat("(", left, "!=", right, ")");
				case BinaryOp.Assign:
					return string.Concat("(", left, "=", right, ")");
				default:
					throw new NotSupportedException(op.ToString());
					
			}
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
