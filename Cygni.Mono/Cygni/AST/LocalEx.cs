using System;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.Errors;
using Cygni.AST.Interfaces;

namespace Cygni.AST
{
    public class LocalEx:ASTNode
    {
        VariableDefinition[] variableDefs;

        public VariableDefinition[] VariableDefs { get { return this.variableDefs; } }

        public override NodeType type
        {
            get
            {
                return NodeType.Local;
            }
        }

        public LocalEx(NameEx[] variables, ASTNode[] values)
        {
            this.variableDefs = new VariableDefinition[variables.Length];
            for (int i = 0; i < variables.Length; i++)
            {
                variableDefs[i] = new VariableDefinition(variables[i], values[i]);
            }
        }

        internal override void Accept(ASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override DynValue Eval(IScope scope)
        {
            foreach (VariableDefinition item in variableDefs)
            {
                if (item.Variable.IsUnknown)
                {
                    scope.Put(item.Variable.Name, item.Value.Eval(scope));
                }
                else
                {
                    scope.Put(item.Variable.Nest, item.Variable.Index, item.Value.Eval(scope));
                }
            }
            return DynValue.Void;
        }
    }

    public class VariableDefinition
    {
        readonly NameEx variable;
        readonly ASTNode value;

        public NameEx Variable { get { return this.variable; } }

        public ASTNode Value { get { return this.value; } }

        public VariableDefinition(NameEx variable, ASTNode value)
        {
            this.variable = variable;
            this.value = value;
        }
    }
}

