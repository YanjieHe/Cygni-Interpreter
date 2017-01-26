using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST;
using Cygni.Loaders;
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

			var globalScope = scope as ModuleScope;
			if (globalScope == null) {
				throw new RuntimeException ("Unable to run command 'source' in local scope");
			}
			if (!Path.HasExtension (filepath))
				filepath = Path.ChangeExtension (filepath, "cyg");

			bool quiet = GlobalSettings.Quiet;
			GlobalSettings.Quiet = true;
			try {
				var executor = new CodeFileLoader (globalScope, filepath, encoding);
				executor.Run ();
				return executor.Result;
			} catch (RuntimeException ex) {
				throw ex;
			} catch (Exception ex) {
				throw ex;
			} finally {
				GlobalSettings.Quiet = quiet;
			}
		}

		public static DynValue assert (ASTNode[] args, IScope scope)
		{
			if (GlobalSettings.IsDebug) {
				RuntimeException.CmdArgsCheck (args.Length == 2, "assert");
				bool test = args [0].Eval (scope).AsBoolean ();
				if (!test) {
					string message = args [1].Eval (scope).AsString ();
					throw RuntimeException.Throw (message, scope);
				} else {
					return DynValue.Void;
				}
			} else {
				return DynValue.Void;
			}
		}

		internal static readonly Dictionary<string, BlockEx> ModulesCache 
		= new Dictionary<string, BlockEx> ();

		internal static string GetModulePath (string moduleName)
		{
			if (!Path.HasExtension (moduleName)) {
				moduleName = Path.ChangeExtension (moduleName, "cyg");
			}
			DirectoryInfo currentDir = GlobalSettings.CurrentDirectory;
			foreach (DirectoryInfo dir in currentDir.EnumerateDirectories()) {
				if (string.Equals (dir.Name, "lib")) {
					foreach (FileInfo file in dir.EnumerateFiles()) {
						if (string.Equals (file.Name, moduleName)) {
							return file.FullName;
						}
					}
					throw new RuntimeException ("No module named '{0}.", moduleName);
				}
			}
			throw new RuntimeException ("Missing 'lib' folder");	
		}

		public static DynValue import (ASTNode[] args, IScope scope)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1 || args.Length == 2, "import");
			if (scope.type != ScopeType.Module) {
				throw new RuntimeException ("command 'import' can only be executed in global scope.");
			} else {

				string moduleName = args [0].Eval (scope).AsString ();
				Encoding encoding;
				if (args.Length == 2) {
					encoding = Encoding.GetEncoding (args [1].Eval (scope).AsString ());
				} else {
					encoding = Encoding.Default;
				}

				string filePath = GetModulePath (moduleName);

				BlockEx program;
				if (ModulesCache.TryGetValue (filePath, out program)) {
					program.Eval (scope);
					return DynValue.Void;
				} else {
					
					bool quiet = GlobalSettings.Quiet;
					GlobalSettings.Quiet = true;

					ModuleScope globalScope = scope as ModuleScope;
					CodeFileLoader executor = new CodeFileLoader (globalScope, filePath, encoding);

					program = executor.Load ();
					ModulesCache.Add (filePath, program);
					program.Eval (scope);
					GlobalSettings.Quiet = quiet;
					return DynValue.Void;
				}
			}
		}

		public static DynValue error (ASTNode[] args, IScope scope)
		{
			RuntimeException.CmdArgsCheck (args.Length == 1, "error");
			string message = args [0].Eval (scope).AsString ();
			throw RuntimeException.Throw (message, scope);
		}

		/*public static DynValue Scope (DynValue[] args, IScope scope)
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
		}*/
	}
}
