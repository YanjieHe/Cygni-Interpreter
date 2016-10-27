using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Errors;
using Cygni.AST.Scopes;

namespace Cygni.AST
{
	/// <summary>
	/// Description of ForEx.
	/// </summary>
	public class ForEx:ASTNode,ISymbolLookUp
	{
		BlockEx body;
		public BlockEx Body{ get { return body; } }
		NameEx iterator;
		ASTNode start;
		ASTNode end;
		ASTNode step;
		public override NodeType type { get { return NodeType.For; } }
		public ForEx(BlockEx body, string iterator, ASTNode start, ASTNode end, ASTNode step)
		{
			this.body = body;
			this.iterator = new NameEx (iterator);
			this.start = start;
			this.end = end;
			this.step = step;
		}
		public override DynValue Eval(IScope scope)
		{
			
			var result = DynValue.Null;
			int istart = (int)start.Eval(scope).AsNumber();
			int iend = (int)end.Eval(scope).AsNumber();
			var iter_var = new DynValue(DataType.Number, (double)istart);
			/* Please do not try to modify the iteration variable during the loop,
			 * it may cause unpredictable error */

			iterator.Assign(iter_var,scope);
			
			if (step == null) {
				/* for i = start,end { ... } */
				
				//for (int i = istart; i < iend; i++, iter_var.Value = (double)i) {
				for (int i = istart; i < iend; i++, iterator.Assign((double)i,scope)) {
					result = body.Eval(scope);
					switch (result.type) {
						case DataType.Break:
							return DynValue.Null;
						case DataType.Continue:
							continue;
						case DataType.Return:
							return result;
					}
				}
			} else {
				/* for i = start,end,step { ... } */
				
				int istep = (int)step.Eval(scope).AsNumber();
				if (istep == 0)
					throw new RuntimeException("The step of for-loop cannot be zero.");
				if (istep > 0) { /* forward */
					
					//for (int i = istart; i < iend; i += istep, iter_var.Value = (double)i) {
					for (int i = istart; i < iend; i += istep, iterator.Assign((double)i,scope)) {
						result = body.Eval(scope);
						switch (result.type) {
							case DataType.Break:
								return DynValue.Null;
							case DataType.Continue:
								continue;
							case DataType.Return:
								return result;
						}
					}
				} else {/* backward */
					
					//for (int i = istart; i > iend; i += istep, iter_var.Value = (double)i) {
					for (int i = istart; i > iend; i += istep, iterator.Assign((double)i,scope)) {
						result = body.Eval(scope);
						switch (result.type) {
							case DataType.Break:
								return DynValue.Null;
							case DataType.Continue:
								continue;
							case DataType.Return:
								return result;
						}
					}
				}
			}
			return result;
		}
		public void LookUpForLocalVariable (List<NameEx>names)
		{
			//iterator.LookUpForLocalVariable (names);
			iterator = new NameEx(iterator.Name,names.Count);
			names.Add (iterator);

			if (start is ISymbolLookUp)
				(start as ISymbolLookUp).LookUpForLocalVariable (names);
			if (end is ISymbolLookUp)
				(end as ISymbolLookUp).LookUpForLocalVariable (names);
			if (step != null && step is ISymbolLookUp)
				(step as ISymbolLookUp).LookUpForLocalVariable (names);
			body.LookUpForLocalVariable (names);
		}

	}
}
