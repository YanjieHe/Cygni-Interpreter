using System;
using Cygni.DataTypes;
using Cygni.Errors;
namespace CygniLib.data
{
	public static class data_funcs
	{
		public static DynValue datatable(DynValue[] args){
			return DynValue.FromUserData (new CygniLib.data.datatable ());
		}
	}
}

