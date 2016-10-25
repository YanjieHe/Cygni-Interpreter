using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.AST.Scopes;

namespace Cygni.AST
{
	/// <summary>
	/// Description of IndexEx.
	/// </summary>
	public class IndexEx:ASTNode,IAssignable,ISymbolLookUp
	{
		public override NodeType type{ get { return NodeType.Index; } }
		internal ASTNode list{  get; private set; }
		internal ASTNode[] indexes{  get; private set; }
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
		public void LookUpForLocalVariable (List<NameEx>names)
		{
			if (list is ISymbolLookUp)
				(list as ISymbolLookUp).LookUpForLocalVariable (names);
			for (int i = 0; i < indexes.Length; i++) {
				if (indexes[i] is ISymbolLookUp)
					(indexes[i] as ISymbolLookUp).LookUpForLocalVariable (names);
			}
		}
	}
}
