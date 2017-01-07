using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.AST.Interfaces;
using Cygni.Errors;

namespace Cygni.AST
{
	/// <summary>
	/// Description of NameEx.
	/// </summary>
	public sealed class NameEx:ASTNode
	{
		private string name;
		private int nest;
		private int index;

		public string Name{ get { return name; } }

		public int Nest{ get { return this.nest; } internal set { this.nest = value; } }

		public int Index{ get { return this.index; } internal set { this.index = value; } }

		public  override NodeType type { get { return NodeType.Name; } }

		public bool IsUnknown { get { return this.index == -1; }}

		public NameEx (string name, int index = -1)
		{
			this.name = name;
			this.index = index;
		}

		public override DynValue Eval (IScope scope)
		{
			if (IsUnknown) {
				return scope.Get (name);		
			} else {
				return scope.Get (nest, index);
			}
		}

		public DynValue Assign (DynValue value, IScope scope)
		{
			if (IsUnknown) {
				return scope.Put (name, value);		
			} else {
				return scope.Put (nest, index, value);
			}
		}

		public override string ToString ()
		{
			return name;
		}

		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}
