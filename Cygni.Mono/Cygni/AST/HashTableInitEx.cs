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
	public class HashTableInitEx:ASTNode
	{
		public override NodeType type { get { return NodeType.HashTableInit; } }
		List<ASTNode> list;
		public List<ASTNode> _List { get { return this.list; } }
		public HashTableInitEx (List<ASTNode> list)
		{
			this.list = list;
		}
		public override DynValue Eval (IScope scope)
		{
			DynHashTable ht = new DynHashTable ();
			int n = list.Count - 1;
			for(int i = 0; i < n;i += 2)
				ht.Add (list [i].Eval (scope), list [i + 1].Eval (scope));
			return DynValue.FromHashTable (ht);
		}
		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}

