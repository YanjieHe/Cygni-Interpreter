using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST.Scopes;

namespace Cygni.AST
{
	/// <summary>
	/// Description of ListInitEx.
	/// </summary>
	public class ListInitEx:ASTNode,ISymbolLookUp
	{
		public override NodeType type { get { return NodeType.ListInit; } }
		List<ASTNode> list;
		public ListInitEx(List<ASTNode>list)
		{
			this.list = list;
		}
		public override DynValue Eval(IScope scope)
		{
			return DynValue.FromList(new DynList(list.Select(i => i.Eval(scope)), list.Count));
		}
		public void LookUpForLocalVariable (List<NameEx>names)
		{
			for (int i = 0; i < list.Count; i++) {
				if (list[i] is ISymbolLookUp)
					(list[i] as ISymbolLookUp).LookUpForLocalVariable (names);
			}
		}
	}
}
