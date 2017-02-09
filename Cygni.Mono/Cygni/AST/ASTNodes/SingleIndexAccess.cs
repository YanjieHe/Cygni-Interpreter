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
    public class SingleIndexAccess:AssignEx
    {
        public SingleIndexAccess(ASTNode target, ASTNode value)
            : base(NodeType.SingleIndexAccess, target, value)
        {
        }

        public override DynValue Eval(IScope scope)
        {
            DynValue value = this.right.Eval(scope);
            SingleIndexEx indexEx = this.left as SingleIndexEx;
            DynValue collection = indexEx.Collection.Eval(scope);
            DynValue index = indexEx.Index.Eval(scope);
            return collection.As<IIndexable>().SetByIndex(index, value);
        }
    }
}

