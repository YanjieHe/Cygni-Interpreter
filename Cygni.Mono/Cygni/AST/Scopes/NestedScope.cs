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
	public sealed class NestedScope: BasicScope
	{
		public new ScopeType type { get { return ScopeType.Nested; } }
		IScope parent;

		public override IScope Parent{ get { return parent; } }

		public NestedScope (IScope parent = null) : base ()
		{
			this.parent = parent;
		}

		public override DynValue Get (string name)
		{
			DynValue value;
			if (this.envTable.TryGetValue (name, out value))
				return value;
			if (parent == null)
				throw RuntimeException.NotDefined (name);
			return parent.Get (name);
		}

		public override bool TryGetValue (string name, out DynValue value)
		{
			if (this.envTable.TryGetValue (name, out value))
				return true;
			if (parent == null)
				return false;
			return parent.TryGetValue (name, out value);
		}

		public void SetParent(IScope scope){
			this.parent = scope;
		}
		public NestedScope Clone ()
		{
			var newScope = new NestedScope (this.parent);
			foreach (var variable in base.envTable) {
				DynValue value = variable.Value;
				if (value.type == DataType.Function) {
					newScope.envTable [variable.Key] = value.As<Function> ().Update (newScope);
				} else if (value.type == DataType.Class) {
					newScope.envTable [variable.Key] = value.As<ClassInfo> ().Update (newScope);
				}
				else
					newScope.envTable [variable.Key] = variable.Value;
			}
			return newScope;
		}
		public void Append(NestedScope scope) {
			foreach (var variable in scope.envTable) {
				DynValue value = variable.Value;
				if (value.type == DataType.Function) {
					this.envTable [variable.Key] = value.As<Function> ().Update (this);
				} else if (value.type == DataType.Class) {
					this.envTable [variable.Key] = value.As<ClassInfo> ().Update (this);
				}
				else
					this.envTable [variable.Key] = variable.Value;
			}
		}
		public IEnumerable<string> Names(){
			foreach (var name in envTable.Keys)
				yield return name;
		}
			
	}
}
