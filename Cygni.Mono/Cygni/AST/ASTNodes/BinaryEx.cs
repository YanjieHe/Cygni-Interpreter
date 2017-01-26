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
        protected readonly ASTNode left;
        protected readonly ASTNode right;
        protected readonly NodeType _type;

        public ASTNode Left{ get { return left; } }

        public ASTNode Right{ get { return right; } }

        public override NodeType type { get { return _type; } }

        public BinaryEx(NodeType _type, ASTNode left, ASTNode right)
        {
            this._type = _type;
            this.left = left;
            this.right = right;
        }

        public override DynValue Eval(IScope scope)
        {
            DynValue lvalue = left.Eval(scope);
            DynValue rvalue = right.Eval(scope);
            switch (_type)
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
                                return new DynValue(DataType.Integer, MathLibrary.IntegerPow((long)lvalue.Value, (int)(long)rvalue.Value));
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
                case NodeType.RightArrow:
                    {
                        return new DynValue(DataType.KeyValuePair, new KeyValuePair(lvalue, rvalue));
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
                case NodeType.NotEqual:
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
                default:
                    {
                        throw new RuntimeException("Not supported binary operator: {0}", _type);
                    }
            }
        }

        private RuntimeException BinaryError(IScope scope)
        {
            throw RuntimeException.Throw(
                string.Format("cannot implement binary operator '{0}' to '{1}' and '{2}'", _type, left, right), scope);
        }

        internal override void Accept(ASTVisitor visitor)
        {
            visitor.VisitBinary(this);
        }
    }
}
