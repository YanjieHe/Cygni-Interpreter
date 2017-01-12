using System;
using System.Text;

namespace Cygni.AST.Visitors
{
    internal class ASTStringBuilder:ASTVisitor
    {
        StringBuilder builder;

        public ASTStringBuilder()
        {
            this.builder = new StringBuilder();
        }

        internal override void Visit(AssignEx node)
        {
            node.Target.Accept(this);
            builder.Append(" = ");
            node.Value.Accept(this);
        }

        internal override void Visit(BinaryEx node)
        {
            
        }
    }
}

