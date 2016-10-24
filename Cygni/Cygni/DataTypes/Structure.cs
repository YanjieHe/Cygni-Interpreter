using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.Errors;
namespace Cygni.DataTypes
{
	/// <summary>
	/// Description of Structure.
	/// </summary>
	public sealed class Structure:Dictionary<string,DynValue>, IDot
	{
		public Structure():base(){
		}
		#region IDot implementation

		public DynValue GetByDot(string fieldname)
		{
			return this[fieldname];
		}

		public DynValue SetByDot(string fieldname, DynValue value)
		{
			if(this.ContainsKey(fieldname))
				return this[fieldname] = value;
			throw RuntimeException.FieldNotExists ("structure", fieldname);
		}
		
		#endregion
		public override string ToString()
		{
			var s = new StringBuilder("struct: {\r\n");
			foreach (var item in this) {
				s
					.Append('\t')
					.Append(item.Key)
					.Append(" = ")
					.AppendLine(item.Value.ToString());
			}
			s.Append(" }");
			return s.ToString();
		}
	}
}
