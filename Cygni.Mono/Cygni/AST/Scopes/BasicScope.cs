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
		public ScopeType type { get { return ScopeType.Basic; } }

		protected readonly EnvTable envTable;
		public static BuiltInScope builtInScope;
		public int Nest{ get { throw new Exception (); } }
		public BasicScope()
		{
			this.envTable = new EnvTable ();
		}

		public int Count { get { return envTable.Count; } }

		public virtual IScope Parent {
			get {
				return builtInScope;
			}
		}
		public virtual DynValue Get(string name){
			DynValue _value;
			if (envTable.TryGetValue (name, out _value))
				return _value;
			return builtInScope.Get (name);
			//throw RuntimeException.NotDefined (name);
		}
		public virtual DynValue Put (string name, DynValue value){
			return envTable[name] = value;
		}
		public virtual bool HasName(string name) {
			return envTable.ContainsKey(name);
		}
		public virtual bool TryGetValue(string name,out DynValue value){
			return envTable.TryGetValue (name, out value);
		}
		public DynValue Get(int nest, int index){
			throw new NotSupportedException ();
		}
		public DynValue Put(int nest, int index,DynValue value){
			throw new NotSupportedException ();
		}
		public bool Delete(string name){
			return envTable.Remove (name);
		}
		public void Clear(){
			this.envTable.Clear ();
		}
		public EnvTable GetTable() {
			return this.envTable;
		}
		public override string ToString ()
		{
			var s = new StringBuilder ("Scope: ");
			s.AppendLine ();
			foreach (var item in envTable) {
				s.Append ('\t');
				s.AppendLine (item.ToString ());
			}
			return s.ToString ();
		}
	}
}
