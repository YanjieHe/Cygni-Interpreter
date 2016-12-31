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
	public class DictionaryInitEx:ASTNode
	{
		public override NodeType type { get { return NodeType.DictionaryInit; } }

		ASTNode[] initializers;

		public ASTNode[] Initializers { get { return this.initializers; } }

		public DictionaryInitEx (ASTNode[] initializers)
		{
			this.initializers = initializers;
		}

		public override DynValue Eval (IScope scope)
		{
			DynDictionary ht = new DynDictionary ();
			int n = initializers.Length - 1;
			for (int i = 0; i < n; i += 2)
				ht.Add (initializers [i].Eval (scope), initializers [i + 1].Eval (scope));
			return DynValue.FromDictionary (ht);
		}

		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}

