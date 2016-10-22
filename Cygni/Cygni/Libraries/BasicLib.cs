using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.AST;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.Errors;
using Cygni.Settings;
namespace Cygni.Libraries
{
	/// <summary>
	/// Description of BasicLib.
	/// </summary>
	public static class BasicLib
	{
		public static DynValue  print(DynValue[] args)
		{
			if (args == null || args.Length == 0)
				return DynValue.Null;
			Console.Write(args[0].Value);
			for (int i = 1; i < args.Length; i++) {
				Console.Write('\t');
				Console.Write(args[i].Value);
			}
			Console.WriteLine();
			return DynValue.Null;
		}
		public static DynValue printf(DynValue[] args)
		{
			var arguments = new object[args.Length - 1];
			for (int i = 0; i < arguments.Length; i++)
				arguments[i] = args[i + 1].Value;
			Console.WriteLine(args[0].AsString(), arguments);
			return DynValue.Null;
		}
		public static DynValue Struct(DynValue[] args)
		{
			if ((args.Length & 1) == 0) {/* even */
				var dict = new Dictionary<string,DynValue>();
				for (int i = 0; i < args.Length - 1; i += 2)
					dict.Add(args[i].AsString(), args[i + 1]);
				return new DynValue(DataType.Struct, dict as Structure);
			} 
			throw RuntimeException.BadArgsNum("struct", "even");
		}
		public static DynValue cast(DynValue[] args)
		{
			if (args.Length != 2)
				throw RuntimeException.BadArgsNum("cast", 2);
			string typeName = args[1].AsString();
			object obj = args[0].Value;
			switch (typeName.ToLower()) {
				case "int16":
					return DynValue.FromUserData(Convert.ToInt16(obj));
				case "int":
				case "int32":
					return DynValue.FromUserData(Convert.ToInt32(obj));
				case "long":
				case "int64":
					return DynValue.FromUserData(Convert.ToInt64(obj));
				case "float":
				case "single":
					return DynValue.FromUserData(Convert.ToSingle(obj));
				case "double":
				case "number":
					return DynValue.FromNumber(Convert.ToDouble(obj));
				case "bool":
				case "boolean":
					return DynValue.FromBoolean(Convert.ToBoolean(obj));
				case "string":
					return DynValue.FromString(Convert.ToString(obj));
				case "char":
					return DynValue.FromUserData(Convert.ToChar(obj));
				case "datetime":
				case"date":
				case "time":
					return DynValue.FromUserData(Convert.ToDateTime(obj));
				default:
					return DynValue.FromObject(Convert.ChangeType(obj, Type.GetType(typeName)));
			}
		}
		public static DynValue typeinfo(DynValue[] args)
		{
			if (args[0].type != DataType.UserData)
				return DynValue.FromString(args[0].type.ToString());
			else
				return DynValue.FromString(args[0].GetDynType().ToString());
		}
		public static DynValue tonumber(DynValue[] args)
		{
			if (args[0].type == DataType.Number)
				return args[0];
			else
				return Convert.ToDouble(args[0].Value);
		}
		public static DynValue tostring(DynValue[] args)
		{
			if (args[0].type == DataType.String)
				return args[0];
			else
				return Convert.ToString(args[0].Value);
		}
		public static DynValue quiet(DynValue[] args)
		{
			if (args.Length == 0) {
				GlobalSettings.Quiet = true;
			} else {
				GlobalSettings.Quiet = args[0].AsBoolean();
			}
			return GlobalSettings.Quiet;
		}
		public static DynValue scan(DynValue[] args)
		{
			if (args.Length == 1)
				Console.Write(args[0].AsString());
			return Console.ReadLine();
		}
		public static DynValue clock(DynValue[] args)
		{
			return (double)DateTime.Now.Ticks;
		}
		public static DynValue bit_or(DynValue[] args)
		{
			int value = (int)args[0].AsNumber();
			for (int i = 1; i < args.Length; i++) {
				value |= (int)args[i].AsNumber();
			}
			return (double)value;
		}
	}
}
