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

        internal void VisitParameters(NameEx[] parameters)
        {
            if (parameters.Length == 0)
            {
                return;
            }
            else
            {
                builder.Append(parameters[0]);
                for (int i = 1; i < parameters.Length; i++)
                {
                    builder.Append(", ");
                    builder.Append(parameters[i]);
                }
            }
        }

        internal override void VisitArguments(Cygni.AST.Interfaces.IArgumentProvider arguments)
        {
            if (arguments.ArgumentCount == 0)
            {
                return;
            }
            else
            {
                builder.Append(arguments.GetArgument(0));
                for (int i = 1; i < arguments.ArgumentCount; i++)
                {
                    builder.Append(", ");
                    builder.Append(arguments.GetArgument(i));
                }
            }
        }

        internal override void Visit(DefFuncEx node)
        {
            if (node.type == NodeType.DefClosure)
            {
                builder.Append("lambda ");
                builder.Append("( ");
                VisitParameters(node.Parameters);
                builder.Append(" )");
                if (node.Body.type != NodeType.Block)
                {
                    builder.Append(" => ");
                }
            }
            else
            {
                builder.Append("def ");
                builder.Append(node.Name);
                builder.Append("( ");
                VisitParameters(node.Parameters);
                builder.Append(" )");
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
                VisitArguments(node);
                builder.Append(" ]");
            }
            else if (node.type == NodeType.DictionaryInit)
            {
                builder.Append("{ ");
                Join(" -> ", node.Initializers, item => item.Accept(this));
                builder.Append(" }");
            }
            else
            {
                throw new NotSupportedException(node.type.ToString());
            }
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

        internal override void Visit(IfEx node)
        {
            builder.Append("if ");
            bool first = true;
            foreach (var branch in node.Branches)
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
            if (node.HasElsePart)
            {
                builder.Append("else ");
                node.ElseBlock.Accept(this);
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
            node.Function.Accept(this);
            builder.Append("( ");
            builder.Append(" )");
            Join(", ", node.Arguments, i => i.Accept(this));
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

