using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.AST;
using Cygni.Errors;
using Cygni.AST.Scopes;
using System.Collections;

namespace Cygni.DataTypes
{
	/// <summary>
	/// Description of ClassInfo.
	/// </summary>
	public sealed class ClassInfo: IComputable,IEnumerable<DynValue>, IComparable<DynValue>, IDot,IFunction
	{
		readonly string name;
		readonly BlockEx body;
		readonly NestedScope classScope;
		readonly ClassInfo parent;
		readonly bool IsInstance;

		public ClassInfo (string name, NestedScope classScope, 
		                  BlockEx body, ClassInfo parent = null, bool IsInstance = false)
		{
			this.name = name;
			this.classScope = classScope;
			this.parent = parent;
			this.IsInstance = IsInstance;
			this.body = body;
		}

		public ClassInfo Init (DynValue[] parameters /*, bool NonArg = false */)
		{

			var newScope = new NestedScope (classScope.Parent);
			// ClassInfo parentClass = parent;
			if (this.parent != null) { /* Does this class inherit from a parent class?  */
			//	parentClass = this.parent.Init (parameters: null, NonArg: true); 
				/* Yes, it does. Initialize its parent class. */
			//	newScope.Put ("base", DynValue.FromClass (parentClass)); 
				/* set 'base' as a pointer to its parent class. */
				parent.body.Eval(newScope);
			}
			body.Eval (newScope); /* Initialize the class */
			/* If there exists same fields in the derived class, then the field will be overwriten. */
			ClassInfo newClass = new ClassInfo (name: name, classScope: newScope, body: body, parent: parent, IsInstance: true);
			newScope.Put ("this", DynValue.FromClass (newClass)); /* pointer to self */
			if (/* !NonArg && */ newScope.HasName ("__INIT__")) /* initialize */
				newScope.Get ("__INIT__").As<Function> ().Update (parameters).Invoke ();
			return newClass;

		}

		public bool HasParent {
			get { return this.parent != null; }
		}

		public string[] FieldNames {
			get { 
				string[] names = new string[classScope.Count];
				int i = 0;
				foreach (var item in classScope.Names()) {
					names [i] = item;
					i++;
				}
				return names;
			}
		}
		
		public DynValue GetByDot (string fieldName)
		{
			DynValue value;
			if (classScope.TryGetValue (fieldName, out value))/* Find in self */
				return value;
			/* if (parent != null)
				return parent.GetByDot (fieldName); /* Find in parent */ 
			throw RuntimeException.FieldNotExist (name, fieldName);
		}

		public DynValue SetByDot (string fieldName, DynValue value)
		{
			// if (parent == null) {
			// 	return this.classScope.Put (fieldName, value);
			// } else if (this.parent.HasField (fieldName)) {
			// 	return this.parent.SetByDot (fieldName, value);
			// } else {
			// 	return this.classScope.Put (fieldName, value);
			// }
			return this.classScope.Put(fieldName, value);
		}

		// private bool HasField(string fieldName){
		// 	if (parent == null) {
		// 		return this.classScope.HasName (fieldName);
		// 	} else {
		// 		if (this.parent.HasField (fieldName))
		// 			return true;
		// 		else
		// 			return this.classScope.HasName (fieldName);
		// 	}
		// }

		public int CompareTo (DynValue other)
		{
			return (int)this.GetByDot ("__CMP__").As<Function> ().Update (new []{ other }).Invoke ().AsNumber ();
		}


		public DynValue Add (DynValue other)
		{
			return this.GetByDot ("__ADD__").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Subtract (DynValue other)
		{
			return this.GetByDot ("__SUB__").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Multiply (DynValue other)
		{
			return this.GetByDot ("__MUL__").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Divide (DynValue other)
		{
			return this.GetByDot ("__DIV__").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Modulo (DynValue other)
		{
			return this.GetByDot ("__MOD__").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Power (DynValue other)
		{
			return this.GetByDot ("__POW__").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue UnaryPlus ()
		{
			return this.GetByDot ("__UNARYPLUS__").As<Function> ().Update (new DynValue[0]).Invoke ();
		}


		public DynValue UnaryMinus ()
		{
			return this.GetByDot ("__UNARYMINUS__").As<Function> ().Update (new DynValue[0]).Invoke ();
		}

		public DynValue DynInvoke (DynValue[] args)
		{
			return DynValue.FromClass (this.Init (args));
		}

		public Func<DynValue[],DynValue> AsDelegate ()
		{
			return (args) => DynValue.FromClass (this.Init (args));
		}


		public override string ToString ()
		{
			if (IsInstance && classScope.HasName ("__TOSTRING__"))
				return this.GetByDot ("__TOSTRING__").As<Function> ().Update (new DynValue[0]).Invoke ().AsString ();
			else
				return string.Concat ("(class: ", name, ")");
		}

		public IEnumerator<DynValue> GetEnumerator ()
		{
			if (classScope.HasName ("__COLLECTION__")) {
				var collection = this.GetByDot ("__COLLECTION__").As<Function> ().Invoke ().As<IEnumerable<DynValue>> ();
				foreach (var item in collection)
					yield return item;
			} else {
				this.GetByDot ("__ITER__").As<Function> ().Invoke ();
				var next = this.GetByDot ("__Next__").As<Function> ().AsDelegate ();
				var current = this.GetByDot ("__CURRENT__").As<Function> ().AsDelegate ();
				while (next (new DynValue[0]).AsBoolean ()) {
					yield return current (new DynValue[0]);
				}
			}
		}

		System.Collections.IEnumerator  System.Collections.IEnumerable.GetEnumerator ()
		{
			yield return this.AsEnumerable ();
		}

		public ClassInfo Update (IScope scope)
		{
			var newScope = classScope.Clone ();
			newScope.SetParent (scope);
			var newClass = new ClassInfo (name: name, classScope: newScope, body: null, parent: parent, IsInstance: true);
			return newClass;
		}
	}
}
