using System;

namespace Cygni.AST.Optimizers
{
	public sealed class Location
	{
		int nest;
		int index;

		public int Nest{ get { return this.nest; } }

		public int Index{ get { return this.index; } }

		public bool IsUnknown{ get { return this.nest == (-1); } }

		public static readonly Location Unknown = new Location (-1, 0);

		public Location (int nest, int index)
		{
			this.nest = nest;
			this.index = index;
		}

	}
}

