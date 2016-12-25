using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.AST;
using Cygni.Errors;
using Cygni.AST.Scopes;
using Cygni.DataTypes.Interfaces;

namespace Cygni.DataTypes
{
	public sealed class DynObject:IDot,IFunction
	{
		readonly string name;
		readonly ClassScope classScope;
		readonly DynObject parent;

		public DynObject Parent{ get { return this.parent; } }

		readonly bool isInstance;

		public DynObject (string name, ClassScope classScope, bool isInstance, DynObject parent = null)
		{
			this.name = name;
			this.classScope = classScope;
			this.parent = parent;
			this.isInstance = isInstance;
		}

		public DynValue GetByDot (string fieldName)
		{
			return classScope.Get (fieldName);
		}

		public DynValue SetByDot (string fieldName, DynValue value)
		{
			return classScope.Put (fieldName, value);
		}

		public string[] FieldNames{ get { return this.classScope.FieldNames (); } }

		public DynValue DynInvoke (DynValue[] args)
		{
			throw new NotSupportedException ();
		}

		public DynValue DynEval (ASTNode[] args, IScope scope)
		{
			if (!isInstance) {
				DynValue constructor;
				if (classScope.TryGetValue ("__init", out constructor)) {
					var newClass = new DynObject (name, this.classScope.Clone (), true, this.parent);
					newClass.SetByDot ("this", newClass);
					newClass.GetByDot ("__init").As<IFunction> ().DynEval (args, scope);
					return newClass;
				} else {
					var newClass = new DynObject (name, this.classScope.Clone (), true, this.parent);
					newClass.SetByDot ("this", newClass);
					return newClass;
				}
			} else {
				throw new RuntimeException ("class '{0}' is an instance, which cannot be used as a constructor.", this.classScope.Name);
			}
		}

		internal Dictionary<string, DynValue> GetFields ()
		{
			Dictionary<string, DynValue> fields;
			if (this.parent == null) {
				fields = new Dictionary<string, DynValue> ();
			} else {
				fields = this.parent.GetFields ();
			}
			foreach (string field in this.FieldNames) {
				if (!string.Equals (field, "this")) {
					fields.Add (field, this.GetByDot (field));
				}
			}
			return fields;
		}

		public override string ToString ()
		{
			return "(Class: " + this.name + ")";
		}
	}
}

