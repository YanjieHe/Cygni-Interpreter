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
using Cygni.AST.Visitors;
using System.Linq.Expressions;
namespace Cygni.Executors
{
	public class CompilerExecutor:Executor
	{
		string Code;
		public CompilerExecutor(BasicScope GlobalScope, string Code)
			: base(GlobalScope)
		{
			this.Code = Code;
		}

		#region implemented abstract members of Executor

		public override DynValue Run()
		{
			DynValue Result = DynValue.Nil;
			try {
				using (var sr = new StringReader(Code)) {
					var lexer = new Lexer(1, sr);
					var ast = new Parser(lexer);
					BlockEx program = ast.Program();
					CompilerVisitor visitor = new CompilerVisitor(GlobalScope);
					Expression runner = visitor.Load(program);
					Func<DynValue> f = Expression.Lambda<Func<DynValue>>(runner).Compile();
					Console.WriteLine("result = {0}",f());
					//Result = ast.Program().Eval(GlobalScope);
				}
			} catch (Exception ex) {
				Console.ForegroundColor = ConsoleColor.Red;
				if (GlobalSettings.CompleteErrorOutput)
					Console.WriteLine ("error: {0}", ex);
				else
					Console.WriteLine ("error: {0}", ex.Message);

			}
			return Result;
		}

		#endregion
	}
}

