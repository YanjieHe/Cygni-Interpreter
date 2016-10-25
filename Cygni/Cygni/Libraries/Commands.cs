﻿using System.Collections.Generic;
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
		public static DynValue DoFile(DynValue[] args, IScope scope)
		{
			var basicScope = scope as BasicScope;
			if (basicScope == null)
				throw new RuntimeException("Unable to run command 'dofile' in local scope");
			
			string filepath = args[0].AsString();
			if (!Path.HasExtension(filepath))
				filepath = Path.ChangeExtension(filepath, "cyg");
			
			Encoding encoding = args.Length == 2
				? Encoding.GetEncoding(args[1].AsString())
				: Encoding.Default;
			bool quiet = GlobalSettings.Quiet;
			GlobalSettings.Quiet = true;

			var executor = new CodeFileExecutor(scope as BasicScope, filepath, encoding);
			GlobalSettings.Quiet = quiet;
			return executor.Run();
		}
		public static DynValue LoadDll(DynValue[]args, IScope scope)
		{
			string filepath = args[0].AsString();
			string class_name = args[1].AsString();
			if (!Path.IsPathRooted(filepath))
				filepath = Path.GetFullPath(filepath);
			
			Assembly assembly = Assembly.LoadFile(filepath);
			Type t = assembly.GetType(class_name, true, true);  //namespace.class
			var methods = t.GetMethods();
			var names = new List<string>();
			
			foreach (var method in methods.Where(i => i.ReturnType == typeof(DynValue))) {
				var parameters = method.GetParameters();
				if (parameters.Length == 1 && parameters[0].ParameterType == typeof(DynValue[])) {
					var method_name = method.Name;
					if (scope.HasName(method_name)) {
						Console.WriteLine("overwriting method '{0}'", method_name);
					}
					scope.Put(method_name, DynValue.FromDelegate(
						method.CreateDelegate(typeof(Func<DynValue[],DynValue>)) as Func<DynValue[],DynValue>));
					names.Add(method_name);
				}
			}
			return DynValue.FromList(new DynList(names.Select(DynValue.FromString), names.Count));
		}
		public static DynValue Delete(DynValue[] args,IScope scope){
			var basicScope = scope as BasicScope;
			if (basicScope == null)
				throw new RuntimeException("Unable to run command 'delete' in local scope");
			if(!args.All(i=>i.type == DataType.String))
				throw new RuntimeException("Type of parameters for command 'delete' must be string");
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
			return DynValue.Null;
		}
	}
}
