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
		readonly TextReader reader;
		int lineNumber;
		readonly StringBuilder s;

		public int LineNumber{ get { return lineNumber; } }

		public Lexer (int lineNumber, TextReader reader)
		{
			this.lineNumber = lineNumber;
			if (reader == null)
				throw new ArgumentNullException ();
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
			if (s.Length > 0)/* Reset the StringBuilder */
				s.Clear ();
			
			while (IsSpace (Peek ())){/* Skip the white spaces */
				if(GetChar () == '\n')
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
				GetChar ();
				if (Peek () == '"' || Peek () == '\'') {
					return ReadUnescapedStr ((char)GetChar ());
				}
				throw new LexicalException ("line {0}: Unexpected '@'", lineNumber);
			}
			
			if (ch == ':') {
				GetChar ();
				return Token.OfIdentifier (":");
			}
			
			if (ch == '"' || ch == '\'') /* start of string */
				return ReadStr ((char)GetChar ());
				
			if (IsDigit (ch)) {/* start of number */
				ReadNumber ();
				
				if (Peek () == 'E' || Peek () == 'e') {/* Scientific notation */
					
					s.Append ((char)GetChar ());
					if (Peek () == '+' || Peek () == '-')
						s.Append ((char)GetChar ());
					ReadInteger ();/* Read exponent */
				}
				
				double number;
				if (double.TryParse (s.ToString (), out number))
					return Token.OfNumber (s.ToString ());
				throw new LexicalException ("line {0}: Illegal number input '{1}'", lineNumber, s);
			}
				
			if (IsLetterOrUnderline (ch)) {/* Start of identifier */
				do {
					s.Append ((char)GetChar ());/* Read Id */
					ch = Peek ();
				} while (IsLetterOrUnderline (ch) || IsDigit (ch));
				return Token.OfIdentifier ((s.ToString ()));
			}
				
			if (ch == '!') {
				GetChar ();
				if (Peek () == '=') {
					GetChar ();
					return Token.OfIdentifier ("!=");
				} else
					throw new LexicalException ("line {0}: Unexpected '!', missing '='", lineNumber);
			}
			switch (ch) {
			case '=':
				return ReadCompare ('=');
			case '>':
				return ReadCompare ('>');
			case '<':
				return ReadCompare ('<');
			case '+':
			case '-':
			case '*':
			case '/':
			case '%':
			case '^':
			case '(':
			case ')':
			case '[':
			case ']':
			case '{':
			case '}':
			case ',':
			case ';':
			case '.':
			case ':':
				return Token.OfIdentifier (char.ToString (((char)GetChar ())));
			case '\n':
				lineNumber++;
				GetChar ();
				return Token.OfIdentifier ("\\n");
			default:
				throw new LexicalException ("line {0}: Unrecognizable token '{1}'", lineNumber, (char)ch);
			}
		}

		Token ReadStr (char start_ch)
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
			throw new LexicalException ("line {0}: Missing closure for string declaration", lineNumber);
		}

		Token ReadUnescapedStr (char start_ch)
		{
			while (Peek () != -1) {
				if (Peek () == start_ch) {
					GetChar ();
					if (Peek () == start_ch) {
						GetChar ();
						s.Append (start_ch);/* "" and '' */
					} else
						return Token.OfString (s.ToString ());
					
				} else
					s.Append ((char)GetChar ());
			}
			throw new LexicalException ("line {0}: Missing closure for string declaration", lineNumber);
		}

		void ReadInteger ()
		{
			do {
				s.Append ((char)GetChar ());/* Read integer part */
			} while (IsDigit (Peek ()));
		}

		void ReadNumber ()
		{
			ReadInteger ();
			if (Peek () == '.') {/* Read decimal part*/
				GetChar ();
				s.Append ('.');
				do {
					s.Append ((char)GetChar ());
				} while (IsDigit (Peek ()));
			}
		}

		Token ReadCompare (char cmp)
		{
			GetChar ();
			if (Peek () == '=') {
				GetChar ();
				return Word.FromString (new string (new []{ cmp, '=' }));
			}
			return Word.FromString (char.ToString (cmp));
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
