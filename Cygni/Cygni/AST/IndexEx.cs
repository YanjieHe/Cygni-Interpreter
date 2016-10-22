using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Extensions;
namespace Cygni.AST
{
	/// <summary>
	/// Description of IndexEx.
	/// </summary>
	public class IndexEx:ASTNode
	{
		public override NodeType type{ get { return NodeType.Index; } }
		internal ASTNode list{  get; private set; }
		internal List<ASTNode> indexes{  get; private set; }
		public IndexEx(ASTNode list, List<ASTNode> indexes)
		{
			this.list = list;
			indexes.TrimExcess();
			this.indexes = indexes;
		}
		public override DynValue Eval(IScope scope)
		{
			return list.Eval(scope).As<IIndexable>()[indexes.Map(i => i.Eval(scope))];
		}
	}
}
