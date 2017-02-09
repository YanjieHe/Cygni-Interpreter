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
    /// <summary>
    /// Description of ListInitEx.
    /// </summary>
    public class ListInitEx:InitEx
    {
        public override NodeType type { get { return NodeType.ListInit; } }

        public ListInitEx(ASTNode[] initializers)
            : base(initializers)
        {
        }

        public override DynValue Eval(IScope scope)
        {
            DynList newList = new DynList(initializers.Length);
            foreach (ASTNode item in this.initializers)
            {
                newList.Add(item.Eval(scope));
            }
            return newList;
        }

        internal override void Accept(ASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
