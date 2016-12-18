using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.Libraries;
using System.Reflection;
using Cygni.AST;
using Cygni.AST.Scopes;
using Cygni.DataTypes.Interfaces;

namespace Cygni.DataTypes
{
	public sealed class Command:IFunction
	{
		readonly string name;
		readonly Func<ASTNode[], IScope, DynValue> command;

		public Command (string name, Func<ASTNode[], IScope, DynValue> command)
		{
			this.name = name;
			this.command = command;
		}

		public DynValue DynInvoke (DynValue[] args)
		{
			throw new NotSupportedException ();	
		}

		public Func<DynValue[],DynValue> AsDelegate ()
		{
			throw new NotSupportedException ();	
		}

		public DynValue DynEval (ASTNode[] args, IScope scope)
		{
			return command (args, scope);
		}

		public override string ToString ()
		{
			return string.Concat ("(Command: ", name, ")");
		}
	}
}

