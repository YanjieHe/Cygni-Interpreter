using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Libraries;
using Cygni.Extensions;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;

namespace Cygni.AST
{
	/// <summary>
	/// Description of CommandEx.
	/// </summary>
	public class CommandEx:ASTNode
	{
		public override NodeType type { get { return NodeType.Command; } }

		public CommandType commandType;
		IList<ASTNode> parameters;

		public IList<ASTNode>Parameters { get { return parameters; } }

		public CommandEx (CommandType commandType, IList<ASTNode>parameters)
		{
			this.commandType = commandType;
			this.parameters = parameters;
		}

		internal static readonly Dictionary<string,CommandType>
			cmdDict = new Dictionary<string, CommandType> () {
			{ "dofile", CommandType.DoFile },
			{ "loaddll", CommandType.LoadDll },
			{ "delete", CommandType.Delete },
			{ "import", CommandType.Import },
			{ "scope", CommandType.Scope },

		};
		public CommandEx (string commandName, IList<ASTNode>parameters)
		{
			if (!cmdDict.TryGetValue (commandName, out commandType))
				throw new NotSupportedException (commandName);
			this.parameters = parameters;
		}

		public override DynValue Eval (IScope scope)
		{
			switch (commandType) {
			case CommandType.DoFile:
				return RunCommand (Commands.DoFile, scope);
			case CommandType.LoadDll:
				return RunCommand (Commands.LoadDll, scope);
			case CommandType.Delete:
				return RunCommand (Commands.Delete, scope);
			case CommandType.Import:
				return RunCommand (Commands.Import, scope);
			case CommandType.Scope:
				return RunCommand (Commands.Scope, scope);
			default:
				throw new NotSupportedException (commandType.ToString ());
			}
		}

		DynValue RunCommand (Func<DynValue[],IScope, DynValue> command, IScope scope)
		{
			return command (parameters.Map (i => i.Eval (scope)), scope);
		}

		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
	}

	public enum CommandType:byte
	{
		DoFile,
		LoadDll,
		Delete,
		Import,
		Scope,
	}
}
