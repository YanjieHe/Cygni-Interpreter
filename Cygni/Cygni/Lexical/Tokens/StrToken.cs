﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Cygni.Lexical.Tokens
{
	/// <summary>
	/// Description of StrToken.
	/// </summary>
	public class StrToken:Token
	{
		readonly string literal;
		public string Literal{ get { return literal; } }
		public StrToken(string literal)
			: base(Tag.String)
		{
			this.literal = literal;
		}
		public override string ToString()
		{
			return literal;
		}
	}
}
