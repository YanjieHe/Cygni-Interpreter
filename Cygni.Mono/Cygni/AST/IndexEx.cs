using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// <summary>
    /// Description of IndexEx.
    /// </summary>
    public class IndexEx:ASTNode
    {
        public override NodeType type{ get { return NodeType.Index; } }

        readonly ASTNode collection;
        readonly ASTNode[] indexes;

        public ASTNode Collection { get { return collection; } }

        public ASTNode[] Indexes { get { return indexes; } }

        public IndexEx(ASTNode collection, ICollection<ASTNode> indexes)
        {
            this.collection = collection;
            this.indexes = new ASTNode[indexes.Count];
            indexes.CopyTo(this.indexes, 0);
        }

        public override DynValue Eval(IScope scope)
        {
            DynValue collection = this.collection.Eval(scope);
            int n = this.indexes.Length;
            DynValue[] indexes = new DynValue[n];
            for (int i = 0; i < n; i++)
                indexes[i] = this.indexes[i].Eval(scope);
            if (collection.IsString)
            {
                RuntimeException.IndexerArgsCheck(n == 1, "string");
                DynValue index = indexes[0];
                if (index.IsRange)
                {
                    return StrLib.Slice(collection.AsString(), index.Value as Range);
                }
                else
                {
                    return collection.AsString()[index.AsInt32()];
                }
            }
            return collection.As<IIndexable>().GetByIndexes(indexes);
        }

        internal override void Accept(ASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
