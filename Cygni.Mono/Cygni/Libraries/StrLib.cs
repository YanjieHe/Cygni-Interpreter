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
			RuntimeException.FuncArgsCheck (args.Length ==  1, "strcat");
			return string.Concat(args[0].As<DynList>().Select(i=>i.Value));
		}

		public static DynValue strjoin (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "strjoin");
			return string.Join (args [0].AsString (), args[1].As<DynList>().Select(i=>i.Value));
		}

		public static DynValue strformat (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length > 1, "strformat");
			return string.Format (args [0].AsString (), args.SkipMap (1, i => i.Value));
		}

		public static DynValue strlen (DynValue[] args)
		{
			return (double)args [0].AsString ().Length;
		}

		public static DynValue strsplit (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length > 1, "strsplit");
			string str = args[0].AsString();
			string[] result;
			if (args[1].type == DataType.List){
				DynList list = args[1].As<DynList>();
				int n = list.Count;
				char[] arr = new char[n];
				for(int i = 0;i < n;i++)
					arr[i] = char.Parse(list[i].AsString());
				result = str.Split(arr);
			}
			else {
				int n = args.Length - 1;
				char[] arr = new char[n];
				for(int i = 0;i < n;i++)
					arr[i] = char.Parse(args[i+1].AsString());
				result = str.Split(arr);
			}
			return DynValue.FromList(result.ToDynList(DynValue.FromString));
		}

		public static DynValue replace (string ,DynValue[] args)
		{/* string replace */
			RuntimeException.FuncArgsCheck (args.Length == 2, "replace");
			return args [0].AsString ().Replace (
				args [1].AsString (),
				args [2].AsString ());
		}

		public static DynValue strrpl (DynValue[] args)
		{/* string replace */
			RuntimeException.FuncArgsCheck (args.Length == 3, "strrpl");
			return args [0].AsString ().Replace (
				args [1].AsString (),
				args [2].AsString ());
		}

		public static DynValue strcmp (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "strcmp");
			return (double)string.Compare (args [0].AsString (),
				args [1].AsString ());
		}

		public static DynValue strfind (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2 || args.Length == 3 || args.Length == 4, "strcmp");
			if (args.Length == 2)
				return args [0].AsString ().IndexOf (args [1].AsString ());
			else if (args.Length == 3)
				return args [0].AsString ().IndexOf (args [1].AsString (), (int)args [2].AsNumber ());
			else
				return args [0].AsString ().IndexOf (args [1].AsString (), (int)args [2].AsNumber (), (int)args [3].AsNumber ());
		}

		public static DynValue tolower (DynValue[]args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "tolower");
			return args [0].AsString ().ToLower ();
		}

		public static DynValue toupper (DynValue[]args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "toupper");
			return args [0].AsString ().ToUpper ();
		}

		public static DynValue _char (DynValue[]args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "char");
			return char.ToString ((char)(int)args [0].AsNumber ());
		}

		public static DynValue Trim (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length >= 1, "string.trim");
			if (args.Length == 1)
				return args [0].AsString ().Trim ();
			else
				return args [0].AsString ().Trim (args.SkipMap (1, i => char.Parse (i.AsString ())));
		}
		
		public static DynValue TrimStart (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length >= 1, "string.trimStart");
			string str = args[0].AsString();
			if (args.Length == 1)
				return str.TrimStart();
			else
				return str.TrimStart(args.SkipMap(1, i => char.Parse(i.AsString())));
		}

		public static DynValue TrimEnd (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length >= 1, "string.trimEnd");
			string str = args[0].AsString();
			if (args.Length == 1)
				return str.TrimEnd();
			else
				return str.TrimEnd(args.SkipMap(1, i => char.Parse(i.AsString())));
		}
		public static DynValue SubString (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2 || args.Length == 3, "string.subString");
			if (args.Length == 2)
				return args [0].AsString ().Substring ((int)args [1].AsNumber ());
			else
				return args [0].AsString ().Substring ((int)args [1].AsNumber (), (int)args [2].AsNumber ());
		}

		
	}
}
