using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.AST;
using Cygni.Errors;
using Cygni.AST.Scopes;

namespace Cygni.DataTypes
{
	/// <summary>
	/// Description of ClassInfo.
	/// </summary>
	public sealed class ClassInfo: IComputable, IComparable<DynValue>, IDot,IFunction
	{
		readonly string name;
		BlockEx body;
		NestedScope classScope;
		ClassInfo[] parents;

		public int ParametersCount{ get { throw new NotImplementedException(); } }
		public ClassInfo (string name, BlockEx body, NestedScope classScope, ClassInfo[] parents = null)
		{
			this.name = name;
			this.body = body;
			this.classScope = classScope;
			this.parents = parents;
		}

		public ClassInfo Init (DynValue[] parameters, bool no_arg_construct = false)
		{
			var newScope = new NestedScope (classScope.Parent);
			body.Eval (newScope);
			var newClass = new ClassInfo (name, body, newScope, parents);
			newScope.Put ("this", DynValue.FromClass (newClass));
			newClass.InitParents ();/* initialize parents */
			if (!no_arg_construct && newScope.HasName ("__INIT__")) /* initialize */
				newScope.Get ("__INIT__").As<Function> ().Update (parameters).Invoke ();
			return newClass;
		}

		void InitParents ()
		{
			if (parents != null) { /* has parents */
				for (int i = 0; i < parents.Length; i++) {
					parents [i] = parents [i].Init (null, true);/* no-arg construct parent classes */
					classScope.Put (parents [i].name, DynValue.FromClass (parents [i]));
					/* add parent class pointers */
				}
			}
		}

		#region IDot implementation

		public DynValue GetByDot (string fieldname)
		{
			return Search (fieldname);
		}

		public DynValue SetByDot (string fieldname, DynValue value)
		{
			return classScope.Put (fieldname, value);
		}

		#endregion

		DynValue Search (string fieldname)
		{
			DynValue value;
			if (classScope.TryGetValue (fieldname, out value))/* Find in self */
				return value;
			
			foreach (var parent in parents) { /* Find in parents */
				if (parent.classScope.TryGetValue (fieldname, out value))
					return value;
			}
			throw RuntimeException.NotDefined (fieldname);
		}

		#region IComparable implementation

		public int CompareTo (DynValue other)
		{
			return (int)classScope.Get ("__COMPARETO__").As<Function> ().Update (new []{ other }).Invoke ().AsNumber ();
		}

		#endregion

		#region IComputable implementation


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

		#endregion

		public override string ToString ()
		{
			if (classScope.HasName ("this") && classScope.HasName ("__TOSTRING__"))
				return classScope.Get("__TOSTRING__").As<Function> ().Update (new DynValue[0]).Invoke ().AsString ();
			return string.Concat ("(class: ", name, ")");
		}
	}
}
