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
using Cygni.AST.Interfaces;
using Cygni.DataTypes.Interfaces;

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

		public DotEx (ASTNode target, string fieldName)
		{
			this.target = target;
			this.fieldName = fieldName;
		}

		public DynValue Set (IDot expression, DynValue value)
		{
			return expression.SetByDot (fieldName, value);
		}

		#region implemented abstract members of ASTNode

		public override DynValue Eval (IScope scope)
		{
			DynValue target_value = target.Eval (scope);
			if (target_value.type == DataType.String) {
				string str = target_value.Value as string;
				return StrGetByDot (str, fieldName);
			} else {
				IDot value = target_value.Value as IDot;
				if (value == null) {
					throw new RuntimeException ("target '{0}' doesn't have any fields. Apparently it does not have field '{1}'.", target, fieldName);
				} else {
					return value.GetByDot (fieldName);
				}
			}
		}

		#endregion

		public DynValue Assign (DynValue value, IScope scope)
		{
			DynValue target_value = target.Eval (scope);
			IDot t_value = target_value.Value as IDot;
			if (t_value == null) {
				throw new RuntimeException ("target '{0}' doesn't have assignable fields. Apparently it does not have assignable field '{1}'.", target, fieldName);
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
				return DynValue.FromDelegate ("replace", (args) => StrLib.replace (str, args));
			case "format":
				return DynValue.FromDelegate ("format", (args) => StrLib.format (str, args));
			case "join":
				return DynValue.FromDelegate ("join", (args) => StrLib.join (str, args));
			case "split":
				return DynValue.FromDelegate ("split", (args) => StrLib.split (str, args));
			case "find":
				return DynValue.FromDelegate ("find", (args) => StrLib.find (str, args));
			case "lower":
				return DynValue.FromDelegate ("lower", (args) => str.ToLower ());
			case "upper":
				return DynValue.FromDelegate ("upper", (args) => str.ToUpper ());
			case "trim":
				return DynValue.FromDelegate ("trim", (args) => StrLib.trim (str, args));
			case "trimStart":
				return DynValue.FromDelegate ("trimStart", (args) => StrLib.trimStart (str, args));
			case "trimEnd":
				return DynValue.FromDelegate ("trimEnd", (args) => StrLib.trimEnd (str, args));
			case "slice":
				return DynValue.FromDelegate ("slice", (args) => StrLib.slice (str, args));
			default:
				throw RuntimeException.FieldNotExist ("string", fieldName);
			}
		}

	}
}
