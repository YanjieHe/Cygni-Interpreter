using System;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.Errors;
using Cygni.AST.Interfaces;
using Cygni.DataTypes.Interfaces;

namespace Cygni.AST
{
    public class AssignEx: BinaryEx
    {
        public ASTNode Target { get { return this.left; } }

        public ASTNode Value { get { return this.right; } }

        public AssignEx(ASTNode target, ASTNode value)
            : base(NodeType.Assign, target, value)
        {
        }

        protected AssignEx(NodeType type, ASTNode target, ASTNode value)
            : base(type, target, value)
        {
            
        }

        public override DynValue Eval(IScope scope)
        {
            /*if (value.IsVoid)
            {
                throw RuntimeException.AssignVoidValue(scope);
            }*/
            DynValue value = this.right.Eval(scope);
            NameEx nameEx = this.left as NameEx;	
            if (nameEx.IsUnknown)
            {
                return scope.Put(nameEx.Name, value);
            }
            else
            {
                return scope.Put(nameEx.Nest, nameEx.Index, value);
            }
        }

        protected RuntimeException NotAssignable(IScope scope)
        {
            return RuntimeException.Throw("left side is not assignable.", scope);
        }

        internal override void Accept(ASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

