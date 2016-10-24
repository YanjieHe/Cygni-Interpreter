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
	public interface IScope:IDictionary<string,DynValue>
	{
		//DynValue this[string name]{ get; set; }
		DynValue Get(string name);
		DynValue Put (string name, DynValue value);
	}
}
