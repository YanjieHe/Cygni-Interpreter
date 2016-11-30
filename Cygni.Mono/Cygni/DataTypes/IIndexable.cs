using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Cygni.DataTypes
{
	/// <summary>
	/// Description of IIndexable.
	/// </summary>
	public interface IIndexable
	{
		DynValue GetByIndex (DynValue index);
		DynValue SetByIndex (DynValue index, DynValue value);

		DynValue GetByIndexes (DynValue[] indexes);
		DynValue SetByIndexes (DynValue[] indexes, DynValue value);
	}
}
