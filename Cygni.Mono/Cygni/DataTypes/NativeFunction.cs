using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.Libraries;
using System.Reflection;
using Cygni.AST;
using Cygni.AST.Scopes;

namespace Cygni.DataTypes
{
	/// <summary>
	/// Description of NativeFunction.
	/// </summary>
	public sealed class NativeFunction:IFunction
	{
		readonly string name;
		readonly Func<DynValue[],DynValue> func;

		public NativeFunction (string name, Func<DynValue[],DynValue> func)
		{
			this.name = name;
			this.func = func;
		}

		public DynValue Invoke (DynValue[] args)
		{
			return func (args);
		}

		public DynValue DynInvoke (DynValue[] args)
		{
			return func (args);
		}

		public DynValue DynEval (ASTNode[]args, IScope scope)
		{
			DynValue[] arguments = new DynValue[args.Length];
			for (int i = 0; i < args.Length; i++) {
				arguments [i] = args [i].Eval (scope);
			}
			return func (arguments);
		}

		public Func<DynValue[],DynValue> AsDelegate ()
		{
			return func;
		}

		public override string ToString ()
		{
			return string.Concat("(Native Function: ", name, ")");
		}
	}
}
