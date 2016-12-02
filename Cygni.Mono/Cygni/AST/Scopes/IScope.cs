using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
namespace Cygni.AST.Scopes
{
	/// <summary>
	/// Description of IScope.
	/// </summary>
	public interface IScope
	{
		DynValue Get(string name);
		DynValue Put (string name, DynValue value);
		DynValue Get(int index);
		DynValue Put(int index,DynValue value);
		int Count { get; }
		bool HasName(string name);
		bool TryGetValue(string name,out DynValue value);
		IScope Parent { get; }
	}
}
