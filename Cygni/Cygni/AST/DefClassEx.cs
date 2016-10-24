using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.Errors;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.AST.Scopes;

namespace Cygni.AST
{
	/// <summary>
	/// Description of DefClassEx.
	/// </summary>
	public class DefClassEx:ASTNode
	{
		string name;
		BlockEx body;
		string[] parentsName;
		public  override NodeType type { get { return NodeType.DefClass; } }
		
		public DefClassEx(string name, BlockEx body, string[] parentsName)
		{
			this.name = name;
			this.body = body;
			this.parentsName = parentsName;
		}
		public override DynValue Eval(IScope scope)
		{
			var newScope = new NestedScope(scope);
			body.Eval(newScope);
			if (parentsName == null) {
				var func = DynValue.FromClass(new ClassInfo(name, body, newScope));
				return scope.Put(name, func);
			} else {
				var parentsClasses = new ClassInfo[parentsName.Length];
				
				for (int i = 0; i < parentsName.Length; i++) {
					var _class = scope.Get(parentsName[i]);
					if (_class.type != DataType.Class)
						throw RuntimeException.NotDefined(parentsName[i]);
					parentsClasses[i] = _class.Value as ClassInfo;
				}
				var func = DynValue.FromClass(new ClassInfo(name, body, newScope, parentsClasses));
				return scope.Put(name, func);
			}
		}
	}
}
