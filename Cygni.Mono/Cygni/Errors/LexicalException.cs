using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Cygni.Errors
{
	/// <summary>
	/// Description of LexicalException.
	/// </summary>
	public class LexicalException:InterpreterException
	{
		public LexicalException(string message)
			: base(" [Lexical error] " + message)
		{
		}
		public LexicalException(string format, params object[] args)
			: base(" [Lexical error] " + format, args)
		{
		}
	}
}
