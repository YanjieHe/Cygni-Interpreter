using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;

namespace Cygni.AST
{
	/// <summary>
	/// Description of WhileEx.
	/// </summary>
	public class WhileEx:ASTNode
	{
		readonly ASTNode condition;
		readonly BlockEx body;
		public	ASTNode Condition{ get { return condition; } }
		public	BlockEx Body{ get { return body; } }
		public  override NodeType type { get { return NodeType.While; } }
		
		public WhileEx(ASTNode condition, BlockEx body)
		{
			this.condition = condition;
			this.body = body;
		}
		public override DynValue Eval(IScope scope)
		{
			DynValue result = DynValue.Null;
			for (;;) {
				if ((bool)condition.Eval(scope).Value) {
					result = body.Eval(scope);
					switch (result.type) {
						case DataType.Break:
							return DynValue.Null;
						case DataType.Continue:
							continue;
						case DataType.Return:
							return result;
					}
				} else
					return result;
			}
		}
		public override string ToString()
		{
			return string.Concat(" while ", condition, body);
		}
	
		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}
