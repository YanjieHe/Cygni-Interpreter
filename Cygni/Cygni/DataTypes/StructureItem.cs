using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.Errors;
namespace Cygni.DataTypes
{
	internal sealed class StructureItem
	{
		internal readonly string Key;
		internal DynValue Value;
		public StructureItem(string Key,DynValue Value){
			this.Key = Key;
			this.Value = Value;
		}
	}
}

