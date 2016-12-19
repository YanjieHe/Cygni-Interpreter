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

		private readonly DynValue[] values;
		private readonly IScope parent;
		private readonly string name;

		public IScope Parent{ get { return parent; } }

		public ScopeType type { get { return ScopeType.Array; } }

		public int Count { get { return values.Length; } }

		public string Name { get { return this.name; } }

		public ArrayScope (string name, DynValue[] values, IScope parent = null)
		{
			this.name = name;
			this.values = values;
			this.parent = parent;
		}

		public DynValue Get (int nest, int index)
		{
			if (nest == 0) {
				return this.values [index];	
			} else {
				return this.parent.Get (nest - 1, index);
			}
		}

		public DynValue Put (int nest, int index, DynValue value)
		{
			if (nest == 0) {
				return this.values [index] = value;	
			} else {
				return this.parent.Put (nest - 1, index, value);
			}
		}

		public DynValue Get (string name)
		{
			if (parent != null)
				return parent.Get (name);
			throw RuntimeException.NotDefined (name);
		}

		public DynValue Put (string name, DynValue value)
		{
			throw new NotSupportedException (name);
		}

		public bool HasName (string name)
		{
			throw new NotSupportedException ();
		}

		public bool TryGetValue (string name, out DynValue value)
		{
			throw new NotSupportedException ();
		}

		public ArrayScope Update (IScope scope)
		{
			var values = new DynValue[this.values.Length];
			Array.Copy (this.values, values,values.Length);
			return new ArrayScope (name, values, scope);
		}
	}
}

