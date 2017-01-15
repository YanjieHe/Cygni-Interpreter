using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.DataTypes.Interfaces;
using Cygni.Errors;


namespace Cygni.AST
{
    /// <summary>
    /// Description of UnaryEx.
    /// </summary>
    public class UnaryEx:ASTNode
    {
        readonly ASTNode operand;
        readonly NodeType op;

        public ASTNode Operand{ get { return operand; } }

        public override NodeType type { get { return this.op; } }

        public UnaryEx(NodeType op, ASTNode operand)
        {
            this.op = op;
            this.operand = operand;
        }

        public override DynValue Eval(IScope scope)
        {
            DynValue obj = Operand.Eval(scope);
            switch (op)
            {
                case NodeType.Plus:
                    {
                        if (obj.IsInteger)
                        {
                            return new DynValue(DataType.Integer, +(long)obj.Value);
                        }
                        else if (obj.IsNumber)
                        {
                            return new DynValue(DataType.Number, +(double)obj.Value);
                        }
                        else
                        {
                            return obj.As<IComputable>().UnaryPlus();
                        }
                    }
                case NodeType.Minus:
                    {
                        if (obj.IsInteger)
                        {
                            return new DynValue(DataType.Integer, -(long)obj.Value);
                        }
                        else if (obj.IsNumber)
                        {
                            return new DynValue(DataType.Number, -(double)obj.Value);
                        }
                        else
                        {
                            return obj.As<IComputable>().UnaryMinus();
                        }
                    }
                default: /* UnaryOp.Not */
                    {
                        if (obj.IsBoolean)
                        {
                            return (bool)obj.Value ? DynValue.False : DynValue.True;
                        }
                        else
                        {
                            throw new RuntimeException("'not' expected boolean type value");
                        }
                    }
            }
        }

        public string GetOperatorStr()
        {
            switch (op)
            {
                case NodeType.Plus:
                    return "+";
                case NodeType.Minus:
                    return "-";
                default: /* UnaryOp.Not */
                    return " not ";
            }
        }

        internal override void Accept(ASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

}
