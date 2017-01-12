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
		IntDiv,
		Mod,
		Pow,

		/* String Operators */
		Concatenate,

		/*Logical Operators */
		And,
		Or,
		Not,

		Assign,
		GoesTo,

		/* Relation Operators */
		Equal,
		NotEqual,
		Less,
		Greater,
		LessOrEqual,
		GreaterOrEqual,

		/* Built-in Constant Values */
		True,
		False,
		Nil,

		/* Constant Values */
		Integer,
		Number,
		String,
		ID,

		/* Statements */
		If,
		Else,
		ElseIf,
		While,
		For,
		ForEach,
		In,
		Define,
		Lambda,
		Class,
		Var,

		/* Separators */
		Comma,
		Colon,
		Semicolon,
		Dot,

		/* Brackets */
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

		EOF,
		EOL,
	}
}
