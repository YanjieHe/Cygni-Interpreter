using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Cygni.DataTypes.Interfaces
{
	public interface ISliceable
	{
		DynValue GetBySlice (Range range);

		DynValue SetBySlice (Range range, DynValue value);
	}
}

