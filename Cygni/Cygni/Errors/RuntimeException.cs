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
		public RuntimeException(string message)
			: base(" [Runtime error] " + message)
		{
		}
		public RuntimeException(string format, params object[] args)
			: base(" [Runtime error] " + format, args)
		{
			
		}
		public static RuntimeException NotDefined(string name)
		{
			return new RuntimeException("'{0}' is not defined.", name);
		}
		public static RuntimeException BadArgsNum(string funcName, int nArgs)
		{
			return new RuntimeException("bad number of arguments for '{0}', expecting '{1}' arguments.", funcName, nArgs);
		}
		public static RuntimeException BadArgsNum(string funcName, string ArgInfo)
		{
			return new RuntimeException("bad number of arguments for '{0}', expecting '{1}' arguments.", funcName, ArgInfo);
		}
	}
}
