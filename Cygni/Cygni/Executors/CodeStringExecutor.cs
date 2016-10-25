using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.AST;
using Cygni.DataTypes;
using System.IO;
using Cygni.Lexical;
using Cygni.AST.Scopes;

namespace Cygni.Executors
{
	/// <summary>
	/// Description of CodeStringExecutor.
	/// </summary>
	public class CodeStringExecutor:Executor
	{
		string Code;
		public CodeStringExecutor(BasicScope GlobalScope, string Code)
			: base(GlobalScope)
		{
			this.Code = Code;
		}

		#region implemented abstract members of Executor

		public override DynValue Run()
		{
			DynValue Result = DynValue.Null;
			try {
				using (var sr = new StringReader(Code)) {
					var lexer = new Lexer(1, sr);
					var ast = new Parser(lexer);
					Result = ast.Program().Eval(GlobalScope);
				}
			} catch (Exception ex) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex.Message);
				//Console.WriteLine(ex);

			}
			return Result;
		}

		#endregion
	}
}
