using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.AST;
using Cygni.Errors;
using Cygni.AST.Scopes;
using Cygni.DataTypes.Interfaces;

namespace Cygni.DataTypes
{
	public class Module: IDot
	{
		ModuleScope scope;

		public Module (ModuleScope scope)
		{
			this.scope = scope;
		}

		public DynValue GetByDot (string fieldName)
		{
			return this.scope.Get (fieldName);
		}

		public DynValue SetByDot (string fieldName, DynValue value)
		{
			return this.scope.Put (fieldName, value);
		}

		public string[] FieldNames {
			get {
				return this.scope.Names;
			}
		}

		public override string ToString ()
		{
			return "(Module)";
		}
	}
}

