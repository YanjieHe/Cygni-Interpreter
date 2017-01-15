using System;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.Errors;
using Cygni.AST.Interfaces;
using Cygni.DataTypes.Interfaces;

namespace Cygni.AST
{
	public class AssignEx: ASTNode
	{
		readonly ASTNode target;
		readonly ASTNode value;

		public ASTNode Target { get { return this.target; } }

		public ASTNode Value { get { return this.value; } }

		public AssignEx (ASTNode target, ASTNode value)
		{
			this.target = target;
			this.value = value;
		}

		public override NodeType type { get { return NodeType.Assign; } }

		public override DynValue Eval (IScope scope)
		{
			DynValue right = value.Eval (scope);
			if (right.IsVoid) {
				throw RuntimeException.AssignVoidValue (scope);
			} else {
				switch (target.type) {
				case NodeType.Name:
					{
						NameEx nameEx = this.target as NameEx;	
						if (nameEx.IsUnknown) {
							return scope.Put (nameEx.Name, right);
						} else {
							return scope.Put (nameEx.Nest, nameEx.Index, right);
						}
					}
				case NodeType.SingleIndex:
					{
						SingleIndexEx indexEx = this.target as SingleIndexEx;
						DynValue collection = indexEx.Collection.Eval (scope);
						DynValue index = indexEx.Index.Eval (scope);
						return collection.As<IIndexable> ().SetByIndex (index, right);
					}
				case NodeType.Index:
					{
						IndexEx indexEx = this.target as IndexEx;
						DynValue collection = indexEx.Collection.Eval (scope);
						int n = indexEx.Indexes.Length;
						DynValue[] indexes = new DynValue[n];
						for (int i = 0; i < n; i++)
							indexes [i] = indexEx.Indexes [i].Eval (scope);
						return collection.As<IIndexable> ().SetByIndexes (indexes, right);
					}
				case NodeType.Dot:
					{
						DotEx dotEx = this.target as DotEx;
						DynValue target = dotEx.Target.Eval (scope);
						return target.As<IDot> ().SetByDot (dotEx.FieldName, right);
					}
				default:
					throw RuntimeException.Throw ("left side is not assignable.", scope);
				}
			}
		}

		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}


	}
}

