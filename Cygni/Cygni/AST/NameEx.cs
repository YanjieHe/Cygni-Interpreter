using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
namespace Cygni.AST
{
	/// <summary>
	/// Description of NameEx.
	/// </summary>
	public class NameEx:ASTNode
	{
		readonly string name;
		public string Name{ get { return name; } }
		public  override NodeType type { get { return NodeType.Name; } }
		
		public NameEx(string name)
		{
			this.name = name;
		}
		public override DynValue Eval(IScope scope)
		{
			return scope[name];
		}
		public override string ToString()
		{
			return name;
		}
	}
}
