using System;
using System.Collections.Generic;
using Cygni.Errors;

namespace Cygni.AST.Optimizers
{
	public class Symbols
	{
		Symbols parent;

		public Symbols Parent { get { return this.parent; } }

		Dictionary<string,int> table;

		public int Count{ get { return this.table.Count; } }

		public Symbols (Symbols parent = null)
		{
			this.parent = parent;
			this.table = new Dictionary<string, int> ();
		}

		public Location Get (string name, int nest = 0)
		{
			int index;
			if (table.TryGetValue (name, out index)) {
				return new Location (nest, index);
			} else {
				if (this.parent == null) {
					return Location.Unknown;
				} else {
					return this.parent.Get (name, nest + 1);
				}
			}
		}

		/*public int PutNew (string name)
		{
			int index;
			if (table.TryGetValue (name, out index)) {
				return index;
			} else {
				return AddName (name);		
			}
		}*/

		public Location Put (string name)
		{
			Location loc = Get (name, 0);
			if (loc.IsUnknown) {
				return new Location (0, AddName (name));
			} else {
				return loc;
			}
		}
		public Location PutLocal (string name)
		{
			int index;
			if (table.TryGetValue (name, out index)) {
				return new Location (0, index);
			} else {
				return new Location (0, AddName (name));
			}
		}

		private int AddName (string name)
		{
			int count = table.Count;
			table.Add (name, count);
			return count;
		}

		internal void AddSymbol (string name, int index)
		{
			this.table.Add (name, index);
		}

	}
}

