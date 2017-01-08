using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.Libraries;
using Cygni.Errors;
using Cygni.AST.Interfaces;
using Cygni.DataTypes.Interfaces;

namespace Cygni.AST
{
    /// <summary>
    /// Description of DotEx.
    /// </summary>
    public class DotEx:ASTNode
    {
        readonly ASTNode target;

        public ASTNode Target{ get { return target; } }

        readonly string fieldName;

        public string FieldName{ get { return fieldName; } }

        public override  NodeType type { get { return NodeType.Dot; } }

        public DotEx(ASTNode target, string fieldName)
        {
            this.target = target;
            this.fieldName = fieldName;
        }

        public DynValue Set(IDot expression, DynValue value)
        {
            return expression.SetByDot(fieldName, value);
        }

        #region implemented abstract members of ASTNode

        public override DynValue Eval(IScope scope)
        {
            DynValue target_value = target.Eval(scope);
            if (target_value.type == DataType.String)
            {
                string str = target_value.Value as string;
                return StrLib.StrGetByDot(str, fieldName);
            }
            else
            {
                IDot value = target_value.Value as IDot;
                if (value == null)
                {
                    throw new RuntimeException("target '{0}' doesn't have any fields. Apparently it does not have field '{1}'.", target, fieldName);
                }
                else
                {
                    return value.GetByDot(fieldName);
                }
            }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0}.{1}", target, fieldName);
        }

        internal override void Accept(ASTVisitor visitor)
        {
            visitor.Visit(this);
        }

       

    }
}
