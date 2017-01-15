using System;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.Errors;
using Cygni.AST.Interfaces;

namespace Cygni.AST
{
    public class RangeEx:ASTNode
    {
        ASTNode start;
        ASTNode end;
        ASTNode step;

        public ASTNode Start { get { return this.start; } }

        public ASTNode End { get { return this.end; } }

        public ASTNode Step { get { return this.step; } }

        public bool IsDefaultStep { get { return this.step == null; } }

        public override NodeType type
        {
            get
            {
                return NodeType.Range;
            }
        }

        public RangeEx(ASTNode start, ASTNode end, ASTNode step = null)
        {
            this.start = start;
            this.end = end;
            this.step = step;
        }

        internal override void Accept(ASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override DynValue Eval(IScope scope)
        {
            int start_value = this.start.Eval(scope).AsInt32();
            int end_value = this.end.Eval(scope).AsInt32();
            if (this.step == null)
            {
                return new Range(start_value, end_value);
            }
            else
            {
                int step_value = this.step.Eval(scope).AsInt32();
                return new Range(start_value, end_value, step_value);

            }
        }

    }
}

