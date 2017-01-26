using System;
using System.Collections.Generic;

namespace Cygni.AST.Visitors
{
    internal abstract class ASTVisitor
    {
        internal virtual void VisitBinary(BinaryEx node)
        {
            node.Left.Accept(this);
            node.Right.Accept(this);
        }

        internal virtual void VisitArguments(ASTNode[] arguments)
        {
            for (int i = 0; i < arguments.Length; i++)
            {
                arguments[i].Accept(this);
            }
        }

        internal virtual void Visit(LogicalEx node)
        {
            node.Left.Accept(this);
            node.Right.Accept(this);
        }

        internal virtual void Visit(AssignEx assignEx)
        {
            assignEx.Target.Accept(this);
            assignEx.Value.Accept(this);
        }


        internal virtual void Visit(RangeInitEx rangeEx)
        {
            rangeEx.Start.Accept(this);
            rangeEx.End.Accept(this);
            if (rangeEx.Step != null)
            {
                rangeEx.Step.Accept(this);
            }
        }

        internal virtual void Visit(BlockEx blockEx)
        {
            foreach (var item in blockEx.Expressions)
                item.Accept(this);
        }

        internal virtual void Visit(DefVarEx node)
        {
            foreach (var item in node.VariableDefs)
            {
                item.Variable.Accept(this);
                item.Value.Accept(this);
            }
        }

        internal virtual void Visit(Constant constant)
        {
            return;
        }

        internal virtual void Visit(DefClassEx defClassEx)
        {
            defClassEx.Body.Accept(this);
        }

        internal virtual void Visit(DefFuncEx defFuncEx)
        {
            defFuncEx.Body.Accept(this);
        }

        internal virtual void Visit(DefClosureEx defClosureEx)
        {
            defClosureEx.Body.Accept(this);
        }

        internal virtual void Visit(DotEx dotEx)
        {
            dotEx.Target.Accept(this);
        }

        internal virtual void Visit(ForEx forEx)
        {
            forEx.Iterator.Accept(this);
            forEx.Collection.Accept(this);
            forEx.Body.Accept(this);
        }

        internal virtual void Visit(ConditionEx conditionEx)
        {
            foreach (Branch branch in conditionEx.Branches)
            {
                branch.Condition.Accept(this);
                branch.Block.Accept(this);
            }
            if (conditionEx.ElseBlock != null)
            {
                conditionEx.ElseBlock.Accept(this);
            }
        }

        internal virtual void Visit(IfEx ifEx)
        {
            ifEx.Condition.Accept(this);
            ifEx.IfTrue.Accept(this);
            if (ifEx.IfFalse != null)
            {
                ifEx.IfFalse.Accept(this);
            }
        }

        internal virtual void Visit(IndexEx indexEx)
        {
            indexEx.Collection.Accept(this);
            foreach (var item in indexEx.Indexes)
                item.Accept(this);
        }

        internal virtual void Visit(SingleIndexEx indexEx)
        {
            indexEx.Collection.Accept(this);
            indexEx.Index.Accept(this);
        }

        internal virtual void Visit(InvokeEx invokeEx)
        {
            invokeEx.Func.Accept(this);
            foreach (var item in invokeEx.Arguments)
                item.Accept(this);
        }

        internal virtual void Visit(InitEx node)
        {
            VisitArguments(node.Arguments);
        }

        internal virtual void Visit(NameEx nameEx)
        {
            nameEx.Accept(this);
            return;
        }

        internal virtual void Visit(ReturnEx returnEx)
        {
            returnEx.Value.Accept(this);
        }

        internal virtual void Visit(UnaryEx unaryEx)
        {
            unaryEx.Operand.Accept(this);
        }

        internal virtual void Visit(WhileEx whileEx)
        {
            whileEx.Condition.Accept(this);
            whileEx.Body.Accept(this);
        }

    }
}

