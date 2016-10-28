using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Cygni.Errors
{
	/// <summary>
	/// Description of InterpreterException.
	/// </summary>
	public class InterpreterException:Exception
	{
		protected InterpreterException(string message)
			: base(message)
		{

		}

		protected InterpreterException(string format, params object[] args)
			: base(string.Format(format, args))
		{

		}
	}
}
