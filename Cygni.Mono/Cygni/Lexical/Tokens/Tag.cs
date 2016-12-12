using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Cygni.Lexical.Tokens
{
	/// <summary>
	/// Description of Tag.
	/// </summary>
	public enum Tag
	{
		/* Arithmetic Operators */
		Add,
		Sub,
		Mul,
		Div,
		Mod,
		Pow,

		/*Logical Operators */
		And,
		Or,
		Not,

		Assign,

		/* Relation Operators */
		Equal,
		NotEqual,
		Less,
		Greater,
		LessOrEqual,
		GreaterOrEqual,


		True,
		False,

		/* Statements */
		If,
		Else,
		ElseIf,
		While,
		For,
		ForEach,
		Define,
		Class,
		Global,


		Number,
		String,
		ID,

		Comma,
		Colon,
		Semicolon,
		LeftParenthesis,
		RightParenthesis,
		LeftBracket,
		RightBracket,
		LeftBrace,
		RightBrace,

		/* Goto Expression */
		Break,
		Continue,
		Return,

		Dot,
		In,
		Nil,
		EOF,
		EOL,
	}
}
