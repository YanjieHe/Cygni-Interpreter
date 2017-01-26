using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.Errors;

namespace Cygni.AST
{
    public class LogicalEx:BinaryEx
    {
        public LogicalEx(NodeType type, ASTNode left, ASTNode right)
            : base(type, left, right)
        {
            if (type != NodeType.And && type != NodeType.Or)
            {
                throw new ArgumentException("Node type for logical expression should be 'and' or 'or'.", "type");
            }
        }

        public override DynValue Eval(IScope scope)
        {
            DynValue lvalue = this.left.Eval(scope);
            if (lvalue.IsBoolean)
            {
                /* shortcut evaluate*/
                if (this._type == NodeType.And)
                {
                    if ((bool)lvalue.Value)
                    {
                        DynValue rvalue = this.right.Eval(scope); 
                        if (rvalue.IsBoolean)
                        {
                            return (bool)rvalue.Value ? DynValue.True : DynValue.False;
                        }
                        else
                        {
                            throw new RuntimeException("Wrong argument type for operation '{0}'. Expected boolean type.", this._type);
                        }
                    }
                    else
                    {
                        return DynValue.False;
                    }
                }
                else
                {
                    if ((bool)lvalue.Value)
                    {
                        return DynValue.True;
                    }
                    else
                    {
                        DynValue rvalue = right.Eval(scope);
                        if (rvalue.IsBoolean)
                        {
                            return (bool)rvalue.Value ? DynValue.True : DynValue.False;
                        }
                        else
                        {
                            throw new RuntimeException("Wrong argument type for operation '{0}'. Expected boolean type.", this._type);
                        }
                    }
                }
            }
            else
            {
                throw new RuntimeException("Wrong argument type for operation '{0}'. Expected boolean type.", this._type);
            }
        }
    }
}

