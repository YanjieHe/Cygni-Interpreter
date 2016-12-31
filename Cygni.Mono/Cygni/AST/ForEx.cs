using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Errors;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;

namespace Cygni.AST
{
	/// <summary>
	/// Description of ForEx.
	/// </summary>
	public class ForEx:ASTNode
	{
		readonly ASTNode collection;
		readonly NameEx iterator;
		readonly BlockEx body;

		public ASTNode Collection{ get { return collection; } }

		public NameEx Iterator{ get { return iterator; } }

		public BlockEx Body{ get { return body; } }



		public ForEx (BlockEx body, NameEx iterator, ASTNode collection)
		{
			this.iterator = iterator;
			this.collection = collection;
			this.body = body;
		}

		public override NodeType type {
			get {
				return NodeType.For;
			}
		}

		public override DynValue Eval (IScope scope)
		{
			DynValue result = DynValue.Nil;
			IEnumerable<DynValue> collection = this.collection.Eval (scope).As<IEnumerable<DynValue>> ();
			foreach (DynValue item in collection) {
				iterator.Assign (item, scope);
				result = body.Eval (scope);
				switch (result.type) {
				case DataType.Break:
					return DynValue.Nil;
				case DataType.Continue:
					continue;
				case DataType.Return:
					return result;
				}
			}
			return DynValue.Nil;
		}

		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}
