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
	/// Description of DefFuncEx.
	/// </summary>
	public class DefFuncEx:ASTNode
	{
		string name;
		BlockEx body;
		public BlockEx Body{ get { return body; } }
		string[] parameters;
		public string[] Parameters{ get { return parameters; } }

		public override  NodeType type {get{return NodeType.DefFunc;}}
		public string Name{ get { return this.name; } }
		public DefFuncEx(string name, string[] parameters, BlockEx body)
		{
			this.name = name;
			this.parameters = parameters;
			this.body = body;
		}
		
		public override DynValue Eval(IScope scope)
		{
			var names = new List<NameEx> ();
			for (int i = 0; i < parameters.Length; i++) 
				names.Add (new NameEx (parameters [i],  i));
			LookUpVisitor visitor = new LookUpVisitor(names);
			body.Accept (visitor);
			var arrayScope = new ArrayScope (names.Count,scope);
			var func = DynValue.FromFunction(new Function(name,parameters.Length, body, arrayScope));
			return scope.Put(name, func);
		}
		public override string ToString()
		{
			return string.Concat(" def ", name, "(", string.Join(", ", parameters), ")", body);
		}
		internal override void Accept (ASTVisitor visitor)
		{
			visitor.Visit (this);
		}		
	}
}
