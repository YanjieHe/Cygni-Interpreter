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
using Cygni.Errors;
using Mono.Terminal;

namespace Cygni.Loaders
{
    /// <summary>
    /// Description of InteractiveLoader.
    /// </summary>
    public class InteractiveLoader:Loader
    {
        readonly StringBuilder input;
        /* storage of code. */
        readonly Stack<Tag> stack;
        private DynValue result;

        public override DynValue Result
        {
            get
            {
                return this.result;
            }
        }

        public InteractiveLoader(IScope GlobalScope)
            : base(GlobalScope)
        {
            input = new StringBuilder();
            stack = new Stack<Tag>();
        }

        LineEditor editor = new LineEditor("Cygni");

        public override void Run()
        {
            Console.WriteLine("Cygni, version 0.7.0");
            Console.WriteLine("Copyright (C) 2017 Jason He.");
            this.result = DynValue.Nil;
            for (;;)
            {
                string prompt = "> ";
                try
                {
                    while (true)
                    {
                        string line = editor.Edit(prompt, "");
                        input.AppendLine(line);
                        string code = input.ToString();
                        //string code = ReadInput();
                        InteractiveState state = TryParse(code);

                        if (state == InteractiveState.Error)
                        {
                            input.Clear();
                            prompt = "> ";
                            continue;
                        }
                        else if (state == InteractiveState.Waiting)
                        {
                            prompt = ">> ";
                            continue;
                        }
                        else
                        {
                            input.Clear();

                            using (var sr = new StringReader(code))
                            {
                                var lexer = new Lexer(1, sr); 
                                /* In the interative mode, the lexer always starts at line 1. */
                                var ast = new Parser(lexer);
                                this.result = ast.Program().Eval(GlobalScope);
                                if (!GlobalSettings.Quiet && !Result.IsVoid && !Result.IsNil)
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write("=> ");
                                    Console.WriteLine(Result);
                                }
                            }
                            break;
                        }
                    }

                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        bool DoesPeekMatch(Tag tag)
        {
            return stack.Count != 0 && stack.Peek() == tag;
        }

        void WriteInCyan(string prompt)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(prompt);
            Console.ForegroundColor = ConsoleColor.White;
        }

        string ReadInput()
        {
            string line = Console.ReadLine();
            input.AppendLine(line);
            return input.ToString();
        }

        InteractiveState TryParse(string code)
        {
            if (stack.Count > 0)
            {
                stack.Clear();
            }
            try
            {
                using (var sr = new StringReader(code))
                {
                    var lexer = new Lexer(1, sr);
                    while (true)
                    {
                        Token tok = lexer.Scan();
                        switch (tok.tag)
                        {
                            case Tag.LeftParenthesis:
                            case Tag.LeftBrace:
                            case Tag.LeftBracket:
                                stack.Push(tok.tag); /* push '(' '[' '{' */
                                break;
                            case Tag.RightParenthesis:
                                if (DoesPeekMatch(Tag.LeftParenthesis))
                                {
                                    stack.Pop();
                                    break;
                                }
                                else
                                {
                                    throw SyntaxException.Unexpected(lexer.LineNumber, ")");
                                }
                            case Tag.RightBracket:
                                if (DoesPeekMatch(Tag.LeftBracket))
                                { 
                                    stack.Pop();
                                    break;
                                }
                                else
                                {
                                    throw SyntaxException.Unexpected(lexer.LineNumber, "]");
                                }
                            case Tag.RightBrace:
                                if (DoesPeekMatch(Tag.LeftBrace))
                                {
                                    stack.Pop();
                                    break;
                                }
                                else
                                {
                                    throw SyntaxException.Unexpected(lexer.LineNumber, "}");
                                }
                            case Tag.EOF:
                                goto Finish;
                        }
                    }
                    Finish:
                    return stack.Count == 0 ? InteractiveState.Success : InteractiveState.Waiting;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
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
