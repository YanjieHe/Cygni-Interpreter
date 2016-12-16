using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
namespace Cygni.AST
{
	/// <summary>
	/// Description of ASTNode.
	/// </summary>
	public abstract class ASTNode
	{
		public abstract DynValue Eval(IScope scope);
		public abstract NodeType type { get; }
		internal abstract void Accept (ASTVisitor visitor);

		public static ASTNode Variable(string name)
		{
			return new NameEx(name);
		}

		public static ASTNode Parameter(string name)
		{
			return new NameEx(name);
		}

		public static ASTNode Assign(ASTNode left, ASTNode right)
		{
			return new AssignEx(left, right);
		}

		public static ASTNode Global(string[] names, ASTNode[] values)
		{
			return new GlobalEx(names, values);
		}

		public static ASTNode Set(ASTNode[] targets, ASTNode[] values)
		{
			return new SetEx(targets, values);
		}

		public static ASTNode Unpack(ASTNode[] items, ASTNode tuple)
		{
			return new UnpackEx(items, tuple);
		}
		public static ASTNode Or(ASTNode left, ASTNode right)
		{
			return new BinaryEx(BinaryOp.Or, left, right);
		}
		public static ASTNode And(ASTNode left, ASTNode right)
		{
			return new BinaryEx(BinaryOp.And, left, right);
		}

		public static ASTNode Equal(ASTNode left, ASTNode right)
		{
			return new BinaryEx(BinaryOp.Equal, left, right);
		}
		public static ASTNode NotEqual(ASTNode left, ASTNode right)
		{
			return new BinaryEx(BinaryOp.NotEqual, left, right);
		}

		public static ASTNode Less(ASTNode left, ASTNode right)
		{
			return new BinaryEx(BinaryOp.Less, left, right);
		}
		public static ASTNode Greater(ASTNode left, ASTNode right)
		{
			return new BinaryEx(BinaryOp.Greater, left, right);
		}
		public static ASTNode LessOrEqual(ASTNode left, ASTNode right)
		{
			return new BinaryEx(BinaryOp.LessOrEqual, left, right);
		}
		public static ASTNode GreaterOrEqual(ASTNode left, ASTNode right)
		{
			return new BinaryEx(BinaryOp.GreaterOrEqual, left, right);
		}

		public static ASTNode Add(ASTNode left, ASTNode right)
		{
			return new BinaryEx(BinaryOp.Add, left, right);
		}
		public static ASTNode Subtract(ASTNode left, ASTNode right)
		{
			return new BinaryEx(BinaryOp.Sub, left, right);
		}
		public static ASTNode Multiply(ASTNode left, ASTNode right)
		{
			return new BinaryEx(BinaryOp.Mul, left, right);
		}
		public static ASTNode Divide(ASTNode left, ASTNode right)
		{
			return new BinaryEx(BinaryOp.Div, left, right);
		}
		public static ASTNode Modulo(ASTNode left, ASTNode right)
		{
			return new BinaryEx(BinaryOp.Mod, left, right);
		}
		public static ASTNode Power(ASTNode left, ASTNode right)
		{
			return new BinaryEx(BinaryOp.Pow, left, right);
		}

		public static ASTNode UnaryPlus(ASTNode value)
		{
			return new UnaryEx(UnaryOp.Plus, value);
		}
		public static ASTNode UnaryMinus(ASTNode value)
		{
			return new UnaryEx(UnaryOp.Minus, value);
		}
		public static ASTNode Negate(ASTNode value)
		{
			return new UnaryEx(UnaryOp.Not, value);
		}

		public static ASTNode Number(double value)
		{
			return new Constant(DynValue.FromNumber(value));
		}
		public static ASTNode Boolean(bool value)
		{
			return value ? True : False;
		}
		public static ASTNode String(string value)
		{
			return new Constant(DynValue.FromString(value));
		}
		public static ASTNode Constant(DynValue value){
			return new Constant (value);
		}
		public static readonly ASTNode True = new Constant(DynValue.True);
		public static readonly ASTNode False = new Constant(DynValue.False);

		public static BlockEx Block(ICollection<ASTNode> expressions)
		{
			return new BlockEx(expressions);
		}
		public static ASTNode IfThen(ASTNode condition, ASTNode  IfTrue)
		{
			return new IfEx(condition, IfTrue, null);
		}
		public static ASTNode IfThenElse(ASTNode condition, ASTNode  IfTrue, ASTNode  IfFalse)
		{
			return new IfEx(condition, IfTrue, IfFalse);
		}
		public static ASTNode While(ASTNode condition, BlockEx  body)
		{
			return new WhileEx(condition, body);
		}
		public static ASTNode For(BlockEx body, string iterator, ASTNode start, ASTNode end)
		{
			return new ForEx(body, iterator, start, end, null);
		}
		public static ASTNode For(BlockEx body, string iterator, ASTNode start, ASTNode end, ASTNode step)
		{
			return new ForEx(body, iterator, start, end, step);
		}
		public static ASTNode ForEach(BlockEx body, string iterator, ASTNode collection)
		{
			return new ForEachEx(body, iterator, collection);
		}
		public static ASTNode Define(string name, string[] parameters, BlockEx body)
		{
			return new DefFuncEx(name, parameters, body);
		}
		public static ASTNode Class(string name, BlockEx body, string parent = null)
		{
			return new DefClassEx(name, body, parent);
		}
		public static ASTNode ListInit(List<ASTNode> list)
		{
			return new ListInitEx(list);
		}
		public static ASTNode DictionaryInit(List<ASTNode> list){
			return new DictionaryInitEx (list);
		}
		public static ASTNode IndexAccess(ASTNode collection, List<ASTNode>indexes)
		{
			if (indexes.Count == 1){
				return new SingleIndexEx(collection, indexes[0]);
			} else {
				return new IndexEx(collection, indexes);
			}
		}
		public static ASTNode Invoke(ASTNode function, ICollection<ASTNode>arguments)
		{
			return new InvokeEx(function, arguments);
		}
		public static readonly ASTNode Break = new Constant(DynValue.Break);
		public static readonly ASTNode Continue = new Constant(DynValue.Continue);
		public static ASTNode Return(ASTNode value)
		{
			return new ReturnEx(value);
		}

		public static readonly ASTNode Nil = new Constant(DynValue.Nil);
		public static ASTNode Dot(ASTNode obj, string fieldName)
		{
			return new DotEx(obj, fieldName);
		}

	}
}
