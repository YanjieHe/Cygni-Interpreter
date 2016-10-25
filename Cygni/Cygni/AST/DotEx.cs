using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST.Scopes;

namespace Cygni.AST
{
	/// <summary>
	/// Description of DotEx.
	/// </summary>
	public class DotEx:ASTNode,IAssignable,ISymbolLookUp
	{
		ASTNode obj;
		public ASTNode Obj{ get { return obj; } }
		readonly string fieldname;
		public override  NodeType type { get { return NodeType.Dot; } }
		
		public DotEx(ASTNode obj, string fieldname)
		{
			this.obj = obj;
			this.fieldname = fieldname;
		}
		public DynValue Set(IDot expression, DynValue value)
		{
			return expression.SetByDot(fieldname, value);
		}
		#region implemented abstract members of ASTNode

		public override DynValue Eval(IScope scope)
		{
			var target = obj.Eval(scope);
			return target.As<IDot>().GetByDot(fieldname);
		}

		#endregion

		public DynValue Assign(DynValue value,IScope scope){
			var target = obj.Eval(scope);
			return target.As<IDot> ().SetByDot (fieldname, value);
		}

		public void LookUpForLocalVariable(List<NameEx> names){
			if (obj is ISymbolLookUp)
				(obj as ISymbolLookUp).LookUpForLocalVariable (names);
		}
	}
}
