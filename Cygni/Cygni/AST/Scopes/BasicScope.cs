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
	public class BasicScope:IScope
	{
		Dictionary<string,DynValue> EnvTable;
		public BasicScope()
		{
			EnvTable = new Dictionary<string, DynValue>();
		}
		public int Count{ get { return EnvTable.Count; } }
		#region IScope implementation
		public DynValue this[string name] {
			get {
				DynValue _value;
				if (EnvTable.TryGetValue(name, out _value))
					return _value;
				throw RuntimeException.NotDefined(name);
			}
			set {
				EnvTable[name] = value;
			}
		}
		#endregion
		public bool HasName(string name){
			return EnvTable.ContainsKey(name);
		}

		public bool TryGetValue(string name, out DynValue value)
		{
			return EnvTable.TryGetValue(name,out value);
		}
	}
}
