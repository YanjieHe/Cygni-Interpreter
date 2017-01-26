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
    public class AssignEx: BinaryEx
    {
        public ASTNode Target { get { return this.left; } }

        public ASTNode Value { get { return this.right; } }

        public AssignEx(ASTNode target, ASTNode value)
            : base(NodeType.Assign, target, value)
        {
        }

        public override NodeType type { get { return NodeType.Assign; } }

        public override DynValue Eval(IScope scope)
        {
            DynValue value = this.right.Eval(scope);
            if (value.IsVoid)
            {
                throw RuntimeException.AssignVoidValue(scope);
            }
            else
            {
                switch (left.type)
                {
                    case NodeType.Name:
                        {
                            NameEx nameEx = this.left as NameEx;	
                            if (nameEx.IsUnknown)
                            {
                                return scope.Put(nameEx.Name, value);
                            }
                            else
                            {
                                return scope.Put(nameEx.Nest, nameEx.Index, value);
                            }
                        }
                    case NodeType.SingleIndex:
                        {
                            SingleIndexEx indexEx = this.left as SingleIndexEx;
                            DynValue collection = indexEx.Collection.Eval(scope);
                            DynValue index = indexEx.Index.Eval(scope);
                            return collection.As<IIndexable>().SetByIndex(index, value);
                        }
                    case NodeType.Index:
                        {
                            IndexEx indexEx = this.left as IndexEx;
                            DynValue collection = indexEx.Collection.Eval(scope);
                            int n = indexEx.Indexes.Length;
                            DynValue[] indexes = new DynValue[n];
                            for (int i = 0; i < n; i++)
                                indexes[i] = indexEx.Indexes[i].Eval(scope);
                            return collection.As<IIndexable>().SetByIndexes(indexes, value);
                        }
                    case NodeType.Dot:
                        {
                            DotEx dotEx = this.left as DotEx;
                            DynValue target = dotEx.Target.Eval(scope);
                            return target.As<IDot>().SetByDot(dotEx.FieldName, value);
                        }
                    default:
                        throw RuntimeException.Throw("left side is not assignable.", scope);
                }
            }
        }

        internal override void Accept(ASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

