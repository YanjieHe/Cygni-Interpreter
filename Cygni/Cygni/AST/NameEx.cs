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
	/// Description of NameEx.
	/// </summary>
	public sealed class NameEx:ASTNode,IAssignable
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
			return scope.Get(name);
		}
		public DynValue Assign(DynValue value,IScope scope){
			return scope.Put(name, value);
		}
		public override string ToString()
		{
			return name;
		}
	}
}
