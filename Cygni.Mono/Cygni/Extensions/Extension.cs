using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST;

namespace Cygni.Extensions
{
    /// <summary>
    /// Description of Extension.
    /// </summary>
    public static class Extension
    {
        public static string OperatorToString(this NodeType type)
        {
            switch (type)
            {
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
                case NodeType.Plus:
                    return "+";
                case NodeType.Minus:
                    return "-";
                default:
                    return type.ToString();
            }
        }
    }
}
