using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Errors;

namespace Cygni.AST.Scopes
{
	public class ResizableArrayScope:IScope
	{

		private readonly List<DynValue> values;
		private readonly Dictionary<string, int> table;

		public ScopeType type { get { return ScopeType.ResizableArray; } }

		public int Count { get { return this.values.Count; } }

		public IScope Parent { get { return null; } }

		public ResizableArrayScope ()
		{
			this.table = new Dictionary<string,int> ();
			this.values = new List<DynValue> ();
		}

		public int Find (string name)
		{
			int index;
			if (this.table.TryGetValue (name, out index)) {
				return index;
			} else {
				throw RuntimeException.NotDefined (name);
			}
		}

		public DynValue Get (string name)
		{
			int index;
			if (this.table.TryGetValue (name, out index)) {
				return this.values [index];
			} else {
				throw RuntimeException.NotDefined (name);
			}
		}

		public DynValue Put (string name, DynValue value)
		{
			int index;
			if (this.table.TryGetValue (name, out index)) {
				return this.values [index] = value;
			} else {
				index = this.values.Count;
				this.values.Add (value);
				this.table.Add (name, index);
				return value;
			}
		}

		public DynValue Get (int nest, int index)
		{
			if (nest == 0) {
				return this.values [index];
			} else {
				throw RuntimeException.NotDefined ("arg" + index);
			}
		}

		public DynValue Put (int nest, int index, DynValue value)
		{
			if (nest == 0) {
				return this.values [index] = value;
			} else {
				throw RuntimeException.NotDefined ("arg" + index);
			}
		}

		public bool HasName (string name)
		{
			return this.table.ContainsKey (name);
		}

		public bool TryGetValue (string name, out DynValue value)
		{
			int index;
			if (this.table.TryGetValue (name, out index)) {
				value = this.values [index];
				return true;
			} else {
				value = DynValue.Nil;
				return false;
			}
		}

	}
}

