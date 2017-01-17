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
		readonly string name;
		readonly BlockEx body;
		readonly NameEx parent;
		Symbols fields;

		public BlockEx Body { get { return body; } }

		public NameEx Parent { get { return parent; } }

		internal Symbols Fields { get { return this.fields; } set { this.fields = value; } }

		public  override NodeType type { get { return NodeType.DefClass; } }

		public DefClassEx (string name, BlockEx body, NameEx parent)
		{
			this.name = name;
			this.body = body;
			this.parent = parent;
		}

		public override DynValue Eval (IScope scope)
		{
			if (scope.type != ScopeType.Module) {
				throw new Exception ("Scope Error");
			}
			ModuleScope GlobalScope = scope as ModuleScope;
			if (!GlobalScope.HasName (name)) {
				GlobalScope.Put (name, DynValue.Nil);
			}

			Symbols symbols = GlobalScope.GetSymbols ();
			ClassLookUpVisitor visitor = new ClassLookUpVisitor (symbols);
			this.Accept (visitor);
			DynValue[] values;
			DynObject parentClass;
			if (this.parent != null) {
				parentClass = parent.Eval (scope).As<DynObject> ();
				Dictionary<string, DynValue> parentFields = parentClass.GetFields ();

				foreach (var item in parentFields) {
					this.fields.PutLocal (item.Key);
				}
				values = new DynValue[this.fields.Count];
				for (int i = 0; i < values.Length; i++) {
					values [i] = DynValue.Nil;
				}
				foreach (var item in parentFields) {
					int index = this.fields.Find (item.Key);
					values [index] = item.Value;
				}
			} else {

				parentClass = null;
				values = new DynValue[this.fields.Count];
				for (int i = 0; i < values.Length; i++) {
					values [i] = DynValue.Nil;
				}
			}

			ClassScope classScope = new ClassScope (name, 
				                        this.fields.GetTable (), values, scope);
			this.body.Eval (classScope);
			DynValue newClass = new DynObject (name, classScope, false, parentClass);
			return GlobalScope.Put (name, newClass);
		}

		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}
