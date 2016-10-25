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
	public sealed class NestedScope: IScope
	{
		EnvTable envTable;
		IScope parent;
		public IScope Parent{ get { return parent; } }
		public NestedScope(IScope parent = null)
		{
			this.envTable = new EnvTable ();
			this.parent = parent;
		}
		public int Count { get { return envTable.Count; } }
		public DynValue Get(string name){
			DynValue value;
			if (this.envTable.TryGetValue (name, out value))
				return value;
			if (parent == null)
				throw RuntimeException.NotDefined (name);
			return parent.Get (name);
		}
		public DynValue Put(string name,DynValue value){
			return this.envTable[name] = value;
		}
		public bool HasName(string name){
			return this.envTable.ContainsKey (name);
		}
		public bool TryGetValue(string name, out DynValue value)
		{
			if (this.envTable.TryGetValue(name, out value))
				return true;
			if (parent == null)
				return false;
			return parent.TryGetValue(name, out value);
		}
		public DynValue Get(int index){
			throw new NotSupportedException ();
		}
		public DynValue Put(int index,DynValue value){
			throw new NotSupportedException ();
		}
	}
}
