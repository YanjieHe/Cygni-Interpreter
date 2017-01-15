using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.Errors;
using Cygni.Libraries;
using Cygni.DataTypes.Interfaces;
namespace Cygni.AST
{
    /// <summary>
    /// Description of BinaryEx.
    /// </summary>
    public class BinaryEx:ASTNode
    {
        readonly ASTNode left;
        readonly ASTNode right;
        readonly NodeType op;

        public ASTNode Left{ get { return left; } }
        public ASTNode Right{ get { return right; } }
        public override NodeType type { get { return op; } }

        public BinaryEx(NodeType op, ASTNode left, ASTNode right)
        {
            this.op = op;
            this.left = left;
            this.right = right;
        }
        public override DynValue Eval(IScope scope)
        {
            DynValue lvalue = left.Eval(scope);
            DynValue rvalue = right.Eval(scope);
            switch (op)
            {
                case NodeType.Add:
                    {
                        if (lvalue.IsInteger)
                        {
                            if (rvalue.IsInteger)
                            { /* integer + integer */
                                return new DynValue(DataType.Integer, (long)lvalue.Value + (long)rvalue.Value);
                            }
                            else if (rvalue.IsNumber)
                            { /* integer + number */
                                return new DynValue(DataType.Number, (double)(long)lvalue.Value + (double)rvalue.Value);
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else if (lvalue.IsNumber)
                        {
                            if (rvalue.IsInteger)
                            { /* number + integer */
                                return new DynValue(DataType.Number, (double)lvalue.Value + (long)rvalue.Value);
                            }
                            else if (rvalue.IsNumber)
                            { /* number + number */
                                return new DynValue(DataType.Number, (double)lvalue.Value + (double)rvalue.Value);
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else
                        {
                            return lvalue.As<IComputable>().Add(rvalue);
                        }
                    }
                case NodeType.Subtract:
                    {
                        if (lvalue.IsInteger)
                        {
                            if (rvalue.IsInteger)
                            { /* integer - integer */
                                return new DynValue(DataType.Integer, (long)lvalue.Value - (long)rvalue.Value);
                            }
                            else if (rvalue.IsNumber)
                            { /* integer - number */
                                return new DynValue(DataType.Number, (double)(long)lvalue.Value - (double)rvalue.Value);
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else if (lvalue.IsNumber)
                        {
                            if (rvalue.IsInteger)
                            { /* number - integer */
                                return new DynValue(DataType.Number, (double)lvalue.Value - (long)rvalue.Value);
                            }
                            else if (rvalue.IsNumber)
                            { /* number - number */
                                return new DynValue(DataType.Number, (double)lvalue.Value - (double)rvalue.Value);
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else
                        {
                            return lvalue.As<IComputable>().Subtract(rvalue);
                        }
                    }
                case NodeType.Multiply:
                    {
                        if (lvalue.IsInteger)
                        {
                            if (rvalue.IsInteger)
                            { /* integer * integer */
                                return new DynValue(DataType.Integer, (long)lvalue.Value * (long)rvalue.Value);
                            }
                            else if (rvalue.IsNumber)
                            { /* integer * number */
                                return new DynValue(DataType.Number, (double)(long)lvalue.Value * (double)rvalue.Value);
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else if (lvalue.IsNumber)
                        {
                            if (rvalue.IsInteger)
                            { /* number * integer */
                                return new DynValue(DataType.Number, (double)lvalue.Value * (long)rvalue.Value);
                            }
                            else if (rvalue.IsNumber)
                            { /* number * number */
                                return new DynValue(DataType.Number, (double)lvalue.Value * (double)rvalue.Value);
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else
                        {
                            return lvalue.As<IComputable>().Multiply(rvalue);
                        }
                    }
                case NodeType.Divide:
                    {
                        if (lvalue.IsInteger)
                        {
                            if (rvalue.IsInteger)
                            { /* integer / integer */
                                return new DynValue(DataType.Number, ((double)(long)lvalue.Value) / ((double)(long)rvalue.Value));
                            }
                            else if (rvalue.IsNumber)
                            { /* integer / number */
                                return new DynValue(DataType.Number, ((double)(long)lvalue.Value / (double)rvalue.Value));
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else if (lvalue.IsNumber)
                        {
                            if (rvalue.IsInteger)
                            { /* number / integer */
                                return new DynValue(DataType.Number, ((double)lvalue.Value) / ((double)(long)rvalue.Value));
                            }
                            else if (rvalue.IsNumber)
                            { /* number / number */
                                return new DynValue(DataType.Number, ((double)lvalue.Value) / (double)rvalue.Value);
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else
                        {
                            return lvalue.As<IComputable>().Divide(rvalue);
                        }
                    }
                case NodeType.IntDiv:
                    {
                        if (lvalue.IsInteger)
                        {
                            if (rvalue.IsInteger)
                            { /* integer // integer */
                                return new DynValue(DataType.Integer, ((long)lvalue.Value) / ((long)rvalue.Value));
                            }
                            else if (rvalue.IsNumber)
                            { /* integer // number */
                                return new DynValue(DataType.Integer, ((long)lvalue.Value) / (long)(double)rvalue.Value);
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else if (lvalue.IsNumber)
                        {
                            if (rvalue.IsInteger)
                            { /* number // integer */
                                return new DynValue(DataType.Integer, ((long)(double)lvalue.Value) / ((long)rvalue.Value));
                            }
                            else if (rvalue.IsNumber)
                            { /* number // number */
                                return new DynValue(DataType.Integer, ((long)(double)lvalue.Value) / (long)(double)rvalue.Value);
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else
                        {
                            return lvalue.As<IComputable>().Divide(rvalue);
                        }
                    }
                case NodeType.Modulo:
                    {
                        if (lvalue.IsInteger)
                        {
                            if (rvalue.IsInteger)
                            { /* integer % integer */
                                return new DynValue(DataType.Integer, (long)lvalue.Value % (long)rvalue.Value);
                            }
                            else if (rvalue.IsNumber)
                            { /* integer % number */
                                return new DynValue(DataType.Number, ((double)(long)lvalue.Value) % (double)rvalue.Value);
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else if (lvalue.IsNumber)
                        {
                            if (rvalue.IsInteger)
                            { /* number % integer */
                                return new DynValue(DataType.Number, ((double)lvalue.Value) % ((double)(long)rvalue.Value));
                            }
                            else if (rvalue.IsNumber)
                            { /* number % number */
                                return new DynValue(DataType.Number, ((double)lvalue.Value) % (double)rvalue.Value);
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else
                        {
                            return lvalue.As<IComputable>().Modulo(rvalue);
                        }
                    }
                case NodeType.Power:
                    {
                        if (lvalue.IsInteger)
                        {
                            if (rvalue.IsInteger)
                            { /* integer ^ integer */
                                return new DynValue(DataType.Integer, MathLib.IntegerPow((long)lvalue.Value, (int)(long)rvalue.Value));
                            }
                            else if (rvalue.IsNumber)
                            { /* integer ^ number */
                                return new DynValue(DataType.Number, Math.Pow((double)lvalue.Value, (double)rvalue.Value));
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else if (lvalue.IsNumber)
                        {
                            if (rvalue.IsInteger)
                            { /* number ^ integer */
                                return new DynValue(DataType.Number, Math.Pow((double)lvalue.Value, (long)rvalue.Value));
                            }
                            else if (rvalue.IsNumber)
                            { /* number ^ number */
                                return new DynValue(DataType.Number, Math.Pow((double)lvalue.Value, (double)rvalue.Value));
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else
                        {
                            return lvalue.As<IComputable>().Power(rvalue);
                        }
                    }
                case NodeType.Concatenate:
                    {
                        return new DynValue(DataType.String, lvalue.Value.ToString() + rvalue.Value.ToString());
                    }
                case NodeType.LessThan:
                    {
                        if (lvalue.IsInteger)
                        {
                            if (rvalue.IsInteger)
                            { /* integer < integer */
                                return  (long)lvalue.Value < (long)rvalue.Value ? DynValue.True : DynValue.False;
                            }
                            else if (rvalue.IsNumber)
                            { /* integer < number */
                                return ((double)(long)lvalue.Value) < (double)rvalue.Value ? DynValue.True : DynValue.False;
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else if (lvalue.IsNumber)
                        { /* number < integer */
                            if (rvalue.IsInteger)
                            {
                                return  ((double)lvalue.Value) < ((double)(long)rvalue.Value) ? DynValue.True : DynValue.False;
                            }
                            else if (rvalue.IsNumber)
                            { /* number < number */
                                return  ((double)lvalue.Value) < ((double)rvalue.Value) ? DynValue.True : DynValue.False;
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else
                        {
                            return lvalue.As<IComparable<DynValue>>().CompareTo(rvalue) < 0;
                        }
                    }
                case NodeType.GreaterThan:
                    {
                        if (lvalue.IsInteger)
                        {
                            if (rvalue.IsInteger)
                            { /* integer > integer */
                                return  (long)lvalue.Value > (long)rvalue.Value ? DynValue.True : DynValue.False;
                            }
                            else if (rvalue.IsNumber)
                            { /* integer > number */
                                return ((double)(long)lvalue.Value) > (double)rvalue.Value ? DynValue.True : DynValue.False;
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else if (lvalue.IsNumber)
                        { /* number > integer */
                            if (rvalue.IsInteger)
                            {
                                return  ((double)lvalue.Value) > ((double)(long)rvalue.Value) ? DynValue.True : DynValue.False;
                            }
                            else if (rvalue.IsNumber)
                            { /* number > number */
                                return  ((double)lvalue.Value) > ((double)rvalue.Value) ? DynValue.True : DynValue.False;
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else
                        {
                            return lvalue.As<IComparable<DynValue>>().CompareTo(rvalue) > 0;
                        }
                    }
                case NodeType.LessThanOrEqual:
                    {
                        if (lvalue.IsInteger)
                        {
                            if (rvalue.IsInteger)
                            { /* integer <= integer */
                                return  (long)lvalue.Value <= (long)rvalue.Value ? DynValue.True : DynValue.False;
                            }
                            else if (rvalue.IsNumber)
                            { /* integer <= number */
                                return ((double)(long)lvalue.Value) <= (double)rvalue.Value ? DynValue.True : DynValue.False;
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else if (lvalue.IsNumber)
                        { /* number <= integer */
                            if (rvalue.IsInteger)
                            {
                                return  ((double)lvalue.Value) <= ((double)(long)rvalue.Value) ? DynValue.True : DynValue.False;
                            }
                            else if (rvalue.IsNumber)
                            { /* number <= number */
                                return  ((double)lvalue.Value) <= ((double)rvalue.Value) ? DynValue.True : DynValue.False;
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else
                        {
                            return lvalue.As<IComparable<DynValue>>().CompareTo(rvalue) <= 0;
                        }
                    }
                case NodeType.GreaterThanOrEqual:
                    {
                        if (lvalue.IsInteger)
                        {
                            if (rvalue.IsInteger)
                            { /* integer >= integer */
                                return  (long)lvalue.Value >= (long)rvalue.Value ? DynValue.True : DynValue.False;
                            }
                            else if (rvalue.IsNumber)
                            { /* integer >= number */
                                return ((double)(long)lvalue.Value) >= (double)rvalue.Value ? DynValue.True : DynValue.False;
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else if (lvalue.IsNumber)
                        { /* number >= integer */
                            if (rvalue.IsInteger)
                            {
                                return  ((double)lvalue.Value) >= ((double)(long)rvalue.Value) ? DynValue.True : DynValue.False;
                            }
                            else if (rvalue.IsNumber)
                            { /* number >= number */
                                return  ((double)lvalue.Value) >= ((double)rvalue.Value) ? DynValue.True : DynValue.False;
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else
                        {
                            return lvalue.As<IComparable<DynValue>>().CompareTo(rvalue) >= 0;
                        }
                    }
                case NodeType.Equal:
                    {
                        if (lvalue.IsInteger)
                        {
                            if (rvalue.IsInteger)
                            { /* integer == integer */
                                return  ((long)lvalue.Value == (long)rvalue.Value) ? DynValue.True : DynValue.False;
                            }
                            else if (rvalue.IsNumber)
                            { /* integer == number */
                                return ((double)(long)lvalue.Value) == (double)rvalue.Value ? DynValue.True : DynValue.False;
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else if (lvalue.IsNumber)
                        {
                            if (rvalue.IsInteger)
                            { /* number == integer */
                                return  (double)lvalue.Value == (double)(long)rvalue.Value ? DynValue.True : DynValue.False;
                            }
                            else if (rvalue.IsNumber)
                            { /* number == number */
                                return  (double)lvalue.Value == (double)rvalue.Value ? DynValue.True : DynValue.False;
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else
                        {
                            return lvalue.Equals(rvalue) ? DynValue.True : DynValue.False;
                        }
                    }
                default:/* NodeType.NotEqual */
                    {
                        if (lvalue.IsInteger)
                        {
                            if (rvalue.IsInteger)
                            { /* integer != integer */
                                return  ((long)lvalue.Value == (long)rvalue.Value) ? DynValue.False : DynValue.True;
                            }
                            else if (rvalue.IsNumber)
                            { /* integer != number */
                                return ((double)(long)lvalue.Value) == (double)rvalue.Value ? DynValue.False : DynValue.True;
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else if (lvalue.IsNumber)
                        {
                            if (rvalue.IsInteger)
                            { /* number != integer */
                                return  (double)lvalue.Value == (double)(long)rvalue.Value ? DynValue.False : DynValue.True;
                            }
                            else if (rvalue.IsNumber)
                            { /* number != number */
                                return  (double)lvalue.Value == (double)rvalue.Value ? DynValue.False : DynValue.True;
                            }
                            else
                            {
                                throw BinaryError(scope);
                            }
                        }
                        else
                        {
                            return lvalue.Equals(rvalue) ? DynValue.False : DynValue.True;
                        }
                    }
            }
        }

		private RuntimeException BinaryError(IScope scope){
            throw RuntimeException.Throw(
                string.Format("cannot implement binary operator '{0}' to '{1}' and '{2}'", op, left, right), scope);
		}

		public string GetOperatorStr(){
			switch(op) {
				case NodeType.Add:
                    return "+";
                case NodeType.Subtract:
                    return "-";
                case NodeType.Multiply:
                    return "*";
                case NodeType.Divide:
                    return "/";
                case NodeType.IntDiv:
                    return "//";
                case NodeType.Modulo:
                    return "%";
                case NodeType.Power:
                    return "^";
                case NodeType.Concatenate:
                    return "&";
                case NodeType.LessThan:
                    return "<";
                case NodeType.GreaterThan:
                    return ">";
                case NodeType.LessThanOrEqual:
                    return "<=";
                case NodeType.GreaterThanOrEqual:
                    return ">=";
                case NodeType.Equal:
                    return "==";
                case NodeType.NotEqual:
                    return "!=";
                default:
                    throw new NotSupportedException(op.ToString());
			}
		}

        internal override void Accept(ASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
