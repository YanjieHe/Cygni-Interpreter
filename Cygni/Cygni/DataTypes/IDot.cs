using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Cygni.DataTypes
{
	/// <summary>
	/// Description of IDot.
	/// </summary>
	public interface IDot
	{
		DynValue GetByDot(string fieldname);
		DynValue SetByDot(string fieldname, DynValue value);
		
	}
}
