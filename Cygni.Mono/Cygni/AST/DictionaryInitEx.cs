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

		KeyValuePair<ASTNode,ASTNode>[] initializers;

		public KeyValuePair<ASTNode,ASTNode>[]  Initializers { get { return this.initializers; } }

		public DictionaryInitEx (KeyValuePair<ASTNode,ASTNode>[]  initializers)
		{
			this.initializers = initializers;
		}

		public override DynValue Eval (IScope scope)
		{
			DynDictionary dict = new DynDictionary (initializers.Length);
			foreach (var item in initializers) {
				dict.Add (item.Key.Eval (scope), item.Value.Eval (scope));
			}
			return DynValue.FromDictionary (dict);
		}

		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}

