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
		Add,
		Sub,
		Mul,
		Div,
		Mod,
		Pow,
		And,
		Or,
		Not,
		Assign,
		Equal,
		NotEqual,
		Less,
		Greater,
		LessOrEqual,
		GreaterOrEqual,
		True,
		False,
		If,
		Else,
		ElseIf,
		Number,
		String,
		ID,
		While,
		For,
		Define,
		Comma,
		Colon,
		Semicolon,
		LeftParenthesis,
		RightParenthesis,
		LeftBracket,
		RightBracket,
		LeftBrace,
		RightBrace,
		Break,
		Continue,
		Return,
		Dot,
		Class,
		Null,
		EOF,
		EOL,
	}
}
