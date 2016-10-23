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
	/// Description of BlockEx.
	/// </summary>
	public class BlockEx:ASTNode
	{
		readonly ASTNode[] _expressions;
		public ASTNode[] expressions{ get { return _expressions; } }
		public  override NodeType type { get { return NodeType.Block; } }
		
		public BlockEx(ICollection<ASTNode> _expressions)
		{
			this._expressions = new ASTNode[_expressions.Count];
			_expressions.CopyTo(this._expressions, 0);
		}
		public static BlockEx EmptyBlock = new BlockEx (new ASTNode[0]);
		public override DynValue Eval(IScope scope)
		{
			DynValue result = DynValue.Null;
			foreach (var element in _expressions) {
				result = element.Eval(scope);
				switch (result.type) {
					case DataType.Break:
					case DataType.Continue:
					case DataType.Return:
						return result;
				}
			}
			return result;
		}
		public override string ToString()
		{
			var s = new StringBuilder();
			s.AppendLine("{ ");
			foreach (var element in _expressions) {
				s.Append(element.ToString()).AppendLine(";");
			}
			s.AppendLine("} ");
			return s.ToString();
		}
	}
}
