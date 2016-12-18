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
		private DynValue result;
		public override DynValue Result {
			get {
				return this.result;
			}
		}
		public InteractiveExecutor (IScope GlobalScope)
			: base (GlobalScope)
		{
			list = new LinkedList<string> ();
			stack = new Stack<Tag> ();
		}

		public override void Run ()
		{
			this.result = DynValue.Nil;
			for (;;) {
				try {
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.Write ("Cygni> ");
					Console.ForegroundColor = ConsoleColor.White;
Start:
					string line = Console.ReadLine ();
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
							this.result = ast.Program ().Eval (GlobalScope);
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
