using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.AST.Interfaces;

namespace Cygni.AST
{
    public abstract class InitEx: ASTNode,IArgumentProvider
    {
        protected readonly ASTNode[] initializers;

        public ASTNode[] Initializers { get { return this.initializers; } }

        protected InitEx(ASTNode[] initializers)
        {
            this.initializers = initializers;
        }

        public int ArgumentCount { get { return this.initializers.Length; } }

        public ASTNode GetArgument(int index)
        {
            return this.initializers[index];
        }

        internal override void Accept(ASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

