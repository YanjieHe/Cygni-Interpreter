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
		public ScopeType type { get { return ScopeType.Basic; } }
		private IScope parent;
		private List<DynValue> values;
		private Dictionary<string, int> table;

		public ResizableArrayScope (IScope parent)
		{
			this.parent = parent;
			this.table = new Dictionary<string,int>();
			this.values = new List<DynValue>();
		}

		public DynValue Get(string name){
			int index;
			if (this.table.TryGetValue(name, out index)) {
				return this.values[index];
			} else {
				return parent.Get (name);
			}
		}
		public DynValue Put (string name, DynValue value){
			int index;
			if (this.table.TryGetValue(name, out index)) {
				return this.values[index] = value;
			} else {
				index = this.values.Count;
				this.values.Add (value);
				this.table.Add (name, index);
				return value;
			}
		}
		public DynValue Get(int index) {
			return this.values[index];
		}
		public DynValue Put(int index,DynValue value) {
			return this.values[index] = value;
		}

		public int Count { get { return this.values.Count;}}
		public bool HasName(string name){
			return this.table.ContainsKey(name);
		}
		public bool TryGetValue(string name,out DynValue value) {
			int index;
			if (this.table.TryGetValue(name, out index)) {
				value = this.values[index];
				return true;
			} else {
				value = DynValue.Nil;
				return false;
			}
		}
		public IScope Parent { get{return this.parent;} }

	}
}

