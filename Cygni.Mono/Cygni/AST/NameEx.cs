using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;

namespace Cygni.AST
{
	/// <summary>
	/// Description of NameEx.
	/// </summary>
	public sealed class NameEx:ASTNode,IAssignable
	{
		string name;

		public string Name{ get { return name; } }

		public  override NodeType type { get { return NodeType.Name; } }

		int index;

		internal void SetIndex(int index){
			this.index = index;
		}

		public bool IsLocal {
			get{ return index != -1; }
		}

		public NameEx (string name, int index = -1)
		{
			this.name = name;
			this.index = index;
		}

		public override DynValue Eval (IScope scope)
		{
			if (index == (-1)) {
				return scope.Get (name);		
			} else {
				return scope.Get (index);
			}
		}

		public DynValue Assign (DynValue value, IScope scope)
		{
			if (index == (-1)) {
				return scope.Put (name, value);		
			} else {
				return scope.Put (index, value);
			}
		}

		public override string ToString ()
		{
			return name;
		}

		public override bool Equals (object obj)
		{
			var a = obj as NameEx;
			if (a == null)
				return false;
			return name == a.name;
		}

		public override int GetHashCode ()
		{
			return name.GetHashCode ();
		}
			
		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}
