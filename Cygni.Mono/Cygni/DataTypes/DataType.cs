using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Cygni.DataTypes
{
	public enum DataType: byte
	{
		Number,
		Boolean,
		String,

		Function,
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
