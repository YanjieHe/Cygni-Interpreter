using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
namespace Cygni.AST
{
	/// <summary>
	/// Description of IScope.
	/// </summary>
	public interface IScope
	{
		DynValue this[string name]{ get; set; }
		bool HasName(string name);
		bool TryGetValue(string name, out DynValue value);
	}
}
