using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Cygni.AST
{
	/// <summary>
	/// Description of NodeType.
	/// </summary>
	public enum NodeType
	{
		Binary,
		Block,
		Constant,
		DefClass,
		DefFunc,
		DefClosure,
		Dot,
		If,
		Invoke,
		Name,
		Unary,
		While,
		For,
		Return,
		Command,
		ListInit,
		DictionaryInit,
		Index,
		SingleIndex,
		Assign,
		Local,
		Global,
		Unpack,
		Range,
	}
}
