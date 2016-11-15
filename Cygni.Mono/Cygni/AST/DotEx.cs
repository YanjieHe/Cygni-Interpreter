using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;

namespace Cygni.AST
{
	/// <summary>
	/// Description of DotEx.
	/// </summary>
	public class DotEx:ASTNode,IAssignable
	{
		ASTNode obj;
		public ASTNode Target{ get { return obj; } }
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
			if (target.type == DataType.String) // 'string' is an exception
				return target.GetByDot (fieldname);
			else
				return target.As<IDot>().GetByDot(fieldname);
		}

		#endregion

		public DynValue Assign(DynValue value,IScope scope){
			var target = obj.Eval(scope);
			return target.As<IDot> ().SetByDot (fieldname, value);
		}


		public override string ToString ()
		{
			return string.Format ("{0}.{1}", obj, fieldname);
		}
		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}
