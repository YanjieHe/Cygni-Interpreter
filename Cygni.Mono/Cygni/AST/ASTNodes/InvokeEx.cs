using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Errors;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.AST.Interfaces;
using Cygni.DataTypes.Interfaces;

namespace Cygni.AST
{
    /// <summary>
    /// Description of InvokeEx.
    /// </summary>
    public class InvokeEx:ASTNode
    {
        protected readonly ASTNode target;

        protected readonly ASTNode[] arguments;

        public ASTNode Func{ get { return target; } }

        public ASTNode[] Arguments{ get { return arguments; } }

        public override  NodeType type { get { return NodeType.Invoke; } }

        public InvokeEx(ASTNode target, ICollection<ASTNode> arguments)
        {
            this.target = target;
            this.arguments = new ASTNode[arguments.Count];
            arguments.CopyTo(this.arguments, 0);
        }

        public override DynValue Eval(IScope scope)
        {
            IFunction f = target.Eval(scope).Value as IFunction;
            if (f == null)
            {
                throw new RuntimeException("'{0}' is not a function.", target);
            }
            else
            {
                return f.DynEval(arguments, scope);
            }
        }

        internal override void Accept(ASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
