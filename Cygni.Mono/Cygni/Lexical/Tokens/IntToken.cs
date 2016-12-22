using System;

namespace Cygni.Lexical.Tokens
{
	public sealed class IntToken:Token
	{
		private readonly long value;
		public long Value { get { return value; } }
		public IntToken(long value)
			: base(Tag.Integer)
		{
			this.value = value;
		}
		public IntToken(string strvalue)
			: base(Tag.Integer)
		{
			this.value = long.Parse(strvalue);
		}
		public override string ToString()
		{
			return value.ToString();
		}
	}
}

