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
using Cygni.Settings;
namespace Cygni.Executors
{
	/// <summary>
	/// Description of CodeStringExecutor.
	/// </summary>
	public class CodeStringExecutor:Executor
	{
		readonly string Code;
		private DynValue result;
		public override DynValue Result {
			get {
				return this.result;
			}
		}
		public CodeStringExecutor(IScope GlobalScope, string Code)
			: base(GlobalScope)
		{
			this.Code = Code;
		}

		#region implemented abstract members of Executor

		public override void Run()
		{
			this.result = DynValue.Nil;
			try {
				using (var sr = new StringReader(Code)) {
					var lexer = new Lexer(1, sr);
					var ast = new Parser(lexer);
					this.result = ast.Program().Eval(GlobalScope);
				}
			} catch (Exception ex) {
				Console.ForegroundColor = ConsoleColor.Red;
				if (GlobalSettings.CompleteErrorOutput)
					Console.WriteLine ("error: {0}", ex);
				else
					Console.WriteLine ("error: {0}", ex.Message);

			}
		}

		#endregion
	}
}
