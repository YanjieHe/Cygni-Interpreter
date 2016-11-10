using System.Linq;
using System;

namespace Cygni.Lexical.Tokens
{
	/// <summary>
	/// Description of Token.
	/// </summary>
	public class Token
	{
		public Tag tag{ get; protected set; }
		protected Token(Tag tag)
		{
			this.tag = tag;
		}
		
		public static readonly Token EOF = new Token(Tag.EOF);
		/* End of file */
		
		public static Token OfNumber(double number){
			return new NumToken(number);
		}
		
		public static Token OfNumber(string text){
			return new NumToken(text);
		}
		
		public static Token OfString(string text){
			return new StrToken(text);
		}
		
		public static Token OfIdentifier(string text){
			return Word.FromString(text);
		}
		
		public override string ToString()
		{
			return tag.ToString();
		}
	}
}
