using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST;
using Cygni.Lexical;
using System.IO;
using Cygni.Settings;
using Cygni.Lexical.Tokens;
using System.Text.RegularExpressions;
using Cygni.AST.Scopes;

namespace Cygni.Executors
{
	/// <summary>
	/// Description of InteractiveExecutor.
	/// </summary>
	public class InteractiveExecutor:Executor
	{
		LinkedList<string> list;
		Stack<Tag> stack;
		public InteractiveExecutor(BasicScope GlobalScope)
			: base(GlobalScope)
		{
			list = new LinkedList<string>();
			stack = new Stack<Tag>();
		}
		static readonly Regex re_exit = new Regex(@"[\s]*exit[\s]*");/* Support 'exit' command */
		public override DynValue Run()
		{
			DynValue Result = DynValue.Null;
			for (;;) {
				try {
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.Write("Cygni:  ");
					Console.ForegroundColor = ConsoleColor.Gray;
					Start:
					string line = Console.ReadLine();
					if(re_exit.IsMatch(line))
						break;
					list.AddLast(line);
					string code = string.Join("\n", list);
					var state = TryParse(code);
					if (state == InteractiveState.Error) {
						list.Clear();
						Console.ForegroundColor = ConsoleColor.Cyan;
						Console.Write("Cygni:  ");
						Console.ForegroundColor = ConsoleColor.Gray;
						goto Start;
					}
					if (state == InteractiveState.Waiting) {
						Console.ForegroundColor = ConsoleColor.Gray;
						Console.Write("....    ");
						goto Start;
					}
					list.Clear();
					using (var sr = new StringReader(code)) {
						var lexer = new Lexer(1, sr);
						var ast = new Parser(lexer);
						/*var a = ast.Program();
						Console.WriteLine(a);*/
						Result = ast.Program().Eval(GlobalScope);
						if (!GlobalSettings.Quiet && Result != DynValue.Null){
							Console.Write("=>  ");
							Console.WriteLine(Result);
						}
					}
				} catch (Exception ex) {
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("error:  {0}", ex.Message);
					//Console.WriteLine(ex);
				}
			}
			return Result;
		}
		InteractiveState TryParse(string code)
		{
			if (stack.Count > 0)
				stack.Clear();
			try {
				using (var sr = new StringReader(code)) {
					var lexer = new Lexer(1, sr);
					while (true) {
						var tok = lexer.Scan();
						switch (tok.tag) {
							case Tag.LeftParenthesis:
							case Tag.LeftBrace:
								stack.Push(tok.tag);
								break;
							case Tag.RightParenthesis:
								if (stack.Count == 0)
									return InteractiveState.Error;
								if (stack.Peek() == Tag.LeftParenthesis)
									stack.Pop();
								else
									return InteractiveState.Error;
								break;
							case Tag.RightBrace:
								if (stack.Count == 0)
									return InteractiveState.Error;
								if (stack.Peek() == Tag.LeftBrace)
									stack.Pop();
								else
									return InteractiveState.Error;
								break;
							case Tag.EOF:
								goto Finish;
						}
					}
					Finish:
					return stack.Count == 0 ? InteractiveState.Success : InteractiveState.Waiting;
				}
			} catch (Exception ex) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("error:  {0}", ex.Message);
				return InteractiveState.Error;
			}
		}
		enum InteractiveState
		{
			Waiting,
			Error,
			Success,
		}
	}
}
