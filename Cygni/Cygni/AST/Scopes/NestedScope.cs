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
	/// Description of NestedScope.
	/// </summary>
	public class NestedScope:IScope
	{
		Dictionary<string,DynValue> EnvTable;
		IScope parent;
		public IScope Parent{ get { return parent; } }
		public NestedScope(IScope parent = null)
		{
			this.EnvTable = new Dictionary<string, DynValue>();
			this.parent = parent;
		}

		#region IScope implementation

		public DynValue this[string name] {
			get {
				DynValue value;
				if (EnvTable.TryGetValue(name, out value))
					return value;
				if (parent == null)
					throw RuntimeException.NotDefined(name);
				return parent[name];
			}
			set {
				EnvTable[name] = value;
			}
		}

		#endregion
		public bool HasName(string name)
		{
			return EnvTable.ContainsKey(name);
		}

		public bool TryGetValue(string name, out DynValue value)
		{
			if (EnvTable.TryGetValue(name, out value))
				return true;
			if (parent == null)
				return false;
			return parent.TryGetValue(name, out value);
		}
	}
}
