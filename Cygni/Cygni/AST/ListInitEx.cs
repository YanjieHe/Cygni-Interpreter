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
	public class ListInitEx:ASTNode
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
	}
}
