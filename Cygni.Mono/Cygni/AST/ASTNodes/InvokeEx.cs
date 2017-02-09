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
    public class InvokeEx:ASTNode, IArgumentProvider
    {
        readonly ASTNode function;

        readonly ASTNode[] arguments;



        public ASTNode Function { get { return function; } }

        public ASTNode[] Arguments{ get { return arguments; } }

        public override  NodeType type { get { return NodeType.Invoke; } }

        public int ArgumentCount { get { return arguments.Length; } }

        public ASTNode GetArgument(int index)
        {
            return this.arguments[index];
        }

        public InvokeEx(ASTNode function, ICollection<ASTNode> arguments)
        {
            this.function = function;
            this.arguments = new ASTNode[arguments.Count];
            arguments.CopyTo(this.arguments, 0);
        }

        public override DynValue Eval(IScope scope)
        {
            IFunction f = function.Eval(scope).Value as IFunction;
            if (f == null)
            {
                throw new RuntimeException("'{0}' is not a function.", Function);
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
