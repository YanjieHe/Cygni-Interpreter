using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST;
using System.IO;
using Cygni.AST.Scopes;
using Cygni.Settings;
using Cygni.Lexical;

namespace Cygni.Executors
{
	/// <summary>
	/// Description of CodeFileExecutor.
	/// </summary>
	public class CodeFileExecutor:Executor
	{
		string FilePath;
		Encoding encoding;
		private DynValue result;

		public override DynValue Result {
			get {
				return this.result;
			}
		}

		public CodeFileExecutor (IScope GlobalScope, string FilePath, Encoding encoding)
			: base (GlobalScope)
		{
			this.FilePath = FilePath;
			this.encoding = encoding;
		}

		#region implemented abstract members of Executor

		public override void Run ()
		{
			this.result = DynValue.Nil;
			try {
				using (var sr = new StreamReader (FilePath, encoding)) {
					var lexer = new Lexer (1, sr);
					var ast = new Parser (lexer);
					this.result = ast.Program ().Eval (GlobalScope);
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

		public BlockEx Load ()
		{
			using (var sr = new StreamReader (FilePath, encoding)) {
				var lexer = new Lexer (1, sr);
				var ast = new Parser (lexer);
				return ast.Program ();
			}
		}
	}
}
