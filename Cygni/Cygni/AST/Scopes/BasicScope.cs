using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Errors;
namespace Cygni.AST.Scopes
{
	/// <summary>
	/// Description of BasicScope.
	/// </summary>
	public class BasicScope:Dictionary<string,DynValue> ,IScope
	{
		public BasicScope():base()
		{
			
		}
		public DynValue Get(string name){
			DynValue _value;
			if (TryGetValue (name, out _value))
				return _value;
			throw RuntimeException.NotDefined (name);
		}
		public DynValue Put (string name, DynValue value){
			return this [name] = value;
		}

	}
}
