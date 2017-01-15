using System;
using Cygni.DataTypes;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;

namespace Cygni.AST
{
    public class ConditionEx:ASTNode
    {
        Branch[] branches;
        BlockEx elseBlock;

        public Branch[] Branches { get { return this.branches; } }

        public BlockEx ElseBlock { get { return this.elseBlock; } }

        public bool HasElsePart { get { return this.elseBlock != null; } }

        public ConditionEx(Branch[] branches, BlockEx elseBlock = null)
        {
            this.branches = branches;
            this.elseBlock = elseBlock;
        }

        public override NodeType type
        {
            get
            {
                return NodeType.Condition;
            }
        }

        internal override void Accept(ASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override DynValue Eval(IScope scope)
        {
            foreach (Branch branch in branches)
            {
                bool test = branch.Condition.Eval(scope).AsBoolean();
                if (test)
                {
                    return branch.Block.Eval(scope);
                }
            }
            if (HasElsePart)
            {
                return elseBlock.Eval(scope);
            }
            else
            {
                return DynValue.Nil;
            }
        }
    }

    public sealed class Branch
    {
        ASTNode condition;
        BlockEx block;

        public ASTNode Condition { get { return this.condition; } }

        public ASTNode Block{ get { return this.block; } }

        public Branch(ASTNode condition, BlockEx block)
        {
            this.condition = condition;
            this.block = block;
        }
    }
}

