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
    public class SingleIndexEx: BinaryEx
    {
        public ASTNode Collection { get { return this.left; } }

        public ASTNode Index { get { return this.right; } }

        public SingleIndexEx(ASTNode collection, ASTNode index)
            : base(NodeType.SingleIndex, collection, index)
        {
        }

        public override DynValue Eval(IScope scope)
        {
            DynValue collection = this.left.Eval(scope);
            DynValue index = this.right.Eval(scope);
            if (collection.type == DataType.String)
            {
                if (index.IsRange)
                {
                    return StringLibrary.Slice(collection.AsString(), index.Value as Range);
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
    }
}

