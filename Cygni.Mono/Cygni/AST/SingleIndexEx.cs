using System;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.Errors;

namespace Cygni.AST
{
	public class SingleIndexEx: ASTNode, IAssignable
	{
		readonly ASTNode collection;
		readonly ASTNode index;
		public ASTNode Collection { get {return this.collection;}}
		public ASTNode Index { get {return this.index;}}

		public SingleIndexEx (ASTNode collection, ASTNode index)
		{
			this.collection = collection;
			this.index = index;
		}
		public override NodeType type{ get { return NodeType.Index; } }
		
		public override DynValue Eval(IScope scope)
		{
			DynValue collection = this.collection.Eval (scope);
			DynValue index = this.index.Eval(scope);
			if(collection.type == DataType.String)
				return collection.AsString()[(int)index.AsNumber()];
			return collection.As<IIndexable>().GetByIndex(index);
		}
		public DynValue Assign(DynValue value, IScope scope){
			DynValue collection = this.collection.Eval(scope);
			DynValue index = this.index.Eval(scope);
			return collection.As<IIndexable>().SetByIndex(index, value);
		}
		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}

	}
}

