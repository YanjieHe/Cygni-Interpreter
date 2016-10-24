using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Cygni.Errors
{
	/// <summary>
	/// Description of RuntimeException.
	/// </summary>
	public class RuntimeException:InterpreterException
	{
		public RuntimeException (string message)
			: base (" [Runtime error] " + message)
		{
		}

		public RuntimeException (string format, params object[] args)
			: base (" [Runtime error] " + format, args)
		{
			
		}

		public static RuntimeException NotDefined (string name)
		{
			return new RuntimeException ("'{0}' is not defined.", name);
		}

		public static RuntimeException FieldNotExists (string target, string field)
		{
			return new RuntimeException ("The field '{0}' of '{1}' does not exist.", field, target);
		}

		public static RuntimeException BadArgsNum (string funcName, int nArgs)
		{
			return new RuntimeException ("bad number of arguments for '{0}', expecting '{1}' arguments.", funcName, nArgs);
		}

		public static RuntimeException BadArgsNum (string funcName, string ArgInfo)
		{
			return new RuntimeException ("bad number of arguments for '{0}', expecting '{1}' arguments.", funcName, ArgInfo);
		}

		public static void FuncArgsCheck (bool condition, string funcName)
		{
			if (!condition)
				throw new RuntimeException ("bad number of arguments for function '{0}'.", funcName);
		}
	}
}
