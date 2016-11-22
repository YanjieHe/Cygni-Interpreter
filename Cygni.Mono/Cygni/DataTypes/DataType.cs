using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Cygni.DataTypes
{
	public enum DataType:byte
	{
		Number,
		Boolean,
		String,
		Function,
		NativeFunction,
	
		List,
		Dictionary,
		
		Struct,
		Class,
		
		Null,
		
		Break,
		Continue,
		Return,
		
		UserData,
	}
}
