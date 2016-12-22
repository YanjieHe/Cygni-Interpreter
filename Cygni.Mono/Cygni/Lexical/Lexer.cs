using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.IO;
using Cygni.Lexical.Tokens;
using Cygni.Errors;
using Re = System.Text.RegularExpressions.Regex;

namespace Cygni.Lexical
{
	/// <summary>
	/// Description of Lexer.
	/// </summary>
	public class Lexer
	{
		private readonly TextReader reader;
		private int lineNumber;
		private readonly StringBuilder s;

		public int LineNumber{ get { return lineNumber; } }

		public Lexer (int lineNumber, TextReader reader)
		{
			this.lineNumber = lineNumber;
			if (reader == null)
				throw new ArgumentNullException ("TextReader cannot be null.");
			this.reader = reader;
			s = new StringBuilder ();
		}

		int Peek ()
		{
			return reader.Peek ();
		}

		int GetChar ()
		{
			return reader.Read ();
		}

		bool IsEOF {
			get { return Peek () == (-1); }
		}

		LexicalException Error (string message)
		{
			return new LexicalException ("line {0}: {1}.", lineNumber, message);
		}

		public LinkedList<Token> ReadWords ()
		{
			var tokenlist = new LinkedList<Token> ();
			for (;;) {
				Token word = Scan ();
				if (word.tag == Tag.EOF)
					break;
				tokenlist.AddLast (word);
			}
			return tokenlist;
		}

		public Token Scan ()
		{
			if (s.Length > 0) {/* Reset the StringBuilder */
				s.Clear ();
			}

			while (IsSpace (Peek ())) {/* Skip the white spaces */
				if (GetChar () == '\n')
					lineNumber++;
			}

			if (Peek () == '#') { /* start of comments */
				while (Peek () != -1 && Peek () != '\n')
					GetChar ();
			}


			int ch = Peek ();

			if (ch < 0)
				return Token.EOF;//End of text

			if (ch == '@') {
				return ParseUnescapedString ();
			} else if (ch == '"' || ch == '\'') { /* start of string */
				return ReadString ((char)GetChar ());
			} else if (IsDigit (ch)) {/* start of integer or number */
				return ParseInteger ();
			} else if (IsLetterOrUnderline (ch)) {/* Start of identifier */
				return ParseIdentifier ();
			} else {

				GetChar ();
				switch (ch) {
					case '!':
						if (Peek () == '=') {
							GetChar ();
							return Word.NotEqual;
						} else
							throw Error("Unexpected '!', missing '='.");
					case '=':
						if (Peek () == '=') {
							GetChar ();
							return Word.Equal;
						} else if (Peek () == '>') {
							GetChar ();
							return Word.GoesTo;
						} else {
							return Word.Assign;
						}
					case '.':
						if (Peek () == '.') {
							GetChar();
							return Word.Concatenate;
						} else {
							return Word.Dot;
						}
					case '>':
						return ParseGreater ();
					case '<':
						return ParseLess ();
						/* arithmetic operators */
					case '+':
						return Word.Add;
					case '-':
						return Word.Sub;
					case '*':
						return Word.Mul;
					case '/':
						if (Peek() == '/') {
							GetChar();
							return Word.IntDiv;
						} else {
							return Word.Div;
						}
					case '%':
						return Word.Mod;
					case '^':
						return Word.Pow;

					case '(':
						return Word.LP;
					case ')':
						return Word.RP;
					case '[':
						return Word.LBracket;
					case ']':
						return Word.RBracket;
					case '{':
						return Word.LBrace;
					case '}':
						return Word.RBrace;

					case ',':
						return Word.Comma;
					case ';':
						return Word.Semicolon;
					case ':':
						return Word.Colon;
					case '\n':
						lineNumber++;
						return Word.EOL;
					default:
						throw Error(string.Format("Unrecognizable token '{1}'.", (char)ch));
				}
			}
		}

		Token ReadString (char start_ch)
		{
			while (Peek () != -1) {
				if (Peek () == start_ch) {
					GetChar ();
					if (s.Length == 0)
						return Token.OfString (string.Empty);/* "" or '' */
					if (s [s.Length - 1] != '\\')
						return Token.OfString (Re.Unescape (s.ToString ()));/* \" or \' */

					s.Append (start_ch);
				} else
					s.Append ((char)GetChar ());
			}
			throw Error("Missing closure for string declaration");
		}

		Token ParseUnescapedString ()
		{
			GetChar();
			if (Peek () == '"' || Peek () == '\'') {
				char start_ch = (char)Peek ();
				GetChar ();
				while (!IsEOF) {
					if (Peek () == start_ch) {
						GetChar ();
						if (Peek () == start_ch) {
							GetChar ();
							s.Append (start_ch);/* "" and '' */
						} else {
							return Token.OfString (s.ToString ());
						}
					} else {
						s.Append ((char)GetChar ());
					}
				}
				throw Error("Missing closure for string declaration");
			} else {
				throw Error("Missing '\"' or ''' when parsing unescaped string.");
			}
		}

		Token ParseInteger ()
		{
			do {
				s.Append ((char)GetChar ());/* Read integer part */
			} while (IsDigit (Peek ()));
			if (Peek () == '.' || Peek () == 'e' || Peek () == 'E') {
				ParseNumber ();
				return TryParseNumber ();
			} else {
				return TryParseInteger ();
			}
		}

		Token ParseNumber ()
		{
			if (Peek () == '.') {/* Read decimal part*/
				GetChar ();
				s.Append ('.');
				if (!IsDigit (Peek ())) {
					throw Error ("Unrecognizable format of number.");
				} else {
					do {
						s.Append ((char)GetChar ());
					} while (IsDigit (Peek ()));
				}
				if (Peek () == 'e' || Peek () == 'E') {
					ParseScientificNotation ();
					return TryParseNumber ();
				} else {
					return TryParseNumber ();
				}
			} else { /* 'e' or 'E' */
				ParseScientificNotation ();
				return TryParseNumber ();
			}
		}

		void ParseScientificNotation ()
		{
			if (Peek () == 'E' || Peek () == 'e') {/* Scientific notation */
				s.Append ((char)GetChar ());
				if (Peek () == '+' || Peek () == '-') {
					s.Append ((char)GetChar ());
					if (!IsDigit (Peek ())) {
						throw Error ("Unrecognizable format of number.");
					} else {
						ParseExponent ();
					}
				} else {
					ParseExponent ();
				}
			}
		}

		void ParseExponent ()
		{
			do {
				s.Append ((char)GetChar ());
			} while (IsDigit (Peek ()));
		}

		Token TryParseInteger ()
		{
			long integer;
			if (long.TryParse (s.ToString (), out integer)) {
				return new IntToken (integer);
			} else {
				throw Error ("Unrecognizable format of integer.");
			}
		}

		Token TryParseNumber ()
		{
			double number;
			if (double.TryParse (s.ToString (), out number)) {
				return new NumToken (number);
			} else {
				throw Error ("Unrecognizable format of number.");
			}
		}

		Token ParseIdentifier ()
		{
			char ch;
			do {
				s.Append ((char)GetChar ());/* Read Identifier */
				ch = (char)Peek ();
			} while (IsLetterOrUnderline (ch) || IsDigit (ch));
			return Token.OfIdentifier ((s.ToString ()));
		}

		Token ParseGreater ()
		{
			if (Peek () == '=') {
				GetChar ();
				return Word.GreaterOrEqual;
			} else {
				return Word.Greater;
			}
		}

		Token ParseLess ()
		{
			if (Peek () == '=') {
				GetChar ();
				return Word.LessOrEqual;
			} else {
				return Word.Less;
			}
		}

		static bool IsLetterOrUnderline (int ch)
		{
			return ('A' <= ch && ch <= 'Z') || ('a' <= ch && ch <= 'z') || ch == '_';
		}

		static bool IsDigit (int ch)
		{
			return '0' <= ch && ch <= '9';
		}

		static bool IsSpace (int ch)
		{
			return ch == '\t' || ch == '\v' || ch == '\f' || ch == ' ' || ch == '\r' || ch == '\n';
		}

	}
}
