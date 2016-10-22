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
		DynValue this[DynValue[] indexes]{ get; set; }
	}
}
