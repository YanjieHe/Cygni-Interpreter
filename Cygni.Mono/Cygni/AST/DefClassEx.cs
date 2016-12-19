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
using Cygni.AST.Optimizers;

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
		Symbols fields;

		internal Symbols Fields { get { return this.fields; } set { this.fields = value; } }

		public  override NodeType type { get { return NodeType.DefClass; } }

		public DefClassEx (string name, BlockEx body, string parentName)
		{
			this.name = name;
			this.body = body;
			this.parentName = parentName;
		}

		public override DynValue Eval (IScope scope)
		{
			if (scope.type != ScopeType.ResizableArray) {
				throw new Exception ("Scope Error");
			}
			ResizableArrayScope GlobalScope = scope as ResizableArrayScope;
			if (GlobalScope.HasName (name)) {
				GlobalScope.Put (name, DynValue.Nil);
			}
			Symbols symbols = GlobalScope.GetSymbols ();
			ClassLookUpVisitor visitor = new ClassLookUpVisitor (symbols);
			this.Accept (visitor);
			DynValue[] values = new DynValue[this.fields.Count];
			for (int i = 0; i < values.Length; i++) {
				values [i] = DynValue.Nil;
			}
			ClassScope classScope = new ClassScope (name, 
				                        this.fields.GetTable (), values, scope);
			this.body.Eval (classScope);
			DynValue newClass = new DynObject (classScope, false, null);
			return GlobalScope.Put (name, newClass);
		}

		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}
