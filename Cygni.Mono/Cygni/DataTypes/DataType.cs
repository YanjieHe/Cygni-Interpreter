using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Cygni.DataTypes
{
	public enum DataType: byte
	{
		Integer,
		Number,
		Boolean,
		String,

		Function,
		Closure,
		NativeFunction,
		Command,
	
		List,
		Dictionary,
		Tuple,

		Struct,
		Class,
		
		Nil,
		
		Break,
		Continue,
		Return,

		UserData,
	}
}
