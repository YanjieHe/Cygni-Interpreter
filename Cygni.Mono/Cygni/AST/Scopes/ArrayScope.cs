using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Errors;
using System.Collections.Generic;
using System.Linq;
namespace Cygni.AST.Scopes
{
	public sealed class ArrayScope:IScope
	{
		public ScopeType type { get { return ScopeType.Array; } }

		readonly DynValue[] values;

		readonly IScope parent;
		public IScope Parent{ get { return parent; } }

		public ArrayScope (int capacity, IScope parent = null)
		{
			this.values = new DynValue[capacity];
			this.parent = parent;
		}
		public ArrayScope (DynValue[] values,IScope parent = null)
		{
			this.values = values;
			this.parent = parent;
		}
		public DynValue Get(int index){
			return values [index];
		}
		public DynValue Put(int index,DynValue value){
			return values [index] = value;
		}

		public DynValue Get(string name){
			if (parent != null)
				return parent.Get (name);
			throw RuntimeException.NotDefined (name);
		}

		public DynValue Put (string name, DynValue value){
			throw new NotSupportedException (name);
		}

		public int Count { get { return values.Length; } }
		public bool HasName(string name){
			throw new NotSupportedException ();
		}
		public bool TryGetValue(string name,out DynValue value){
			throw new NotSupportedException ();
		}
	}
}

