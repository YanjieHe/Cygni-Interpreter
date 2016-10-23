using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Cygni.Errors
{
	/// <summary>
	/// Description of SyntaxException.
	/// </summary>
	public class SyntaxException:InterpreterException
	{
		public SyntaxException(string message)
			: base(" [Syntax error] " + message)
		{
		}
		public SyntaxException(string format, params object[] args)
			: base(" [Syntax error] " + format, args)
		{
		}
	}
}
