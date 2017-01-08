﻿using System;
using System.Collections.Generic;
using Cygni.AST.Scopes;
using Cygni.AST.Optimizers;
using Cygni.Errors;

namespace Cygni.AST.Visitors
{
    internal class LookUpVisitor:ASTVisitor
    {
        protected Symbols symbols;

        internal LookUpVisitor(Symbols symbols)
        {
            this.symbols = symbols;	
        }

        internal override void Visit(AssignEx assignEx)
        {
            if (assignEx.Target.type == NodeType.Name)
            {
                NameEx nameEx = assignEx.Target as NameEx;
                Location loc = symbols.Put(nameEx.Name);
                nameEx.Nest = loc.Nest;
                nameEx.Index = loc.Index;

                assignEx.Value.Accept(this);
            }
            else
            {
                base.Visit(assignEx);
            }
        }

        internal override void Visit(LocalEx localEx)
        {
            for (int i = 0; i < localEx.VariableDefs.Length; i++)
            {

                NameEx nameEx = localEx.VariableDefs[i].Variable;
                Location loc = symbols.PutLocal(nameEx.Name);
                nameEx.Nest = loc.Nest;
                nameEx.Index = loc.Index;

                localEx.VariableDefs[i].Value.Accept(this);
            }
        }

	
        internal override void Visit(UnpackEx unpackEx)
        {
            for (int i = 0; i < unpackEx.Items.Length; i++)
            {
                var item = unpackEx.Items[i];
                if (item.type == NodeType.Name)
                {
                    NameEx nameEx = item as NameEx;
                    Location loc = symbols.Put(nameEx.Name);
                    nameEx.Nest = loc.Nest;
                    nameEx.Index = loc.Index;

                    unpackEx.Tuple.Accept(this);
                }
                else
                {
                    base.Visit(unpackEx);
                }
            }
        }

        internal override void Visit(NameEx nameEx)
        {
            Location loc = symbols.Get(nameEx.Name);
            if (loc.IsUnknown)
            {
                throw RuntimeException.NotDefined(nameEx.Name); 
            }
            else
            {
                nameEx.Nest = loc.Nest;
                nameEx.Index = loc.Index;
            }
        }

        internal override void Visit(ForEx forEx)
        {
            NameEx nameEx = forEx.Iterator as NameEx;
            Location loc = symbols.PutLocal(nameEx.Name);
            nameEx.Nest = loc.Nest;
            nameEx.Index = loc.Index;

            forEx.Collection.Accept(this);
            forEx.Body.Accept(this);
        }

        internal override void Visit(DefFuncEx defFuncEx)
        {
            Symbols outerSymbols = this.symbols;
            Symbols newSymbols = new Symbols(this.symbols);
            this.symbols = newSymbols;
            for (int i = 0; i < defFuncEx.Parameters.Length; i++)
            {
                symbols.PutLocal(defFuncEx.Parameters[i].Name);
            }
            defFuncEx.Body.Accept(this);
            defFuncEx.Size = this.symbols.Count;
            this.symbols = outerSymbols;
        }

        internal override void Visit(DefClosureEx defClosureEx)
        {
            Symbols outerSymbols = this.symbols;
            Symbols newSymbols = new Symbols(this.symbols);
            this.symbols = newSymbols;
            for (int i = 0; i < defClosureEx.Parameters.Length; i++)
            {
                symbols.PutLocal(defClosureEx.Parameters[i].Name);
            }
            defClosureEx.Body.Accept(this);
            defClosureEx.Size = this.symbols.Count;
            this.symbols = outerSymbols;
        }
    }
}

