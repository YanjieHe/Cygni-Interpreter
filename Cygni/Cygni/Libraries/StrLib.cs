using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Extensions;
namespace Cygni.Libraries
{
	/// <summary>
	/// Description of StrLib.
	/// </summary>
	public static class StrLib
	{
		public static DynValue strcat(DynValue[] args)
		{
			return string.Concat(args.Map(i => i.AsString()));
		}
		public static DynValue strjoin(DynValue[] args)
		{
			return string.Join(args[0].AsString(), args.SkipMap(1, i => i.AsString()));
		}
		public static DynValue strformat(DynValue[] args)
		{
			return string.Format(args[0].AsString(), args.SkipMap(1, i => i.Value));
		}
		public static DynValue strlen(DynValue[] args)
		{
			return (double)args[0].AsString().Length;
		}
		public static DynValue strsplit(DynValue[] args)
		{
			return DynValue.FromList(args[0].AsString()
			                         .Split(args.SkipMap(1, i => char.Parse(i.AsString())))
			                         .ToDynList(DynValue.FromString));
		}
		public static DynValue getencoding(DynValue[] args)
		{
			return DynValue.FromObject(Encoding.GetEncoding(args[0].AsString()));
		}
	}
}
