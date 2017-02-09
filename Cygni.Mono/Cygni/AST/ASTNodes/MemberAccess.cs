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
    public class MemberAccess:AssignEx
    {
        public MemberAccess(ASTNode target, ASTNode value)
            : base(NodeType.MemberAccess, target, value)
        {
        }

        public override DynValue Eval(IScope scope)
        {
            DynValue value = this.right.Eval(scope);
            DotEx dotEx = this.left as DotEx;
            DynValue target = dotEx.Target.Eval(scope);
            return target.As<IDot>().SetByDot(dotEx.FieldName, value);
        }
    }
}

