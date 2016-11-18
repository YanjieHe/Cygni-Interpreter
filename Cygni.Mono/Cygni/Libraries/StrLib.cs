using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.Errors;

namespace Cygni.Libraries
{
	/// <summary>
	/// Description of StrLib.
	/// </summary>
	public static class StrLib
	{
		public static DynValue strcat (DynValue[] args)
		{
			int n = args.Length;
			RuntimeException.FuncArgsCheck (n >=  1, "strcat");
			string[] strs = new string[n];
			for (int i = 0; i < n; i++)
				strs [i] = args [i].AsString ();
			return string.Concat (strs);
		}
		public static DynValue concat (string str, DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length ==  1, "concat");
			return str + args[0].AsString();
		}
		public static DynValue join (string str, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "join");
			return string.Join (str, args[0].As<DynList>().Select(i=>i.Value));
		}
		public static DynValue format (string str, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length >= 1, "format");
			return string.Format (str, args.Map(i=>i.Value));
		}
		public static DynValue split (string str, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length >= 1, "strsplit");
			string[] result;
			if (args[0].type == DataType.List){
				DynList list = args[0].As<DynList>();
				int n = list.Count;
				char[] arr = new char[n];
				for(int i = 0;i < n;i++)
					arr[i] = char.Parse(list[i].AsString());
				result = str.Split(arr);
			}
			else {
				int n = args.Length;
				char[] arr = new char[n];
				for(int i = 0;i < n;i++)
					arr[i] = char.Parse(args[i].AsString());
				result = str.Split(arr);
			}
			return DynValue.FromList(result.ToDynList(DynValue.FromString));
		}
		public static DynValue replace (string str, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "replace");
			string oldValue = args [0].AsString ();
			string newValue = args [1].AsString ();
			return str.Replace (oldValue, newValue);
		}
		public static DynValue strcmp (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "strcmp");
			return (double)string.Compare (args [0].AsString (),
				args [1].AsString ());
		}
		public static DynValue find (string str, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1 || args.Length == 2 || args.Length == 3, "find");
			if (args.Length == 1)
				return str.IndexOf (args [0].AsString ());
			else if (args.Length == 2)
				return str.IndexOf (args [0].AsString (), (int)args [1].AsNumber ());
			else
				return str.IndexOf (args [0].AsString (), (int)args [1].AsNumber (), (int)args [2].AsNumber ());
		}
		public static DynValue trim(string str, DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 0 || args.Length == 1, "trim");
			if (args.Length == 0)
				return str.Trim ();
			else {
				DynList list = args [0].As<DynList> ();
				int n = list.Count;
				char[] arr = new char[n];
				for (int i = 0; i < n; i++)
					arr [i] = char.Parse (list [i].AsString ());
				return str.Trim (arr);
			}
		}
		public static DynValue trimStart(string str, DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 0 || args.Length == 1, "trimStart");
			if (args.Length == 0)
				return str.TrimStart ();
			else {
				DynList list = args [0].As<DynList> ();
				int n = list.Count;
				char[] arr = new char[n];
				for (int i = 0; i < n; i++)
					arr [i] = char.Parse (list [i].AsString ());
				return str.TrimStart (arr);
			}
		}
		public static DynValue trimEnd(string str, DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length == 0 || args.Length == 1, "trimEnd");
			if (args.Length == 0)
				return str.TrimEnd ();
			else {
				DynList list = args [0].As<DynList> ();
				int n = list.Count;
				char[] arr = new char[n];
				for (int i = 0; i < n; i++)
					arr [i] = char.Parse (list [i].AsString ());
				return str.TrimEnd (arr);
			}
		}
		public static DynValue subString (string str, DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1 || args.Length == 2, "subString");
			if (args.Length == 1)
				return str.Substring ((int)args [1].AsNumber ());
			else
				return str.Substring ((int)args [1].AsNumber (), (int)args [2].AsNumber ());
		}
		
	}
}
