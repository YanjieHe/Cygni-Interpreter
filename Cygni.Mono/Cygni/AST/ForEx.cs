using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Errors;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;

namespace Cygni.AST
{
	/// <summary>
	/// Description of ForEx.
	/// </summary>
	public class ForEx:ASTNode
	{
		readonly BlockEx body;
		readonly NameEx iterator;
		readonly ASTNode start;
		readonly ASTNode end;
		readonly ASTNode step;

		public BlockEx Body{ get { return body; } }

		public NameEx Iterator{ get { return iterator; } }

		public ASTNode Start{ get { return start; } }

		public ASTNode End{ get { return end; } }

		public ASTNode Step{ get { return step; } }

		public override NodeType type { get { return NodeType.For; } }

		public ForEx (BlockEx body, string iterator, ASTNode start, ASTNode end, ASTNode step)
		{
			this.body = body;
			this.iterator = new NameEx (iterator);
			this.start = start;
			this.end = end;
			this.step = step;
		}

		public override DynValue Eval (IScope scope)
		{
			
			DynValue result = DynValue.Nil;
			long istart = start.Eval (scope).AsInteger ();
			long iend = end.Eval (scope).AsInteger ();
			DynValue iter_var = new DynValue (DataType.Integer, istart);
			/* Please do not try to modify the iteration variable during the loop,
			 * it may cause unpredictable error */

			iterator.Assign (iter_var, scope);
			
			if (step == null) {
				/* for i = start,end { ... } */
				
				//for (int i = istart; i < iend; i++, iter_var.Value = (double)i) {
				for (long i = istart; i < iend; i++, iterator.Assign (i, scope)) {
					result = body.Eval (scope);
					switch (result.type) {
					case DataType.Break:
						return DynValue.Nil;
					case DataType.Continue:
						continue;
					case DataType.Return:
						return result;
					}
				}
			} else {
				/* for i = start,end,step { ... } */
				
				long istep = step.Eval (scope).AsInt32 ();
				if (istep == 0) {
					throw new RuntimeException ("The step of for-loop cannot be zero.");
				} else if (istep > 0) { /* forward */
					
					//for (int i = istart; i < iend; i += istep, iter_var.Value = (double)i) {
					for (long i = istart; i < iend; i += istep, iterator.Assign (i, scope)) {
						result = body.Eval (scope);
						switch (result.type) {
						case DataType.Break:
							return DynValue.Nil;
						case DataType.Continue:
							continue;
						case DataType.Return:
							return result;
						}
					}
				} else {/* backward */
					
					//for (int i = istart; i > iend; i += istep, iter_var.Value = (double)i) {
					for (long i = istart; i > iend; i += istep, iterator.Assign (i, scope)) {
						result = body.Eval (scope);
						switch (result.type) {
						case DataType.Break:
							return DynValue.Nil;
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

		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}
