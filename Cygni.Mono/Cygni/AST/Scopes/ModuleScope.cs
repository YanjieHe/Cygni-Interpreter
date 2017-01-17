using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.DataTypes.Interfaces;
using Cygni.Errors;
using Cygni.Libraries;
using Cygni.AST.Optimizers;

namespace Cygni.AST.Scopes
{
	public class ModuleScope:IScope
	{
		private readonly string scopeName;
		private int count;
		private DynValue[] values;
		private readonly Dictionary<string, int> table;

		public ScopeType type { get { return ScopeType.Module; } }

		public IScope Parent { get { return null; } }

		public string ScopeName { get { return this.scopeName; } }

		public ModuleScope (string name = "main")
		{
			this.scopeName = name;
			this.table = new Dictionary<string,int> ();
			this.values = new DynValue[4];
			this.count = 0;
		}

		public int Find (string name)
		{
			int index;
			if (this.table.TryGetValue (name, out index)) {
				return index;
			} else {
				throw RuntimeException.NotDefined (name);
			}
		}

		internal Symbols GetSymbols ()
		{
			Symbols symbols = new Symbols ();
			foreach (var item in table) {
				symbols.AddSymbol (item.Key, item.Value);
			}
			return symbols;
		}

		internal string[] Names {
			get{ return this.table.Keys.ToArray (); }
		}

		public DynValue Get (string name)
		{
			int index;
			if (this.table.TryGetValue (name, out index)) {
				return this.values [index];
			} else {
				throw RuntimeException.NotDefined (name);
			}
		}

		public DynValue Put (string name, DynValue value)
		{
			int index;
			if (this.table.TryGetValue (name, out index)) {
				return this.values [index] = value;
			} else {
				EnsureCapacity ();
				this.values [this.count] = value;
				this.table.Add (name, this.count);
				this.count++;
				return value;
			}
		}

		public DynValue Get (int nest, int index)
		{
			if (nest == 0) {
				return this.values [index];
			} else {
				throw RuntimeException.NotDefined ("arg" + index);
			}
		}

		public DynValue Put (int nest, int index, DynValue value)
		{
			if (nest == 0) {
				return this.values [index] = value;
			} else {
				throw RuntimeException.NotDefined ("arg" + index);
			}
		}

		public bool HasName (string name)
		{
			return this.table.ContainsKey (name);
		}

		public bool TryGetValue (string name, out DynValue value)
		{
			int index;
			if (this.table.TryGetValue (name, out index)) {
				value = this.values [index];
				return true;
			} else {
				value = DynValue.Nil;
				return false;
			}
		}

		private void EnsureCapacity ()
		{
			if (this.count == this.values.Length) {
				Array.Resize (ref values, this.count * 2);
			}
		}

		public void BuiltIn ()
		{
			BuiltInLibrary.BuiltIn (this);
		}

	
	}
}

