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
	public class NestedScope:Dictionary<string,DynValue>, IScope
	{
		IScope parent;
		public IScope Parent{ get { return parent; } }
		public NestedScope(IScope parent = null):base()
		{
			this.parent = parent;
		}
		public DynValue Get(string name){
			DynValue value;
			if (this.TryGetValue (name, out value))
				return value;
			if (parent == null)
				throw RuntimeException.NotDefined (name);
			return parent.Get (name);
		}
		public DynValue Put(string name,DynValue value){
			return this [name] = value;
		}
		public new bool TryGetValue(string name, out DynValue value)
		{
			if (base.TryGetValue(name, out value))
				return true;
			if (parent == null)
				return false;
			return parent.TryGetValue(name, out value);
		}
	}
}
