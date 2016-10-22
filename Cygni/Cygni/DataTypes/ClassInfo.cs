using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.AST;
using Cygni.Errors;

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
			newScope ["this"] = DynValue.FromClass (newClass);
			newClass.InitParents ();/* initialize parents */
			if (!no_arg_construct && newScope.HasName ("__INIT__")) /* initialize */
			newScope ["__INIT__"].As<Function> ().Update (parameters).Invoke ();
			return newClass;
		}

		void InitParents ()
		{
			if (parents != null) { /* has parents */
				for (int i = 0; i < parents.Length; i++) {
					parents [i] = parents [i].Init (null, true);/* no-arg construct parent classes */
					classScope [parents [i].name] = DynValue.FromClass (parents [i]);
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
			return classScope [fieldname] = value;
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
			return (int)classScope ["__COMPARETO__"].As<Function> ().Update (new []{ other }).Invoke ().AsNumber ();
		}

		#endregion

		#region IComputable implementation


		public DynValue Add (DynValue other)
		{
			return classScope ["__ADD__"].As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Subtract (DynValue other)
		{
			return classScope ["__SUBTRACT__"].As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Multiply (DynValue other)
		{
			return classScope ["__MULTIPLY__"].As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Divide (DynValue other)
		{
			return classScope ["__DIVIDE__"].As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Modulo (DynValue other)
		{
			return classScope ["__MODULO__"].As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue Power (DynValue other)
		{
			return classScope ["__POWER__"].As<Function> ().Update (new []{ other }).Invoke ();
		}


		public DynValue UnaryPlus ()
		{
			return classScope ["__UNARYPLUS__"].As<Function> ().Update (new DynValue[0]).Invoke ();
		}


		public DynValue UnaryMinus ()
		{
			return classScope ["__UNARYMINUS__"].As<Function> ().Update (new DynValue[0]).Invoke ();
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
				return classScope ["__TOSTRING__"].As<Function> ().Update (new DynValue[0]).Invoke ().AsString ();
			return string.Concat ("(class: ", name, ")");
		}

		public static ClassInfo CreateClass (IScope parentScope, string className, string[] fields, DynValue[] fieldnames)
		{
			var scope = new NestedScope (parentScope);
			int n = fields.Length;
			if (n != fieldnames.Length)
				throw new ArgumentException ();
			for (int i = 0; i < n; i++) {
				scope [fields [i]] = fieldnames [i];
			}
			return new ClassInfo (className, BlockEx.EmptyBlock, scope, null);
		}
		public static void AddMethod(ClassInfo classInfo,string methodName,string[] parameters, string fieldName, Func<DynValue[],DynValue>f){
			IScope classScope = classInfo.classScope;
			var arguments = new ASTNode[parameters.Length];
			arguments [0] = ASTNode.Dot (ASTNode.Variable ("this"), fieldName);
			for (int i = 1; i < arguments.Length; i++) {
				arguments [i] = ASTNode.Parameter (parameters [i - 1]);
			}
			var func =  new Function (
				methodName,
				parameters,
				ASTNode.Block (new ASTNode[] {
					ASTNode.Invoke (
						ASTNode.Constant (
							DynValue.FromDelegate (
								f)),
						arguments)
				}), new NestedScope (classScope));
			classScope [methodName] = DynValue.FromFunction( func);
		}
	}
}
