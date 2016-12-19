using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Errors;
using System.Collections.Generic;
using System.Linq;
using Cygni.AST.Optimizers;

namespace Cygni.AST.Scopes
{
	public sealed class ClassScope:IScope
	{
		readonly IScope parent;
		readonly Dictionary<string,int> table;
		readonly DynValue[] values;
		readonly string name;

		public string Name { get { return this.name; } }

		public IScope Parent { get { return this.parent; } }

		public ScopeType type { get { return ScopeType.Class; } }

		public ClassScope (string name, Dictionary<string,int> table, DynValue[] values, IScope parent)
		{
			this.name = name;
			this.parent = parent;
			this.table = table;
			this.values = values;
		}

		internal Symbols GetSymbols ()
		{
			Symbols symbols = new Symbols ();
			foreach (var item in table) {
				symbols.AddSymbol (item.Key, item.Value);
			}
			return symbols;
		}

		internal string[] FieldNames ()
		{
			string[] names = new string[this.table.Count];
			int i = 0;
			foreach (var item in this.table.Keys) {
				names [i] = item;
				i++;
			}
			return names;
		}

		public DynValue Get (string name)
		{
			int index;
			if (table.TryGetValue (name, out index)) {
				return values [index];
			} else {
				throw RuntimeException.FieldNotExist (this.name, name);
			}
		}

		public DynValue Put (string name, DynValue value)
		{
			int index;
			if (table.TryGetValue (name, out index)) {
				return values [index] = value;
			} else {
				throw RuntimeException.FieldNotExist (this.name, name);
			}
		}

		public DynValue Get (int nest, int index)
		{
			if (nest == 0) {
				return values [index];
			} else {
				return this.parent.Get (nest - 1, index);
			}
		}

		public DynValue Put (int nest, int index, DynValue value)
		{
			if (nest == 0) {
				return values [index] = value;
			} else {
				return this.parent.Get (nest - 1, index);
			}
		}

		public bool TryGetValue (string name, out DynValue value)
		{
			int index;
			if (table.TryGetValue (name, out index)) {
				value = values [index];
				return true;
			} else {
				value = DynValue.Nil;
				return false;
			}
		}

		public ClassScope Clone ()
		{
			DynValue[] values = new DynValue[this.values.Length];
			var newScope =	new ClassScope (name, table, values, parent);
			for (int i = 0; i < this.values.Length; i++) {
				if (this.values [i].type == DataType.Function) {
					values [i] = this.values [i].As<Function> ().Update (newScope);
				} else {
					values [i] = this.values [i];
				}
			}
			return newScope;
		}
	}
}

