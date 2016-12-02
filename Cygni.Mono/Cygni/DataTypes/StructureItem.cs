using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.Errors;
namespace Cygni.DataTypes
{
	public sealed class StructureItem
	{
		public readonly string Key;
		public DynValue Value;
		internal StructureItem(string Key,DynValue Value){
			this.Key = Key;
			this.Value = Value;
		}
	}
}

