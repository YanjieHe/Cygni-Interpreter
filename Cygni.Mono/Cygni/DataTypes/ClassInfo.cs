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
			// if(!IsInstance)
			// 	body.Eval (classScope); 
		}

		public ClassInfo Init (DynValue[] parameters,bool NonArg = false)
		{

			var newScope = new NestedScope (classScope.Parent);
			ClassInfo parentClass = parent;
			if (this.parent != null) {
				parentClass = this.parent.Init (null, true);
				newScope.Put ("base", DynValue.FromClass (parentClass));
			}
			body.Eval (newScope);
			var newClass = new ClassInfo (name: name, classScope: newScope, body: body, parent: parentClass, IsInstance: true);
			newScope.Put ("this", DynValue.FromClass (newClass)); /* pointer to self */
			if (!NonArg && newScope.HasName ("__INIT__")) /* initialize */
				newScope.Get ("__INIT__").As<Function> ().Update (parameters).Invoke ();
			return newClass;

			//var newScope = classScope.Clone();
			/* var newScope = new NestedScope(classScope.Parent);
			if (this.parent != null) {
				var parentClass = parent.InitWithoutArgs (newScope);*/
				//newScope.Put ("base", DynValue.FromClass (parentClass)); /* add parent class pointer */
				//parent.body.Eval (newScope);
			//}
			/*body.Eval (newScope);
			var newClass = new ClassInfo (name: name, classScope: newScope, body: body, parent: parent, IsInstance: true);
			newScope.Put ("this", DynValue.FromClass (newClass)); */
			/* define 'this' */
//			if (newScope.HasName ("__INIT__")) /* initialize */
/*				newScope.Get ("__INIT__").As<Function> ().Update (parameters).Invoke ();
			return newClass;*/
		}

		/* internal ClassInfo InitWithoutArgs(NestedScope scope){
			//var newScope = new NestedScope(classScope.Parent);
			if (this.parent != null) {
				scope.Put("base", DynValue.FromClass(parent.InitWithoutArgs(scope)));
				parent.body.Eval (scope);
			}
			body.Eval(scope);
			var newClass = new ClassInfo(name: name, classScope: scope, body: body, parent: parent, IsInstance: true);
			//newScope.Put("this", DynValue.FromClass(newClass));
			return newClass;
		} */

		public bool HasParent {
			get { return this.parent != null; }
		}

		public DynValue GetByDot (string fieldName)
		{
			DynValue value;
			if (classScope.TryGetValue (fieldName, out value))/* Find in self */
				return value;
			if (parent != null)
				return parent.GetByDot (fieldName); /* Find in parent */
			throw RuntimeException.FieldNotExist (name, fieldName);
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
		public DynValue SetByDot (string fieldName, DynValue value)
		{
			return classScope.Put (fieldName, value);
		}

		public int CompareTo (DynValue other)
		{
			return (int)classScope.Get ("__COMPARETO__").As<Function> ().Update (new []{ other }).Invoke ().AsNumber ();
		}


		public DynValue Add (DynValue other)
		{
			return classScope.Get ("__ADD__").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Subtract (DynValue other)
		{
			return classScope.Get ("__SUBTRACT__").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Multiply (DynValue other)
		{
			return classScope.Get ("__MULTIPLY__").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Divide (DynValue other)
		{
			return classScope.Get ("__DIVIDE__").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Modulo (DynValue other)
		{
			return classScope.Get ("__MODULO__").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Power (DynValue other)
		{
			return classScope.Get ("__POWER__").As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue UnaryPlus ()
		{
			return classScope.Get ("__UNARYPLUS__").As<Function> ().Update (new DynValue[0]).Invoke ();
		}


		public DynValue UnaryMinus ()
		{
			return classScope.Get ("__UNARYMINUS__").As<Function> ().Update (new DynValue[0]).Invoke ();
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
				return classScope.Get ("__TOSTRING__").As<Function> ().Update (new DynValue[0]).Invoke ().AsString ();
			else
				return string.Concat ("(class: ", name, ")");
		}

		public IEnumerator<DynValue> GetEnumerator ()
		{
			if (classScope.HasName ("__COLLECTION__")) {
				var collection = classScope.Get ("__COLLECTION__").As<Function> ().Invoke ().As<IEnumerable<DynValue>> ();
				foreach (var item in collection) 
					yield return item;
			} else {
				classScope.Get ("__ITER__").As<Function> ().Invoke ();
				var next = classScope.Get ("__Next__").As<Function> ().AsDelegate ();
				var current = classScope.Get ("__CURRENT__").As<Function> ().AsDelegate ();
				while (next (new DynValue[0]).AsBoolean()) {
					yield return current (new DynValue[0]);
				}
			}
		}

		System.Collections.IEnumerator  System.Collections.IEnumerable.GetEnumerator ()
		{
			yield return this.AsEnumerable ();
		}

		public ClassInfo Update(IScope scope) {
			var newScope = classScope.Clone ();
			newScope.SetParent (scope);
			var newClass = new ClassInfo (name: name, classScope: newScope, body: null, parent: parent, IsInstance: true);
			return newClass;
		}
	}
}
