using System;
using System.Collections.Generic;
using Cygni.AST.Scopes;
using Cygni.AST.Optimizers;
using Cygni.Errors;

namespace Cygni.AST.Visitors
{
	internal class LookUpVisitor:ASTVisitor
	{
		Symbols symbols;

		internal LookUpVisitor (Symbols symbols)
		{
			this.symbols = symbols;	
		}

		internal override void Visit (AssignEx assignEx)
		{
			if (assignEx.Target.type == NodeType.Name) {
				NameEx nameEx = assignEx.Target as NameEx;
				Location loc = symbols.Put (nameEx.Name);
				nameEx.Nest = loc.Nest;
				nameEx.Index = loc.Index;

				assignEx.Value.Accept (this);
			} else {
				base.Visit (assignEx);
			}
		}

		internal override void Visit (SetEx setEx)
		{
			for (int i = 0; i < setEx.Targets.Length; i++) {
				var item = setEx.Targets [i];
				if (item.type == NodeType.Name) {
					NameEx nameEx = item as NameEx;
					Location loc = symbols.Put (nameEx.Name);
					nameEx.Nest = loc.Nest;
					nameEx.Index = loc.Index;

					setEx.Values [i].Accept (this);
				} else {
					base.Visit (setEx);
				}
			}
		}

		internal override void Visit (LocalEx localEx){
			for (int i = 0; i < localEx.Variables.Length; i++) {

				NameEx nameEx = localEx.Variables[i];
				Location loc = symbols.PutLocal (nameEx.Name);
				nameEx.Nest = loc.Nest;
				nameEx.Index = loc.Index;

				localEx.Values[i].Accept(this);
			}
		}

		internal override void Visit (UnpackEx unpackEx)
		{
			for (int i = 0; i < unpackEx.Items.Length; i++) {
				var item = unpackEx.Items [i];
				if (item.type == NodeType.Name) {
					NameEx nameEx = item as NameEx;
					Location loc = symbols.Put (nameEx.Name);
					nameEx.Nest = loc.Nest;
					nameEx.Index = loc.Index;

					unpackEx.Tuple.Accept (this);
				} else {
					base.Visit (unpackEx);
				}
			}
		}

		internal override void Visit (NameEx nameEx)
		{
			Location loc = symbols.Get (nameEx.Name);
			if (loc.IsUnknown) {
				throw RuntimeException.NotDefined (nameEx.Name); 
			} else {
				nameEx.Nest = loc.Nest;
				nameEx.Index = loc.Index;
			}
		}

		internal override void Visit (ForEx forEx)
		{
			NameEx nameEx = forEx.Iterator as NameEx;
			Location loc = symbols.PutLocal (nameEx.Name);
			nameEx.Nest = loc.Nest;
			nameEx.Index = loc.Index;

			forEx.Start.Accept (this);
			forEx.End.Accept (this);
			if (forEx.Step != null) {
				forEx.Step.Accept (this);
			}
			forEx.Body.Accept (this);
		}

		internal override void Visit (ForEachEx forEachEx)
		{
			NameEx nameEx = forEachEx.Iterator as NameEx;
			Location loc = symbols.PutLocal (nameEx.Name);
			nameEx.Nest = loc.Nest;
			nameEx.Index = loc.Index;

			forEachEx.Collection.Accept (this);
			forEachEx.Body.Accept (this);
		}

		internal override void Visit (DefFuncEx defFuncEx)
		{
			for (int i = 0; i < defFuncEx.Parameters.Length; i++) {
				symbols.PutLocal (defFuncEx.Parameters [i]);
			}
			defFuncEx.Body.Accept (this);
		}

		internal override void Visit (DefClosureEx defClosureEx)
		{
			Symbols outerSymbols = this.symbols;
			Symbols newSymbols = new Symbols(this.symbols);
			this.symbols = newSymbols;
			for (int i = 0; i < defClosureEx.Parameters.Length; i++) {
				symbols.PutLocal (defClosureEx.Parameters [i].Name);
			}
			defClosureEx.Body.Accept (this);
			defClosureEx.Size = this.symbols.Count;
			this.symbols = outerSymbols;
		}
	}
}

