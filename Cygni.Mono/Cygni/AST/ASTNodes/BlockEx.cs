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
    /// Description of BlockEx.
    /// </summary>
    public class BlockEx:ASTNode
    {
        readonly ASTNode[] expressions;

        public ASTNode[] Expressions{ get { return expressions; } }

        public  override NodeType type { get { return NodeType.Block; } }

        public BlockEx(ASTNode[] expressions)
        {
            this.expressions = expressions;
        }

        public static BlockEx EmptyBlock = new BlockEx(new ASTNode[0]);

        public override DynValue Eval(IScope scope)
        {
            DynValue result = DynValue.Nil;
            int n = expressions.Length;
            foreach (ASTNode line in this.expressions)
            {
                result = line.Eval(scope);
                switch (result.type)
                {
                    case DataType.Break:
                    case DataType.Continue:
                    case DataType.Return:
                        return result;
                }
            }
            return result;
        }

        internal override void Accept(ASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
