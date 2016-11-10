using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Cygni.DataTypes
{
	/// <summary>
	/// Description of IComputable.
	/// </summary>
	public interface IComputable
	{
		DynValue Add(DynValue other);
		DynValue Subtract(DynValue other);
		DynValue Multiply(DynValue other);
		DynValue Divide(DynValue other);
		DynValue Modulo(DynValue other);
		DynValue Power(DynValue other);
		
		DynValue UnaryPlus();
		DynValue UnaryMinus();
		
	}
}
