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

			var globalScope = scope as ResizableArrayScope;
			if (globalScope == null)
				throw new RuntimeException ("Unable to run command 'source' in local scope");

			if (!Path.HasExtension (filepath))
				filepath = Path.ChangeExtension (filepath, "cyg");

			bool quiet = GlobalSettings.Quiet;
			GlobalSettings.Quiet = true;

			var executor = new CodeFileExecutor (globalScope, filepath, encoding);
			GlobalSettings.Quiet = quiet;

			executor.Run ();
			return executor.Result;
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
	}
}
