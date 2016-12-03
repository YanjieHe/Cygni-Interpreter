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
	public sealed class ClassInfo: 
	IComputable,IEnumerable<DynValue>, IEnumerator<DynValue>,
	IEquatable<ClassInfo>, IComparable<DynValue>, IDot,IFunction
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
			if (!this.IsInstance) {
				this.body.Eval (this.classScope);
			}
		}

		public ClassInfo Init (DynValue[] parameters)
		{

			var newScope = new NestedScope (classScope.Parent);
			if (this.parent != null) { /* Does this class inherit from a parent class?  */
				newScope.Append (parent.classScope);
			}
			newScope.Append (this.classScope);/* Initialize the class */
			/* If there exists same fields in the derived class, then the fields will be overwriten. */
			ClassInfo newClass = new ClassInfo (
				                     name: name, classScope: newScope, body: body, parent: parent, IsInstance: true);
			newScope.Put ("this", DynValue.FromClass (newClass)); /* pointer to self */
			if (newScope.HasName ("__init")) /* initialize */
				newScope.Get ("__init").As<Function> ().Update (parameters).Invoke ();
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
			throw RuntimeException.FieldNotExist (name, fieldName);
		}

		public DynValue SetByDot (string fieldName, DynValue value)
		{
			return this.classScope.Put (fieldName, value);
		}

		public int CompareTo (DynValue other)
		{
			return (int)this.GetByDot ("__cmp").As<Function> ().Update (new []{ other }).Invoke ().AsNumber ();
		}


		public DynValue Add (DynValue other)
		{
			return this.GetByDot ("__add").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Subtract (DynValue other)
		{
			return this.GetByDot ("__sub").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Multiply (DynValue other)
		{
			return this.GetByDot ("__mul").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Divide (DynValue other)
		{
			return this.GetByDot ("__div").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Modulo (DynValue other)
		{
			return this.GetByDot ("__mod").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Power (DynValue other)
		{
			return this.GetByDot ("__pow").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue UnaryPlus ()
		{
			return this.GetByDot ("__unaryPlus").As<Function> ().Update (new DynValue[0]).Invoke ();
		}


		public DynValue UnaryMinus ()
		{
			return this.GetByDot ("__unaryMinus").As<Function> ().Update (new DynValue[0]).Invoke ();
		}

		public DynValue DynInvoke (DynValue[] args)
		{
			return DynValue.FromClass (this.Init (args));
		}

		public Func<DynValue[],DynValue> AsDelegate ()
		{
			return (args) => DynValue.FromClass (this.Init (args));
		}

		public bool Equals (ClassInfo other)
		{
			DynValue func;
			if (IsInstance && classScope.TryGetValue ("__equals", out func)) {
				return func.As<Function> ().Update (new DynValue[]{ DynValue.FromClass (other) }).Invoke ().AsBoolean ();
			}
			throw RuntimeException.FieldNotExist (name, "__equals");
		}

		public override bool Equals (object obj)
		{
			var other = obj as ClassInfo;
			if (other == null) {
				return false;
			} else {
				return this.Equals (other);
			}
		}

		public override int GetHashCode ()
		{
			return (-1);
		}


		public override string ToString ()
		{
			DynValue func;
			if (IsInstance && classScope.TryGetValue ("__toStr", out func))
				return func.As<Function> ().Update (new DynValue[0]).Invoke ().AsString ();
			else
				return string.Concat ("(class: ", name, ")");
		}

		public IEnumerator<DynValue> GetEnumerator ()
		{
			IEnumerator<DynValue> iterator = 
				this.GetByDot ("__iter")
				.As<Function> ().DynInvoke(DynValue.Empty)
				.As<IEnumerator<DynValue>>();
			while (iterator.MoveNext()) {
				yield return iterator.Current;
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			yield return this.AsEnumerable ();
		}

		public ClassInfo Update (IScope scope)
		{
			var newScope = classScope.Clone ();
			newScope.SetParent (scope);
			var newClass = new ClassInfo (
				               name: name, classScope: newScope, body: null, parent: parent, IsInstance: true);
			return newClass;
		}

		public void Dispose ()
		{
			GC.SuppressFinalize (this);
		}

		public DynValue Current {
			get {
				DynValue current;
				if (classScope.TryGetValue ("__current", out current)) {
					return current;
				}
				throw RuntimeException.FieldNotExist (name, "__current");
			}
		}

		object System.Collections.IEnumerator.Current {
			get {
				DynValue current;
				if (classScope.TryGetValue ("__current", out current)) {
					return current;
				}
				throw RuntimeException.FieldNotExist (name, "__current");
			}
		}
		//
		// Methods
		//
		public bool MoveNext ()
		{
			DynValue func;
			if (classScope.TryGetValue ("__next", out func)) {
				return func.As<Function> ().DynInvoke (DynValue.Empty).AsBoolean ();
			}
			throw RuntimeException.FieldNotExist (name, "__next");
		}

		public void Reset ()
		{
			DynValue func;
			if (classScope.TryGetValue ("__reset", out func)) {
				func.As<Function> ().DynInvoke (DynValue.Empty);
			}
			throw RuntimeException.FieldNotExist (name, "__reset");
		}


	}
}
