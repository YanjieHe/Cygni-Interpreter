using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.Libraries;
using System.Reflection;
namespace Cygni.DataTypes
{
	/// <summary>
	/// Description of NativeFunction.
	/// </summary>
	public sealed class NativeFunction
	{
		readonly Func<DynValue[],DynValue> func;
		public NativeFunction(Func<DynValue[],DynValue> func)
		{
			this.func = func;
		}
		public DynValue Invoke(DynValue[] args)
		{
			return func(args);
		}
		public override string ToString()
		{
			return "(Native Function)";
		}
	}
}
