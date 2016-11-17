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
	/// Description of IndexEx.
	/// </summary>
	public class IndexEx:ASTNode,IAssignable
	{
		public override NodeType type{ get { return NodeType.Index; } }
		readonly ASTNode collection;
		readonly ASTNode[] indexes;
		public ASTNode _List { get { return collection; } }
		public ASTNode[] Indexes { get { return indexes; } }

		public IndexEx(ASTNode collection, ICollection<ASTNode> indexes)
		{
			this.collection = collection;
			this.indexes = new ASTNode[indexes.Count];
			indexes.CopyTo (this.indexes, 0);
		}
		public override DynValue Eval(IScope scope)
		{
			DynValue collection = this.collection.Eval (scope);
			int n = indexes.Length;
			DynValue[] args = new DynValue[n];
			for (int i = 0; i < n; i++)
				args [i] = indexes [i].Eval (scope);
			if(collection.type == DataType.String)
				return collection[args];
			return collection.As<IIndexable>()[args];
		}
		public DynValue Assign(DynValue value, IScope scope){
			DynValue collection = this.collection.Eval(scope);
			int n = indexes.Length;
			DynValue[] args = new DynValue[n];
			for (int i = 0; i < n; i++)
				args [i] = indexes [i].Eval (scope);
			return collection.As<IIndexable>()[args] = value;
		}
		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}
