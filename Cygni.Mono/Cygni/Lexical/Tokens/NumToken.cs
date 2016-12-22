using System;

namespace Cygni.Lexical.Tokens
{
	/// <summary>
	/// Description of NumToken.
	/// </summary>
	public sealed class NumToken:Token
	{
		private readonly double value;
		public double Value { get { return value; } }
		public NumToken(double value)
			: base(Tag.Number)
		{
			this.value = value;
		}
		public NumToken(string strvalue)
			: base(Tag.Number)
		{
			this.value = double.Parse(strvalue);
		}
		public override string ToString()
		{
			return value.ToString();
		}
	}
}
