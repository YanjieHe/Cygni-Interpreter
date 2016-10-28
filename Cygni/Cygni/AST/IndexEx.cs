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
		ASTNode list;
		ASTNode[] indexes;
		public ASTNode _List { get { return list; } }
		public ASTNode[] Indexes { get { return indexes; } }

		public IndexEx(ASTNode list, ICollection<ASTNode> indexes)
		{
			this.list = list;
			this.indexes = new ASTNode[indexes.Count];
			indexes.CopyTo (this.indexes, 0);
		}
		public override DynValue Eval(IScope scope)
		{
			return list.Eval(scope).As<IIndexable>()[indexes.Map(i => i.Eval(scope))];
		}
		public DynValue Assign(DynValue value, IScope scope){
			var collection = list.Eval(scope);
			return collection.As<IIndexable>()[indexes.Map(i => i.Eval(scope))] = value;
		}
		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}
