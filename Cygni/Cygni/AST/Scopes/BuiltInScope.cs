using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.Errors;
using System.Collections.Generic;
using System.Linq;
namespace Cygni.AST.Scopes
{
	public class BuiltInScope :BasicScope
	{
		public BuiltInScope ():base()
		{
		}
		public override DynValue Get (string name)
		{
			DynValue _value;
			if (envTable.TryGetValue (name, out _value))
				return _value;
			throw RuntimeException.NotDefined (name);
		}
		public override  DynValue Put (string name, DynValue value){
			throw new NotSupportedException (
			"Built-In Scope is read-only");
		}
		public void BuiltIn(string name, DynValue value){
			this.envTable[name] = value;
		}
		public void BuiltIn(string name, Structure value){
			this.envTable[name] = DynValue.FromStructure(value);
		}
		public void BuiltIn(string name, Func<DynValue[],DynValue> f){
			this.envTable [name] = DynValue.FromNativeFunction (new NativeFunction (f));
		}

	}
}

