using System;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.Errors;

namespace Cygni.AST
{
	public class UnpackEx:ASTNode
	{
		ASTNode[] items;

		public ASTNode[] Items { get { return this.items; } }

		ASTNode tuple;

		public ASTNode Tuple { get { return this.tuple; } }

		public UnpackEx (ASTNode[] items, ASTNode tuple)
		{
			this.items = items;
			this.tuple = tuple;
		}

		public override NodeType type {
			get {
				return NodeType.Unpack;
			}
		}

		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit(this);
		}

		public override DynValue Eval (IScope scope)
		{
			DynValue v = this.tuple.Eval(scope);
			if (v.type != DataType.Tuple) {
				throw new RuntimeException("The right side is not a tuple. Unable to unpack.");
			} else {
				DynTuple tuple = v.Value as DynTuple;
				if (this.items.Length != tuple.Values.Length) {
					throw new RuntimeException("The number of items must be the same as the number of the items of the tuple to be unpacked.");
				} else {
					for (int i = 0; i <this.items.Length; i++) {
						IAssignable t = this.items[i] as IAssignable;
						if (t == null) {
							throw new RuntimeException("The left side {0} cannot be assigned to. Unable to unpack the tuple.", this.items[i]);
						} else {
							t.Assign(tuple.Values[i], scope);
						}
					}
					return DynValue.Nil;
				}
			}
		}
	}
}

