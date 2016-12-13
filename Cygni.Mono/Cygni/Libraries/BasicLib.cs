using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.AST;
using Cygni.AST.Scopes;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.Errors;
using System.Reflection;
using System.IO;
using Cygni.Settings;
using System.Threading;
using Cygni.Executors;

namespace Cygni.Libraries
{
	/// <summary>
	/// Description of BasicLib.
	/// </summary>
	public static class BasicLib
	{
		public static DynValue print (DynValue[] args)
		{
			if (args == null || args.Length == 0)
				return DynValue.Nil;
			Console.Write (args [0].Value);
			for (int i = 1; i < args.Length; i++) {
				Console.Write ('\t');
				Console.Write (args [i].Value);
			}
			Console.WriteLine ();
			return DynValue.Nil;
		}

		public static DynValue printf (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length >= 1, "printf");
			var arguments = new object[args.Length - 1];
			for (int i = 0; i < arguments.Length; i++)
				arguments [i] = args [i + 1].Value;
			Console.WriteLine (args [0].AsString (), arguments);
			return DynValue.Nil;
		}

		public static DynValue input (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length <= 1, "input");
			if (args.Length == 0) {
				return Console.ReadLine ();
			} else {
				Console.Write (args [0].Value);
				return Console.ReadLine ();
			}
		}

		public static DynValue Struct (DynValue[] args)
		{
			if ((args.Length & 1) == 0) {/* even */
				var structureItems = new StructureItem [args.Length >> 1];
				for (int i = 0, j = 0; i < args.Length - 1; i += 2,j++)
					structureItems [j] = new StructureItem (args [i].AsString (), args [i + 1]);
				return new DynValue (DataType.Struct, new Structure (structureItems));
			} 
			throw RuntimeException.BadArgsNum ("struct", "even");
		}

		public static DynValue cast (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "cast");
			string typeName = args [1].AsString ();
			object obj = args [0].Value;
			switch (typeName.ToLower ()) {
				case "int16":
					return DynValue.FromUserData (Convert.ToInt16 (obj));
				case "int":
				case "int32":
					return DynValue.FromUserData (Convert.ToInt32 (obj));
				case "long":
				case "int64":
					return DynValue.FromUserData (Convert.ToInt64 (obj));
				case "float":
				case "single":
					return DynValue.FromUserData (Convert.ToSingle (obj));
				case "double":
				case "number":
					return DynValue.FromNumber (Convert.ToDouble (obj));
				case "bool":
				case "boolean":
					return DynValue.FromBoolean (Convert.ToBoolean (obj));
				case "string":
					return DynValue.FromString (Convert.ToString (obj));
				case "char":
					return DynValue.FromUserData (Convert.ToChar (obj));
				case "datetime":
				case "date":
				case "time":
					return DynValue.FromUserData (Convert.ToDateTime (obj));
				default:
					return DynValue.FromObject (Convert.ChangeType (obj, Type.GetType (typeName)));
			}
		}

		public static DynValue getType (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "getType");
			if (args [0].type != DataType.UserData)
				return DynValue.FromString (args [0].type.ToString ());
			else
				return DynValue.FromString (args [0].GetDynType ().Name);
		}

		public static DynValue toNumber (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "toNumber");
			var value = args [0];
			if (value.type == DataType.Number)
				return value;
			else
				return Convert.ToDouble (value.Value);
		}

		public static DynValue toString (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "toString");
			var value = args [0];
			if (value.type == DataType.String)
				return value;
			else
				return Convert.ToString (value.Value);
		}

		public static DynValue toList (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "toList");
			var collection = args [0].As<IEnumerable<DynValue>> ();
			return new DynList (collection);
		}

		public static DynValue quiet (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length <= 1, "toString");
			if (args.Length == 0) {
				GlobalSettings.Quiet = true;
			} else {
				GlobalSettings.Quiet = args [0].AsBoolean ();
			}
			return GlobalSettings.Quiet;
		}

		public static DynValue scan (DynValue[] args)
		{
			if (args.Length == 1)
				Console.Write (args [0].AsString ());
			return Console.ReadLine ();
		}

		public static DynValue bit_or (DynValue[] args)
		{
			int value = (int)args [0].AsNumber ();
			for (int i = 1; i < args.Length; i++) {
				value |= (int)args [i].AsNumber ();
			}
			return (double)value;
		}

		public static DynValue LoadLibrary (DynValue[]args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "CSharpDll");
			string filepath = args [0].AsString ();
			string class_name = args [1].AsString ();


			if (!Path.IsPathRooted (filepath)) {
				filepath = GlobalSettings.CurrentDirectory + "/" + filepath;
			}

			Assembly assembly = Assembly.LoadFile (filepath);
			Type t = assembly.GetType (class_name, true, true);  //namespace.class
			MethodInfo[] methods = t.GetMethods ();
			var list = new List<StructureItem> ();

			foreach (MethodInfo method in methods.Where(i => i.ReturnType == typeof(DynValue))) {
				ParameterInfo[] parameters = method.GetParameters ();
				if (parameters.Length == 1 && parameters [0].ParameterType == typeof(DynValue[])) {
					string method_name = method.Name;
					Func<DynValue[],DynValue> f = method.CreateDelegate (typeof(Func<DynValue[],DynValue>)) as Func<DynValue[],DynValue>;
					StructureItem item = new StructureItem (method_name, DynValue.FromDelegate (f, method_name));
					list.Add (item);
				}
			}
			var arr = new StructureItem[list.Count];
			list.CopyTo (arr);
			Structure structure = new Structure (arr);
			return DynValue.FromStructure (structure);
		}

		public static DynValue console_clear (DynValue[]args)
		{
			Console.Clear ();
			return DynValue.Nil;
		}

		public static DynValue console_write (DynValue[]args)
		{
			RuntimeException.FuncArgsCheck (args.Length >= 1, "console.write");
			if (args.Length == 1)
				Console.Write (args [0].Value);
			else {
				var arguments = new object[args.Length - 1];
				for (int i = 0; i < arguments.Length; i++)
					arguments [i] = args [i + 1].Value;
				Console.Write (args [0].AsString (), arguments);
			}
			return DynValue.Nil;
		}

		public static DynValue console_writeLine (DynValue[]args)
		{
			RuntimeException.FuncArgsCheck (args.Length >= 1, "console.writeLine");
			if (args.Length == 1)
				Console.WriteLine (args [0].Value);
			else {
				var arguments = new object[args.Length - 1];
				for (int i = 0; i < arguments.Length; i++)
					arguments [i] = args [i + 1].Value;
				Console.WriteLine (args [0].AsString (), arguments);
			}
			return DynValue.Nil;
		}

		public static DynValue console_read (DynValue[]args)
		{
			return (double)Console.Read ();
		}

		public static DynValue console_readLine (DynValue[]args)
		{
			return Console.ReadLine ();
		}

		public static DynValue console_readKey (DynValue[]args)
		{
			return (double)Console.ReadKey ().KeyChar;
		}

		public static DynValue os_clock (DynValue[] args)
		{
			return (double)DateTime.Now.Ticks;
		}

		public static DynValue dispose (DynValue[] args)
		{
			for (int i = 0; i < args.Length; i++) {
				args [i].As<IDisposable> ().Dispose ();
			}
			return DynValue.Nil;
		}

		public static DynValue exit (DynValue[] args)
		{
			Environment.Exit ((int)args [0].AsNumber ());
			return DynValue.Nil;
		}

		public static DynValue Throw (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length >= 1, "throw");
			if (args.Length == 1)
				throw new RuntimeException (args [0].AsString ());
			else
				throw new RuntimeException (args [0].AsString (), args.SkipMap (1, i => i.Value));
		}

		public static DynValue Range (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2 || args.Length == 3, "range");
			if (args.Length == 2)
				return DynValue.FromUserData (Extension.Range ((int)args [0].AsNumber (), 
							(int)args [1].AsNumber ()).Select (i => DynValue.FromNumber (i)));
			else
				return DynValue.FromUserData (Extension.Range ((int)args [0].AsNumber ()
							, (int)args [1].AsNumber ()
							, (int)args [2].AsNumber ()).Select (i => DynValue.FromNumber (i)));
		}

		public static DynValue len (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "len");
			return (double)args [0].As<ICollection> ().Count;
		}

		public static DynValue TryCatch (DynValue[] args)
		{
			/* Inspire by Lua */
			RuntimeException.FuncArgsCheck (args.Length == 2, "TryCatch");
			IFunction func = args [0].As<IFunction> ();
			DynList paras = args [1].As<DynList> ();
			try {
				DynValue[] parameters = paras.ToArray ();
				func.DynInvoke (parameters);
				return true;
			} catch (RuntimeException ex) {
				return ex.Message;
			} catch (Exception ex) {
				return ex.Message;
			}
		}

		static readonly string[] StrFieldNames = new string[] {
			"length", "replace", "format", "join", "split", "find", "lower", "upper", "trim", "trimStart", "trimEnd", "subString"
		};

		public static DynValue names (DynValue[] args)
		{
			/* Inspire by R */
			RuntimeException.FuncArgsCheck (args.Length == 1, "names");
			var obj = args [0];
			if (obj.type == DataType.String) {
				return new DynList (StrFieldNames.Select (DynValue.FromString));
			} else
				return new DynList (obj.As<IDot> ().FieldNames.Select (DynValue.FromString));
		}

		public static DynValue getwd (DynValue[] args)
		{
			return Directory.GetCurrentDirectory ();
		}

		public static DynValue setwd (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "setwd");
			string path = args [0].AsString ();
			Directory.SetCurrentDirectory (path);
			return DynValue.Nil;
		}

		private static readonly Dictionary<string, DynValue> ModulesCache = new Dictionary<string, DynValue>();
		public static DynValue import (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1 || args.Length == 2, "import");
			string moduleName = args [0].AsString ();
			Encoding encoding;
			if (args.Length == 2) {
				encoding = Encoding.GetEncoding (args [1].AsString ());
			} else {
				encoding = Encoding.Default;
			}
			string currentDir = GlobalSettings.CurrentDirectory;


			if (!Path.HasExtension (moduleName))
				moduleName = Path.ChangeExtension (moduleName, "cyg");

			string filePath = currentDir + "/lib/" + moduleName;
			DynValue Result;
			if (ModulesCache.TryGetValue(filePath, out Result)) {
				return Result;
			} else {

				bool quiet = GlobalSettings.Quiet;
				GlobalSettings.Quiet = true;

				if (!File.Exists (filePath))
					throw new RuntimeException ("No module named '{0}.", moduleName);

				BasicScope basicScope = new BasicScope ();
				CodeFileExecutor executor = new CodeFileExecutor (basicScope, filePath, encoding);
				GlobalSettings.Quiet = quiet;

				DynValue value = executor.Run ();
				if (value.type != DataType.Return) {
					throw new RuntimeException ("Module file should return value at the end of the file");
				} else {
					Result = (DynValue)value.Value;
					ModulesCache.Add (filePath, Result);
					return Result;
				}
			}
		}
	}
}
