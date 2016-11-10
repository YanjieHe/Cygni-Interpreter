using System;
namespace Cygni.DataTypes
{
	public interface IFunction
	{
		DynValue DynInvoke (DynValue[] args);
		Func<DynValue[],DynValue> AsDelegate();
	}
}

