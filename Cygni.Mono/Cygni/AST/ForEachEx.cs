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
	public class ForEachEx:ASTNode
	{
		readonly ASTNode collection;
		public ASTNode Collection{ get { return collection; } }
		NameEx iterator;
		public NameEx Iterator{ get { return iterator; } }
		readonly BlockEx body;
		public BlockEx Body{ get { return body; } }

		public ForEachEx (BlockEx body,string iterator,ASTNode collection)
		{
			this.iterator =new NameEx( iterator);
			this.collection = collection;
			this.body = body;
		}
		public override NodeType type {
			get {
				return NodeType.ForEach;
			}
		}
		public override DynValue Eval (IScope scope)
		{
			DynValue result = DynValue.Nil;
			foreach (var item in collection.Eval(scope)) {
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
			return result;
		}
		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
		internal void SetIterator(NameEx iterator){
			this.iterator = iterator;
		}
	}
}

