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
    public abstract class InitEx:ASTNode
    {
        protected readonly ASTNode[] arguments;

        public ASTNode[] Arguments { get { return this.arguments; } }

        protected InitEx(ASTNode[] arguments)
        {
            this.arguments = arguments;
        }

        internal override void Accept(ASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

