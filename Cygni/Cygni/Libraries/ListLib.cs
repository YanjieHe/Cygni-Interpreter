using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.Errors;
using Cygni.Settings;
namespace Cygni.Libraries
{
	/// <summary>
	/// Description of ListLib.
	/// </summary>
	public static class ListLib
	{
		public static DynValue append(DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length >= 2, "append");
			args[0].As<DynList>().AddRange(args.Skip(1));
			return DynValue.Null;
		}
		public static DynValue len(DynValue[] args)
		{
			return (double)args[0].As<DynList>().Count;
		}
		public static DynValue remove_at(DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "remove_at");
			args[0].As<DynList>().RemoveAt((int)args[1].AsNumber());
			return DynValue.Null;
		}
		public static DynValue insert_at(DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 3, "insert_at");
			args[0].As<DynList>().Insert((int)args[1].AsNumber(),args[2]);
			return DynValue.Null;
		}
	}
}
