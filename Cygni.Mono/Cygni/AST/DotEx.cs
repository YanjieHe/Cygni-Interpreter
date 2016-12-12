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
		readonly ASTNode obj;
		public ASTNode Target{ get { return obj; } }
		readonly string fieldName;
		public string FieldName{ get { return fieldName; } }

		public override  NodeType type { get { return NodeType.Dot; } }
		
		public DotEx(ASTNode obj, string fieldName)
		{
			this.obj = obj;
			this.fieldName = fieldName;
		}
		public DynValue Set(IDot expression, DynValue value)
		{
			return expression.SetByDot(fieldName, value);
		}
		#region implemented abstract members of ASTNode

		public override DynValue Eval(IScope scope)
		{
			DynValue target = obj.Eval(scope);
			if (target.type == DataType.String) // 'string' is an exception
				return target.GetByDot (fieldName);
			else
				return target.As<IDot>().GetByDot(fieldName);
		}

		#endregion

		public DynValue Assign(DynValue value,IScope scope){
			DynValue target = obj.Eval(scope);
			return target.As<IDot> ().SetByDot (fieldName, value);
		}


		public override string ToString ()
		{
			return string.Format ("{0}.{1}", obj, fieldName);
		}
		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}
