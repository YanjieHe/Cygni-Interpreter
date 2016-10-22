using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Errors;
namespace Cygni.AST
{
	/// <summary>
	/// Description of ForEx.
	/// </summary>
	public class ForEx:ASTNode
	{
		BlockEx body;
		string iterator;
		ASTNode start;
		ASTNode end;
		ASTNode step;
		public override NodeType type { get { return NodeType.For; } }
		public ForEx(BlockEx body, string iterator, ASTNode start, ASTNode end, ASTNode step)
		{
			this.body = body;
			this.iterator = iterator;
			this.start = start;
			this.end = end;
			this.step = step;
		}
		public override DynValue Eval(IScope scope)
		{
			
			var result = DynValue.Null;
			int istart = (int)start.Eval(scope).AsNumber();
			int iend = (int)end.Eval(scope).AsNumber();
			
			if (step == null) {
				/* for i = start,end { ... } */
				var iter_var = new DynValue(DataType.Number, (double)istart);
				/* Please do not try to modify the iteration variable during the loop,
				 * it may cause unpredictable error */
				scope[iterator] = iter_var;
				
				for (int i = istart; i < iend; i++, iter_var.Value = (double)i) {
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
					
					var iter_var = new DynValue(DataType.Number, (double)istart);
					scope[iterator] = iter_var;
					
					for (int i = istart; i < iend; i += istep, iter_var.Value = (double)i) {
						
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
					var iter_var = new DynValue(DataType.Number, (double)istart);
					scope[iterator] = iter_var;
					
					for (int i = istart; i > iend; i += istep, iter_var.Value = (double)i) {
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
	}
}
