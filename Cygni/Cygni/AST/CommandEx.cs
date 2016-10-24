using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Libraries;
using Cygni.Extensions;
using Cygni.AST.Scopes;

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
		public CommandEx(CommandType commandType, IList<ASTNode>parameters)
		{
			this.commandType = commandType;
			this.parameters = parameters;
		}
		public CommandEx(string commandName, IList<ASTNode>parameters)
		{
			switch (commandName.ToLower()) {
				case "dofile":
					this.commandType = CommandType.DoFile;
					break;
				case "loaddll":
					this.commandType = CommandType.LoadDll;
					break;
				default:
					throw new NotSupportedException(commandName);
			}
			this.parameters = parameters;
		}
		public override DynValue Eval(IScope scope)
		{
			switch (commandType) {
				case CommandType.DoFile:
					return Commands.DoFile(parameters.Map(i => i.Eval(scope)), scope);
				case CommandType.LoadDll:
					return Commands.LoadDll(parameters.Map(i => i.Eval(scope)), scope);
				default:
					throw new NotSupportedException(commandType.ToString());
			}
		}
	}
	public enum CommandType
	{
		DoFile,
		LoadDll,
	}
}
