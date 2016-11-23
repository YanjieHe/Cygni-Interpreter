using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.Errors;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;

namespace Cygni.AST
{
	/// <summary>
	/// Description of DefClassEx.
	/// </summary>
	public class DefClassEx:ASTNode
	{
		string name;
		BlockEx body;
		public BlockEx Body{ get { return body; } }
		string parentName;
		public  override NodeType type { get { return NodeType.DefClass; } }
		
		public DefClassEx(string name, BlockEx body, string parentName)
		{
			this.name = name;
			this.body = body;
			this.parentName = parentName;
		}
		public override DynValue Eval(IScope scope)
		{
			var newScope = new NestedScope(scope);
			body.Eval(newScope);
			ClassInfo parentClass = null;
			if (parentName != null) {
				DynValue value = scope.Get (parentName);
				if (value.type != DataType.Class)
					throw  RuntimeException.NotDefined (parentName);
				parentClass = value.As<ClassInfo> ();
			}
			var constructor = DynValue.FromClass(new ClassInfo(name: name,classScope: newScope,body: body,parent: parentClass, IsInstance: false));
				return scope.Put(name, constructor);
		}
		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}
