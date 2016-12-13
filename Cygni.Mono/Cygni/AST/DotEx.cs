using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;
using Cygni.Libraries;
using Cygni.Errors;
namespace Cygni.AST
{
	/// <summary>
	/// Description of DotEx.
	/// </summary>
	public class DotEx:ASTNode,IAssignable
	{
		readonly ASTNode target;
		public ASTNode Target{ get { return target; } }
		readonly string fieldName;
		public string FieldName{ get { return fieldName; } }

		public override  NodeType type { get { return NodeType.Dot; } }
		
		public DotEx(ASTNode target, string fieldName)
		{
			this.target = target;
			this.fieldName = fieldName;
		}
		public DynValue Set(IDot expression, DynValue value)
		{
			return expression.SetByDot(fieldName, value);
		}
		#region implemented abstract members of ASTNode

		public override DynValue Eval(IScope scope)
		{
			DynValue target_value = target.Eval(scope);
			if (target_value.type == DataType.String) {
				string str = target_value.Value as string;
				return StrGetByDot(str, fieldName);
			} else {
				IDot value = target_value.Value as IDot;
				if (value == null) {
					throw new RuntimeException("target '{0}' doesn't have any fields. Apparently it does not have field '{1}'.", target, fieldName);
				} else {
					return value.GetByDot(fieldName);
				}
			}
		}

		#endregion

		public DynValue Assign(DynValue value,IScope scope){
			DynValue target_value = target.Eval(scope);
			IDot t_value = target_value.Value as IDot;
			if (t_value == null) {
				throw new RuntimeException("target '{0}' doesn't have assignable fields. Apparently it does not have assignable field '{1}'.",target, fieldName);
			} else {
				return t_value.SetByDot (fieldName, value);
			}
		}


		public override string ToString ()
		{
			return string.Format ("{0}.{1}", target, fieldName);
		}
		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
		static DynValue StrGetByDot (string str, string fieldName)
		{
			switch (fieldName) {
			case "length":
				return (double)str.Length;
			case "replace":
				return DynValue.FromDelegate ((args) => StrLib.replace (str, args));
			case "format":
				return DynValue.FromDelegate ((args) => StrLib.format (str, args));
			case "join":
				return DynValue.FromDelegate ((args) => StrLib.join (str, args));
			case "split":
				return DynValue.FromDelegate ((args) => StrLib.split (str, args));
			case "find":
				return DynValue.FromDelegate ((args) => StrLib.find (str, args));
			case "lower":
				return DynValue.FromDelegate ((args) => str.ToLower ());
			case "upper":
				return DynValue.FromDelegate ((args) => str.ToUpper ());
			case "trim":
				return DynValue.FromDelegate ((args) => StrLib.trim (str, args));
			case "trimStart":
				return DynValue.FromDelegate ((args) => StrLib.trimStart (str, args));
			case "trimEnd":
				return DynValue.FromDelegate ((args) => StrLib.trimEnd (str, args));
			case "subString":
				return DynValue.FromDelegate ((args) => StrLib.subString (str, args));
			default:
				throw RuntimeException.NotDefined (fieldName);
			}
		}

	}
}
