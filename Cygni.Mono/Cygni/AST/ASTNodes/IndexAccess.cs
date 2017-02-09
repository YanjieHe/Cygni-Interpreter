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
    public class IndexAccess:AssignEx
    {
        public IndexAccess(ASTNode target, ASTNode value)
            : base(NodeType.IndexAccess, target, value)
        {
        }

        public override DynValue Eval(IScope scope)
        {
            DynValue value = this.right.Eval(scope);
            IndexEx indexEx = this.left as IndexEx;
            DynValue collection = indexEx.Collection.Eval(scope);
            int n = indexEx.Indexes.Length;
            DynValue[] indexes = new DynValue[n];
            for (int i = 0; i < n; i++)
            {
                indexes[i] = indexEx.Indexes[i].Eval(scope);
            }
            return collection.As<IIndexable>().SetByIndexes(indexes, value);
        }
    }
}

