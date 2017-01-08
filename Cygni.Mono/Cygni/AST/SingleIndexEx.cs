using System;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.Errors;
using Cygni.AST.Interfaces;
using Cygni.DataTypes.Interfaces;
using Cygni.Libraries;

namespace Cygni.AST
{
    public class SingleIndexEx: ASTNode
    {
        readonly ASTNode collection;
        readonly ASTNode index;

        public ASTNode Collection { get { return this.collection; } }

        public ASTNode Index { get { return this.index; } }

        public SingleIndexEx(ASTNode collection, ASTNode index)
        {
            this.collection = collection;
            this.index = index;
        }

        public override NodeType type{ get { return NodeType.SingleIndex; } }

        public override DynValue Eval(IScope scope)
        {
            DynValue collection = this.collection.Eval(scope);
            DynValue index = this.index.Eval(scope);
            if (collection.type == DataType.String)
            {
                if (index.IsRange)
                {
                    return StrLib.Slice(collection.AsString(), index.Value as Range);
                }
                else
                {
                    return collection.AsString()[index.AsInt32()];
                }
            }
            else
            {
                return collection.As<IIndexable>().GetByIndex(index);
            }
        }

        internal override void Accept(ASTVisitor visitor)
        {
            visitor.Visit(this);
        }

    }
}

