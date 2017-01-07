using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.Errors;
using Cygni.Libraries;
using Cygni.DataTypes.Interfaces;

namespace Cygni.DataTypes
{
	public sealed class DynTuple:IDot
	{
		readonly DynValue[] values;

		public DynValue[] Values{ get { return this.values; } }

		public DynTuple (DynValue[] values)
		{
			this.values = values;
		}

		public DynValue GetByDot (string fieldName)
		{
			if (fieldName.Length > 1 && fieldName [0] == '_') {
				int index = 0;
				char ch = fieldName [1];
				if (ch == '0') {
					throw RuntimeException.FieldNotExist ("tuple", fieldName);
				} else {
					if (!IsDigit(ch)) {
						throw RuntimeException.FieldNotExist ("tuple", fieldName);
					} else {
						index = ch - '0';
						for (int i = 2; i < fieldName.Length; i++) {
							index = index * 10 + fieldName [i];
						}
						return this.values [index - 1];
					}
				}
			} else {
				throw RuntimeException.FieldNotExist ("tuple", fieldName);
			}
		}

		private bool IsDigit(char ch)
		{
			return ch >= '0' && ch <= '9';
		}

		public DynValue SetByDot (string fieldName, DynValue value)
		{
			throw RuntimeException.FieldNotExist ("tuple", fieldName);
		}


		public string[] FieldNames {
			get {
				var names = new string[this.values.Length];
				for (int i = 0; i < this.values.Length; i++) {
					names [i] = "_" + (i + 1);
				}
				return names;
			}
		}

		public override string ToString ()
		{
			return "(" + string.Join<DynValue> (", ", this.values) + ")";
		}
	}
}

