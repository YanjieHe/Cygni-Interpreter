using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST;
using Cygni.Executors;
using Cygni.Settings;
using System.Reflection;
using System.IO;
using Cygni.Errors;
using Cygni.AST.Scopes;

namespace Cygni.Libraries
{
	/// <summary>
	/// Description of Commands.
	/// </summary>
	public static class Commands
	{
		public static DynValue source (ASTNode[] args, IScope scope)
		{
			RuntimeException.CmdArgsCheck (args.Length == 1 || args.Length == 2, "source");
			string filepath = args [0].Eval (scope).AsString ();
			Encoding encoding;
			if (args.Length == 2) {
				encoding = Encoding.GetEncoding (args [1].Eval (scope).AsString ());
			} else {
				encoding = Encoding.Default;
			}

			var basicScope = scope as BasicScope;
			if (basicScope == null)
				throw new RuntimeException ("Unable to run command 'source' in local scope");

			if (!Path.HasExtension (filepath))
				filepath = Path.ChangeExtension (filepath, "cyg");

			bool quiet = GlobalSettings.Quiet;
			GlobalSettings.Quiet = true;

			var executor = new CodeFileExecutor (basicScope, filepath, encoding);
			GlobalSettings.Quiet = quiet;
			return executor.Run ();
		}

		public static DynValue cond (ASTNode[] args, IScope scope)
		{
			RuntimeException.CmdArgsCheck (args.Length == 3, "cond");
			return args [0].Eval (scope).AsBoolean () ? args [1].Eval (scope) : args [2].Eval (scope);
		}

		public static DynValue assert (ASTNode[] args, IScope scope)
		{
			if (GlobalSettings.IsDebug) {
				RuntimeException.CmdArgsCheck (args.Length == 2, "assert");
				bool test = args [0].Eval (scope).AsBoolean ();
				if (!test) {
					string message = args [1].Eval (scope).AsString ();
					throw new RuntimeException (message);
				} else {
					return DynValue.Nil;
				}
			} else {
				return DynValue.Nil;
			}
		}

		/* public static DynValue LoadLibrary (ASTNode[] args, IScope scope)
		{
			RuntimeException.CmdArgsCheck (args.Length == 2, "loadLibrary");
			string filepath = args [0].Eval (scope).AsString ();
			string class_name = args [1].Eval (scope).AsString ();

			if (!Path.IsPathRooted (filepath)) {
				filepath = GlobalSettings.CurrentDirectory + "/" + filepath;
			}

			Assembly assembly = Assembly.LoadFile (filepath);
			Type t = assembly.GetType (name: class_name, throwOnError: true, ignoreCase: true);  //namespace.class
			var methods = t.GetMethods ();
			var names = new List<string> ();

			foreach (var method in methods.Where(i => i.ReturnType == typeof(DynValue))) {
				var parameters = method.GetParameters ();
				if (parameters.Length == 1 && parameters [0].ParameterType == typeof(DynValue[])) {
					var method_name = method.Name;

					if (scope.HasName (method_name)) {
						Console.WriteLine ("overwriting method '{0}'", method_name);
					}
					scope.Put (method_name, DynValue.FromDelegate (
						method.CreateDelegate (typeof(Func<DynValue[],DynValue>))
								as Func<DynValue[],DynValue>));
					names.Add (method_name);
				}
			}
			return DynValue.FromList (new DynList (names.Select (DynValue.FromString), names.Count));
		}*/

		public static DynValue DoFile (DynValue[] args, IScope scope)
		{
			RuntimeException.CmdArgsCheck (args.Length == 1 || args.Length == 2, "dofile");
			var basicScope = scope as BasicScope;
			if (basicScope == null)
				throw new RuntimeException ("Unable to run command 'dofile' in local scope");

			string filepath = args [0].AsString ();
			if (!Path.HasExtension (filepath))
				filepath = Path.ChangeExtension (filepath, "cyg");

			Encoding encoding = args.Length == 2
				? Encoding.GetEncoding (args [1].AsString ())
				: Encoding.Default;
			bool quiet = GlobalSettings.Quiet;
			GlobalSettings.Quiet = true;

			var executor = new CodeFileExecutor (scope as BasicScope, filepath, encoding);
			GlobalSettings.Quiet = quiet;
			return executor.Run ();
		}

		/* public static DynValue LoadDll (DynValue[]args, IScope scope)
		{
			RuntimeException.CmdArgsCheck (args.Length == 2, "loaddll");
			string filepath = args [0].AsString ();
			string class_name = args [1].AsString ();
			if (!Path.IsPathRooted (filepath)) {
				filepath = GlobalSettings.CurrentDirectory + "/" + filepath;
			}
			//	filepath = Path.GetFullPath (filepath);

			Assembly assembly = Assembly.LoadFile (filepath);
			Type t = assembly.GetType (class_name, true, true);  //namespace.class
			var methods = t.GetMethods ();
			var names = new List<string> ();

			foreach (var method in methods.Where(i => i.ReturnType == typeof(DynValue))) {
				var parameters = method.GetParameters ();
				if (parameters.Length == 1 && parameters [0].ParameterType == typeof(DynValue[])) {
					var method_name = method.Name;
					if (scope.HasName (method_name)) {
						Console.WriteLine ("overwriting method '{0}'", method_name);
					}
					scope.Put (method_name, DynValue.FromDelegate (
						method.CreateDelegate (typeof(Func<DynValue[],DynValue>)) as Func<DynValue[],DynValue>));
					names.Add (method_name);
				}
			}
			return DynValue.FromList (new DynList (names.Select (DynValue.FromString), names.Count));
		}*/

		public static DynValue Delete (DynValue[] args, IScope scope)
		{
			var basicScope = scope as BasicScope;
			if (basicScope == null)
				throw new RuntimeException ("Unable to run command 'delete' in local scope");
			if (!args.All (i => i.type == DataType.String))
				throw new RuntimeException ("Type of parameters for command 'delete' must be string");
			foreach (var item in args) {
				var name = item.AsString ();
				bool success = basicScope.Delete (name);
				if (!GlobalSettings.Quiet) {
					if (success)
						Console.WriteLine ("variable '{0}' has been deleted successfully.", name);
					else
						Console.WriteLine ("Unable to delete variable '{0}'.", name);
				}
			}
			return DynValue.Nil;
		}

		public static DynValue Import (DynValue[] args, IScope scope)
		{
			RuntimeException.CmdArgsCheck (args.Length == 1 || args.Length == 2, "import");
			var basicScope = scope as BasicScope;
			if (basicScope == null)
				throw new RuntimeException ("Unable to run command 'import' in local scope");

			string moduleName = args [0].AsString ();
			if (!Path.HasExtension (moduleName))
				moduleName = Path.ChangeExtension (moduleName, "cyg");
			string currentDir = GlobalSettings.CurrentDirectory;
			Encoding encoding = args.Length == 2
				? Encoding.GetEncoding (args [1].AsString ())
				: Encoding.Default;

			bool quiet = GlobalSettings.Quiet;
			GlobalSettings.Quiet = true;
			string filePath = currentDir + "/lib/" + moduleName;
			if (!File.Exists (filePath))
				throw new RuntimeException ("No module named '{0}.", moduleName);
			var executor = new CodeFileExecutor (scope as BasicScope, filePath, encoding);
			GlobalSettings.Quiet = quiet;

			return executor.Run ();
		}

		public static DynValue Scope (DynValue[] args, IScope scope)
		{
			RuntimeException.CmdArgsCheck (args.Length == 1, "scope");
			string cmdType = args [0].AsString ();
			switch (cmdType) {
			case "clear":
				{
					var basicScope = scope as BasicScope;
					if (basicScope == null)
						throw new RuntimeException ("Unable to run command 'delete' in local scope");
					int count = basicScope.Count;
					basicScope.Clear ();
					if (!GlobalSettings.Quiet) {
						if (count == 0)
							Console.WriteLine ("There is no variable in the current scope.");
						else if (count == 1)
							Console.WriteLine ("1 variable has been deleted successfully.");
						else
							Console.WriteLine ("{0} variables have been deleted successfully.", count);
					}
					return DynValue.Nil;
				}

			case "display":
				{
					Console.WriteLine (scope);
					return DynValue.Nil;
				}
			default :
				throw new RuntimeException ("Not supported parameter '{0}' for command 'scope'", cmdType);
			}
		}

		public static DynValue SetGlobal (DynValue[] args, IScope scope)
		{
			RuntimeException.CmdArgsCheck (args.Length == 2, "setglobal");
			string variableName = args [0].AsString ();
			DynValue value = args [1];
			while (!(scope is BasicScope)) {
				scope = scope.Parent;
			}
			scope.Put (variableName, value);
			return DynValue.Nil;
		}
	}
}
