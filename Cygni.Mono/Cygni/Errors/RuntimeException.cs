using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.AST.Scopes;
using Cygni.DataTypes;

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

		public static RuntimeException FieldNotExist (string target, string field)
		{
			return new RuntimeException ("'{0}' does not have field '{1}'.", target, field);
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

		public static void IndexerArgsCheck (bool condition, string collectionName)
		{
			if (!condition)
				throw new RuntimeException ("bad number of arguments for indexer of '{0}'.", collectionName);
		}

		public static void CmdArgsCheck (bool condition, string cmdName)
		{
			if (!condition)
				throw new RuntimeException ("bad number of arguments for command '{0}'.", cmdName);
		}

		public static void SliceCheck (int length, Range range)
		{
			if (range.Step > 0 && length < range.End) {
				throw new RuntimeException ("the end of slice should greater than or equals to the origin length when the step is positive.");
			} else if (range.Step < 0 && length < range.Start) {
				throw new RuntimeException ("the start of slice should greater than or equals to the origin length when the step is negative.");
			} else {
				return;
			}
		}

		public static RuntimeException Throw (string message, IScope scope)
		{
			Queue<string> queue = new Queue<string> ();
			IScope current = scope;
			while (current != null) {
				queue.Enqueue (string.Format (": in {0} {1} scope.", current.ScopeName, current.type));
				current = current.Parent;
			}
			StringBuilder s = new StringBuilder ();
			s.AppendLine ();
			s.AppendLine ("stdin:" + queue.Count + ": " + message);
			while (queue.Count > 0) {
				s.Append ("\tstdin:" + queue.Count);
				s.AppendLine (queue.Dequeue ());
			}
			return new RuntimeException (s.ToString ());
		}
		public static RuntimeException AssignVoidValue (IScope scope)
		{
		return	Throw("Cannot assign 'void'",scope);
		}
	}
}
