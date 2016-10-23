using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST.Scopes;
namespace Cygni.AST
{
	public class LocalNameEx:NameEx
	{
		const int UNKNOWN = -1;
		readonly int nest;
		readonly int index;
		public LocalNameEx (string name, int nest,int index):base(name)
		{
			this.nest = nest;
			this.index = index;
		}
		public override DynValue Eval(IScope scope)
		{
			if (index == UNKNOWN)
				return scope [name];
			return (ArrayScope) [nest, index];
		}
	}
}

