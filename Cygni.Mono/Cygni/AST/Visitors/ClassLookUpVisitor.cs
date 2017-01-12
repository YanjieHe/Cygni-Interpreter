using System;
using System.Collections.Generic;
using Cygni.AST.Scopes;
using Cygni.AST.Optimizers;
using Cygni.Errors;

namespace Cygni.AST.Visitors
{
    internal class ClassLookUpVisitor:LookUpVisitor
    {
        internal ClassLookUpVisitor(Symbols symbols)
            : base(symbols)
        {
        }

        internal override void Visit(AssignEx assignEx)
        {
            if (assignEx.Target.type == NodeType.Name)
            {
                NameEx nameEx = assignEx.Target as NameEx;
                if (nameEx.Name == "this")
                {
                    throw new RuntimeException("Cannot assign to `this' because it is read-only.");
                }
            }
            base.Visit(assignEx);
        }

        internal override void Visit(DefVarEx node)
        {
            for (int i = 0; i < node.VariableDefs.Length; i++)
            {
                NameEx nameEx = node.VariableDefs[i].Variable;
                if (nameEx.Name == "this")
                {
                    throw new RuntimeException("Cannot assign to `this' because it is read-only.");
                }
            }
            base.Visit(node);
        }

        internal override void Visit(DefFuncEx defFuncEx)
        {
            this.symbols.PutLocal(defFuncEx.Name);
            base.Visit(defFuncEx);
        }

        internal override void Visit(DefClassEx defClassEx)
        {
            if (defClassEx.Parent != null)
            {
                this.Visit(defClassEx.Parent);
            }

            Symbols outerSymbols = this.symbols;
            Symbols newSymbols = new Symbols(this.symbols);
            this.symbols = newSymbols;
            this.symbols.PutLocal("this");
            defClassEx.Body.Accept(this);
            defClassEx.Fields = this.symbols;
            this.symbols = outerSymbols;
        }
    }
}

