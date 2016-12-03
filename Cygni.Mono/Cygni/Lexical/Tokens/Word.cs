using System.Linq;
using System;

namespace Cygni.Lexical.Tokens
{
	/// <summary>
	/// Description of Word.
	/// </summary>
	public class Word:Token
	{
		readonly string text;

		public string Text{ get { return text; } }

		public Word (string text, Tag tag)
			: base (tag)
		{
			this.text = text;
		}

		public static Word FromString (string text)
		{
			switch (text) {
			/* logical */
			case "and":
				return And;
			case "or":
				return Or;
			case "not":
				return Not;

			case "==":
				return Eq;
			case "!=":
				return Neq;
			case "=":
				return Assign;

				/* relation */
			case "<":
				return Less;
			case ">":
				return Greater;
			case "<=":
				return LessOrEqual;
			case ">=":
				return GreaterOrEqual;
			case "true":
				return True;
			case "false":
				return False;

				/* arithmetic */
			case "+":
				return Add;
			case "-":
				return Sub;
			case "*":
				return Mul;
			case "/":
				return Div;
			case "%":
				return Mod;
			case "^":
				return Pow;

			case "if":
				return If;
			case "else":
				return Else;
			case "elif":
				return ElseIf;
			case "while":
				return While;
			case "for":
				return For;
			case "foreach":
				return ForEach;
			case "def":
				return Define;
			case "class":
				return Class;

			case ",":
				return Comma;
			case ":":
				return Colon;
			case ";":
				return Semicolon;
			case "(":
				return LP;
			case ")":
				return RP;
			case"[":
				return LBracket;
			case "]":
				return RBracket;
			case "{":
				return LBrace;
			case "}":
				return RBrace;
			case "break":
				return Break;
			case "continue":
				return Continue;
			case "return":
				return Return;
			case ".":
				return Dot;
			case "in":
				return In;
			case "null":
				return Null;
			case "\\n":
				return EOL;
			default:
				return new Word (text, Tag.ID);
			}
		}

		public static readonly Word
			And = new Word ("and", Tag.And), Or = new Word ("or", Tag.Or),
			Not = new Word("not",Tag.Not),
			Eq = new Word ("==", Tag.Equal), Neq = new Word ("!=", Tag.NotEqual),
			Assign = new Word ("=", Tag.Assign),
			Less = new Word ("<", Tag.Less), Greater = new Word (">", Tag.Greater),
			LessOrEqual = new Word ("<=", Tag.LessOrEqual), GreaterOrEqual = new Word (">=", Tag.GreaterOrEqual),
			True = new Word ("true", Tag.True), False = new Word ("false", Tag.False),
			
			Add = new Word ("+", Tag.Add), Sub = new Word ("-", Tag.Sub),
			Mul = new Word ("*", Tag.Mul), Div = new Word ("/", Tag.Div),
			Mod = new Word ("%", Tag.Mod), Pow = new Word ("^", Tag.Pow),
			
			If = new Word ("if", Tag.If), Else = new Word ("else", Tag.Else),
			ElseIf = new Word ("elif", Tag.ElseIf),
			While = new Word ("while", Tag.While),
			For = new Word ("for", Tag.For),
			ForEach = new Word ("foreach", Tag.ForEach),
			Define = new Word ("def", Tag.Define),
			Class = new Word ("class", Tag.Class),
				


			Comma = new Word (",", Tag.Comma),
			Colon = new Word (":", Tag.Colon),
			Semicolon = new Word (";", Tag.Semicolon),
			LP = new Word ("(", Tag.LeftParenthesis),
			RP = new Word (")", Tag.RightParenthesis),
			LBracket = new Word ("[", Tag.LeftBracket),
			RBracket = new Word ("]", Tag.RightBracket),
			LBrace = new Word ("{", Tag.LeftBrace),
			RBrace = new Word ("}", Tag.RightBrace),
			Break = new Word ("break", Tag.Break),
			Continue = new Word ("continue", Tag.Continue),
			Return = new Word ("return", Tag.Return),
			Dot = new Word (".", Tag.Dot),
			In = new Word ("in", Tag.In),
			Null = new Word ("null", Tag.Null),
			EOL = new Word ("\\n", Tag.EOL);

		public override string ToString ()
		{
			return text;
		}
	}
}
