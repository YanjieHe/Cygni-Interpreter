using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST.Scopes;

namespace Cygni.AST
{
	/// <summary>
	/// Description of DefFuncEx.
	/// </summary>
	public class DefFuncEx:ASTNode
	{
		string name;
		BlockEx body;
		string[] parameters;
		public override  NodeType type {get{return NodeType.DefFunc;}}
		
		public DefFuncEx(string name, string[] parameters, BlockEx body)
		{
			this.name = name;
			this.parameters = parameters;
			this.body = body;
		}
		
		public override DynValue Eval(IScope scope)
		{
			var list = new List<NameEx> ();
			for (int i = 0; i < parameters.Length; i++) 
				list.Add (new NameEx (parameters [i],  i));
			body.LookUpForLocalVariable (list);
			var arrayScope = new ArrayScope (list.Count,scope);
			var func = DynValue.FromFunction(new Function(name,parameters.Length, body, arrayScope));
			return scope.Put(name, func);
		}
		public override string ToString()
		{
			return string.Concat(" def ", name, "(", string.Join(", ", parameters), ")", body);
		}
		
	}
}
