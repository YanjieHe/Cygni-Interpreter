using System;
using System.Text;

namespace Cygni.AST.Visitors
{
    internal class ASTStringBuilder: ASTVisitor
    {
        private StringBuilder builder;

        public ASTStringBuilder()
        {
            this.builder = new StringBuilder();
        }

        public string GetString()
        {
            return builder.ToString();
        }

        internal override void Visit(AssignEx node)
        {
            node.Target.Accept(this);
            builder.Append(" = ");
            node.Value.Accept(this);
        }

        internal override void Visit(BinaryEx node)
        {
            node.Left.Accept(this);
            builder.Append(" " + node.GetOperatorStr() + " ");
            node.Right.Accept(this);
        }

        internal override void Visit(BlockEx blockEx)
        {
            builder.AppendLine(" { ");
            foreach (var line in blockEx.Expressions)
            {
                line.Accept(this);
                builder.AppendLine();
            }
            builder.AppendLine("}");
        }

        internal override void Visit(ConditionEx conditionEx)
        {
            builder.Append("if ");
            bool first = true;
            foreach (var branch in conditionEx.Branches)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    builder.Append("elif ");
                }
                branch.Condition.Accept(this);
                branch.Block.Accept(this);
            }
            if (conditionEx.HasElsePart)
            {
                builder.Append("else ");
                conditionEx.ElseBlock.Accept(this);
            }
        }

        internal override void Visit(Constant constant)
        {
            builder.Append(constant.Value.ToString());
        }

        internal override void Visit(DefClassEx defClassEx)
        {
            base.Visit(defClassEx);
        }

        internal override void Visit(DefClosureEx node)
        {
            builder.Append("lambda ");
            builder.Append("( ");
            bool first = true;
            foreach (var item in node.Parameters)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    builder.Append(", ");
                }
                item.Accept(this);
            }
            if (node.Body.type != NodeType.Block)
            {
                builder.Append(" => ");
            }
            node.Body.Accept(this);
        }

        internal override void Visit(DefFuncEx node)
        {
            builder.Append("def ");
            builder.Append(node.Name);
            builder.Append("( ");
            bool first = true;
            foreach (var item in node.Parameters)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    builder.Append(", ");
                }
                item.Accept(this);
            }
            node.Body.Accept(this);
        }

        internal override void Visit(DefVarEx node)
        {
            builder.Append("var ");
            foreach (var item in node.VariableDefs)
            {
                builder.Append(item.Variable);
                builder.Append(" = ");
                builder.Append(item.Value);
            }
        }

        internal override void Visit(ListInitEx node)
        {
            builder.Append("[ ");
            bool first = true;
            foreach (var item in node.Initializers)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    builder.Append(", ");
                }
                item.Accept(this);
            }
            builder.Append(" ]");
        }

        internal override void Visit(DictionaryInitEx node)
        {
            builder.Append("{ ");
            bool first = true;
            foreach (var item in node.Initializers)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    builder.Append(", ");
                }
                item.Key.Accept(this);
                builder.Append(":");
                item.Value.Accept(this);
            }
            builder.Append(" }");
        }

        internal override void Visit(DotEx dotEx)
        {
            dotEx.Target.Accept(this);
            builder.Append(".");
            builder.Append(dotEx.FieldName);
        }

        internal override void Visit(ForEx node)
        {
            builder.Append("for ");
            node.Iterator.Accept(this);
            builder.Append(" in ");
            node.Collection.Accept(this);
            node.Body.Accept(this);
        }

        internal override void Visit(IfEx ifEx)
        {
            builder.Append("if ");
            ifEx.Condition.Accept(this);
            ifEx.IfTrue.Accept(this);
            if (ifEx.HasElsePart)
            {
                builder.Append("else ");
                ifEx.IfFalse.Accept(this);
            }
        }

        internal override void Visit(IndexEx indexEx)
        {
            base.Visit(indexEx);
        }

        internal override void Visit(InvokeEx node)
        {
            node.Func.Accept(this);
            bool first = true;
            foreach (var item in node.Arguments)
            {
                if (first)
                {
                    first = false;
                    item.Accept(this);
                }
                else
                {
                    builder.Append(", ");
                    item.Accept(this);
                }
            }
        }

        internal override void Visit(LogicalEx node)
        {
            node.Left.Accept(this);
            builder.Append(" " + node.type.ToString().ToLower() + " ");
            node.Right.Accept(this);
            base.Visit(node);
        }

        internal override void Visit(NameEx nameEx)
        {
            builder.Append(nameEx.Name);
        }

        internal override void Visit(RangeEx node)
        {
            node.Start.Accept(this);
            builder.Append(":");
            node.End.Accept(this);
            if (!node.IsDefaultStep)
            {
                builder.Append(":");
                node.End.Accept(this);
            }
        }

        internal override void Visit(ReturnEx node)
        {
            builder.Append("return (");
            node.Accept(this);
            builder.Append(") ");
        }

        internal override void Visit(SingleIndexEx node)
        {
            node.Collection.Accept(this);
            builder.Append("[ ");
            node.Index.Accept(this);
            builder.Append(" ]");
        }

        internal override void Visit(UnaryEx unaryEx)
        {
            base.Visit(unaryEx);
        }

        internal override void Visit(WhileEx node)
        {
            builder.Append("while ");
            node.Condition.Accept(this);
            node.Body.Accept(this);
        }
    }
}

