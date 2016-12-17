using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST;
using Cygni.Settings;
using Cygni.AST.Scopes;
using Cygni.Libraries;
using Cygni.Errors;
namespace Cygni.Executors
{
	/// <summary>
	/// Description of Engine.
	/// </summary>
	public class Engine
	{
		ResizableArrayScope globalScope;

		public Engine ()
		{
			globalScope = new ResizableArrayScope ();
		}

		public void Initialize ()
		{
			GlobalSettings.BuiltIn (globalScope);
		}

		public static Engine CreateInstance ()
		{
			var engine = new Engine ();
			engine.Initialize ();
			return engine;
		}

		public DynValue Evaluate (string code)
		{
			var executor = new CodeStringExecutor (globalScope, code);
			return executor.Run ();
		}

		public T Evaluate<T> (string code)
		{
			var executor = new CodeStringExecutor (globalScope, code);
			var result = executor.Run ();
			return (T)Convert.ChangeType (result.Value, typeof(T));
		}

		public DynValue DoFile (string filepath, Encoding encoding = null)
		{
			var executor = new CodeFileExecutor (globalScope, filepath, encoding ?? Encoding.Default);
			return executor.Run ();
		}

		public T DoFile<T> (string filepath, Encoding encoding = null)
		{
			var executor = new CodeFileExecutor (globalScope, filepath, encoding ?? Encoding.Default);
			var result = executor.Run ();
			return (T)Convert.ChangeType (result.Value, typeof(T));
		}

		public void Import (string package)
		{
			Commands.Import (new DynValue[]{ package }, globalScope);
		}

		public void Import (string[] packages)
		{
			Commands.Import (packages.Select (i => DynValue.FromString (i)).ToArray (), globalScope);
		}

		public DynValue ExecuteFromEntryPoint(params DynValue[] args){
			const string MainFuncName = "__MAIN__";
			DynValue funcValue;
			if (globalScope.TryGetValue (MainFuncName, out funcValue)) {
				return funcValue.As<IFunction> ().DynInvoke (args);
			}
			throw new RuntimeException ("Missing Entry Point, expecting function '__MAIN__'");
		}

		public DynValue ExecuteInConsole ()
		{
			var executor = new InteractiveExecutor (globalScope);
			return executor.Run ();
		}

		public Engine SetSymbol (string name, DynValue value)
		{
			globalScope.Put (name, value);
			return this;
		}

		public DynValue GetSymbol (string name)
		{
			return globalScope.Get (name);
		}

		public bool TryGetValue (string name, out DynValue value)
		{
			return globalScope.TryGetValue (name, out value);
		}

	}
}
