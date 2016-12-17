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
		readonly LinkedList<string> list;
		/* storage of code. */
		readonly Stack<Tag> stack;

		public InteractiveExecutor (IScope GlobalScope)
			: base (GlobalScope)
		{
			list = new LinkedList<string> ();
			stack = new Stack<Tag> ();
		}

		private static readonly Regex re_exit = new Regex (@"^[\s]*exit[\s]*$");
		/* Support 'exit' command */

		public override DynValue Run ()
		{
			DynValue Result = DynValue.Nil;
			for (;;) {
				try {
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.Write ("Cygni> ");
					Console.ForegroundColor = ConsoleColor.White;
Start:
					string line = Console.ReadLine ();
					if (re_exit.IsMatch (line)){ 
						/* If user input 'exit', the interactive mode ends. */
						break;
					}
					list.AddLast (line);
					string code = string.Join ("\n", list);
					var state = TryParse (code);
					if (state == InteractiveState.Error) {
						list.Clear ();
						Console.ForegroundColor = ConsoleColor.Cyan;
						Console.Write ("Cygni> ");
						Console.ForegroundColor = ConsoleColor.White;
						goto Start;
					} else if (state == InteractiveState.Waiting) {
						Console.ForegroundColor = ConsoleColor.Cyan;
						Console.Write ("     -> ");
						Console.ForegroundColor = ConsoleColor.White;
						goto Start;
					} else {
						list.Clear ();
						using (var sr = new StringReader (code)) {
							var lexer = new Lexer (1, sr); 
							/* In the interative mode, the lexer always starts at line 1. */
							var ast = new Parser (lexer);
							Result = ast.Program ().Eval (GlobalScope);
							if (!GlobalSettings.Quiet && Result != DynValue.Nil) {
								Console.ForegroundColor = ConsoleColor.White;
								Console.Write ("=> ");
								Console.WriteLine (Result);
							}
						}
					}
				} catch (Exception ex) {
					Console.ForegroundColor = ConsoleColor.Red;
					if (GlobalSettings.CompleteErrorOutput)
						Console.WriteLine ("error: {0}", ex);
					else
						Console.WriteLine ("error: {0}", ex.Message);
				}
			}
			return Result;
		}

		InteractiveState TryParse (string code)
		{
			if (stack.Count > 0){
				stack.Clear ();
			}
			try {
				using (var sr = new StringReader (code)) {
					var lexer = new Lexer (1, sr);
					while (true) {
						var tok = lexer.Scan ();
						switch (tok.tag) {
							case Tag.LeftParenthesis:
							case Tag.LeftBrace:
							case Tag.LeftBracket:
								stack.Push (tok.tag); /* push '(' '[' '{' */
								break;
							case Tag.RightParenthesis:
								if (stack.Peek () == Tag.LeftParenthesis) { // get '(', match ')'
									stack.Pop ();
									break;
								}
								else {
									return InteractiveState.Error;
								}
							case Tag.RightBracket:
								if (stack.Peek() == Tag.LeftBracket) { // get '[', match ']'
									stack.Pop();
									break;
								} else {
									return InteractiveState.Error;
								}
							case Tag.RightBrace:
								if (stack.Peek () == Tag.LeftBrace) { // get '{', match '}'
									stack.Pop ();
									break;
								}
								else {
									return InteractiveState.Error;
								}
							case Tag.EOF:
								goto Finish;
						}
					}
Finish:
					return stack.Count == 0 ? InteractiveState.Success : InteractiveState.Waiting;
				}
			} catch (Exception ex) {
				Console.ForegroundColor = ConsoleColor.Red;
				if (GlobalSettings.CompleteErrorOutput)
					Console.WriteLine ("error: {0}", ex);
				else
					Console.WriteLine ("error: {0}", ex.Message);
				return InteractiveState.Error;
			}
		}

		private enum InteractiveState: byte
		{
			Waiting,
			Error,
			Success,
		}
	}
}
