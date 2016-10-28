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
	/// Description of Constant.
	/// </summary>
	public sealed class Constant:ASTNode
	{
		readonly DynValue value;
		public DynValue Value{ get { return value; } }
		public override NodeType type {get{return NodeType.Constant;}}
		
		public Constant(DynValue value)
		{
			this.value = value;
		}
		
		public override DynValue Eval(IScope scope)
		{
			return value;
		}
		
		public override string ToString()
		{
			return value.ToString();
		}
		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}		
	}
	
}
