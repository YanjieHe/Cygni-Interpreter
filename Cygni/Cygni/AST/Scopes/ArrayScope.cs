using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygni.DataTypes;
using Cygni.Errors;

namespace  Cygni.AST.Scopes
{
	public class ArrayScope:IScope
	{
		DynValue[] values;
		string[] names;
		IScope parent;
		public IScope Parent{ get { return parent; } }
		public ArrayScope (int capacity, IScope parent = null)
		{
			values = new DynValue[capacity];
			names = new string[capacity];
			this.parent = parent;
		}
		public int Count{ get { return values.Length; } }
		public DynValue this[int nest,int index] {
			get {
				if (nest == 0)
					return values [index];
				if (parent == null)
					throw RuntimeException.NotDefined (names [index]);
				return ((ArrayScope)parent) [nest - 1, index];
			}
			set {
				if (nest == 0)
					values [index] = value;
				if (parent == null)
					throw new RuntimeException ("Missing parent scope");
				((ArrayScope)parent) [nest - 1, index] = value;
			}
		}


		#region IScope implementation
		public DynValue this[string name] {
			get {
				throw new NotSupportedException ();
			}
			set {
				throw new NotSupportedException ();
			}
		}
		#endregion
		public bool HasName(string name){
			throw new NotSupportedException ();
		}

		public bool TryGetValue(string name, out DynValue value)
		{
			throw new NotSupportedException ();
		}
	}
}

