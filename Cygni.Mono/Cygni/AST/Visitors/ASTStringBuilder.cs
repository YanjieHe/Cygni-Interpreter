using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Cygni.Extensions;

namespace Cygni.AST.Visitors
{
    internal class ASTStringBuilder: ASTVisitor
    {
        private StringBuilder builder;

        public ASTStringBuilder()
        {
            this.builder = new StringBuilder();
        }

        public override string ToString()
        {
            return builder.ToString();
        }

        private void Join<T>(string separator, IEnumerable<T> collection, Action <T> action) where T:ASTNode
        {
            using (var iterator = collection.GetEnumerator())
            {
                if (iterator.MoveNext())
                {
                    action(iterator.Current);
                }

                while (iterator.MoveNext())
                {
                    builder.Append(separator);
                    action(iterator.Current);
                }
            }
        }

        internal override void Visit(AssignEx node)
        {
            node.Target.Accept(this);
            builder.Append(" = ");
            node.Value.Accept(this);
        }

        internal override void VisitBinary(BinaryEx node)
        {
            node.Left.Accept(this);
            builder.Append(" " + node.type.OperatorToString() + " ");
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

        internal override void Visit(DefClassEx node)
        {
            builder.Append("class ");
            builder.Append(node.Name);
            if (node.HasParent)
            {
                builder.Append(":");
                builder.Append(node.Parent.Name);
            }
            node.Body.Accept(this);
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

        internal override void Visit(InitEx node)
        {
            if (node.type == NodeType.ListInit)
            {
                builder.Append("[ ");
                Join(", ", node.Arguments, item => item.Accept(this));
                builder.Append(" ]");
            }
            else if (node.type == NodeType.DictionaryInit)
            {
                builder.Append("{ ");
                Join(", ", node.Arguments, item => item.Accept(this));
                builder.Append(" }");
            }
        }

        private void AppendKeyValuePair(KeyValuePair<ASTNode,ASTNode> pair)
        {
            pair.Key.Accept(this);
            builder.Append(':');
            pair.Value.Accept(this);
        }

        /* internal override void Visit(DictionaryInitEx node)
        {
            builder.Append("{ ");
            using (
                var iterator = node.Initializers
                .AsEnumerable<KeyValuePair<ASTNode,ASTNode>>()
                .GetEnumerator())
            {
                if (iterator.MoveNext())
                {
                    AppendKeyValuePair(iterator.Current);
                }
                while (iterator.MoveNext())
                {
                    builder.Append(", ");
                    AppendKeyValuePair(iterator.Current);
                }
            }
            builder.Append(" }");
        }*/

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
            indexEx.Collection.Accept(this);
            builder.Append("[ ");
            Join(", ", indexEx.Indexes, i => i.Accept(this));
            builder.Append(" ]");
        }

        internal override void Visit(InvokeEx node)
        {
            node.Func.Accept(this);
            builder.Append("( ");
            builder.Append(" )");
            Join(", ", node.Arguments, i => i.Accept(this));
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

        internal override void Visit(RangeInitEx node)
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
            builder.Append(unaryEx.type.OperatorToString());
            unaryEx.Operand.Accept(this);
        }

        internal override void Visit(WhileEx node)
        {
            builder.Append("while ");
            node.Condition.Accept(this);
            node.Body.Accept(this);
        }
    }
}

