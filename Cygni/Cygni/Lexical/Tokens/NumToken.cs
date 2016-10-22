using System.Linq;
using System;

namespace Cygni.Lexical.Tokens
{
	/// <summary>
	/// Description of NumToken.
	/// </summary>
	public class NumToken:Token
	{
		readonly double value;
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
