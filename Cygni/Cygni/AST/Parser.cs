using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.Lexical.Tokens;
using Cygni.Lexical;
using System.IO;
using Cygni.Errors;
using Cygni.AST.Scopes;

namespace Cygni.AST
{
	/// <summary>
	/// Description of Parser.
	/// </summary>
	public class Parser
	{
		Lexer lexer;
		Token look;

		public Parser (Lexer lexer)
		{
			this.lexer = lexer;
			Move ();
		}

		void Move ()
		{
			look = lexer.Scan ();
		}

		void Match (Tag tag)
		{
			if (look.tag == tag)
				Move ();
			else
				throw new SyntaxException ("line {0}: Expecting '{1}'", lexer.LineNumber, tag);
		}

		void Match (Tag tag1, Tag tag2)
		{
			if (look.tag == tag1 || look.tag == tag2)
				Move ();
			else
				throw new SyntaxException ("line {0}: Expecting '{1}' or '{2}", lexer.LineNumber, tag1, tag2);
		}

		public BlockEx Program ()
		{
			var block = Block (false);
			if (look.tag != Tag.EOF)
				throw new Exception ();
			
			return block;
		}

		static readonly string[] commands = CommandEx.cmdDict.Keys.ToArray ();

		ASTNode Statement ()
		{
			ASTNode statement = Assign ();
			return statement;
		}

		BlockEx Block (bool matchBrackets = true)
		{
			var list = new List<ASTNode> ();
			if (matchBrackets)
				Match (Tag.LeftBrace);
			while (look.tag != Tag.RightBrace) {
				switch (look.tag) {
				case Tag.If:
					list.Add (If ());
					break;
				case Tag.While:
					list.Add (While ());
					break;
				case Tag.For:
					list.Add (For ());
					break;
				case Tag.ForEach:
					list.Add (ForEach ());
					break;
				case Tag.Define:
					list.Add (DefFunc ());
					break;
				case Tag.Class:
					list.Add (DefClass ());
					break;
				case Tag.Return:
					Move ();
					list.Add (ASTNode.Return (Statement ()));
					break;
				case Tag.EOF:
					if (matchBrackets)
						throw new SyntaxException ("line {0}: Missing '}'", lexer.LineNumber);
					goto Finish;
				case Tag.EOL:
					Move ();
					break;
				case Tag.ID:
					if (commands.Contains (look.ToString ())) {
						list.Add (Command ());
						break;
					} else
						goto default;
				default:
					list.Add (Statement ());
					break;
				}
			}
			Move ();
			Finish:
			return ASTNode.Block (list);
		}

		ASTNode If ()
		{
			Match (Tag.If, Tag.ElseIf);
			ASTNode c = Bool ();
			BlockEx body = Block ();
			if (look.tag == Tag.Else) {
				Move ();
				return ASTNode.IfThenElse (c, body, Block ());
			}
			if (look.tag == Tag.ElseIf) {
				return ASTNode.IfThenElse (c, body, If ());
			}
			return ASTNode.IfThen (c, body);
		}

		ASTNode While ()
		{
			Match (Tag.While);
			ASTNode c = Bool ();
			BlockEx body = Block ();
			return ASTNode.While (c, body);
		}

		ASTNode For ()
		{
			Match (Tag.For);
			var iterator = look.ToString ();
			Match (Tag.ID);
			Match (Tag.Assign);
			var start = Bool ();
			Match (Tag.Comma);
			var end = Bool ();
			if (look.tag == Tag.Comma) {
				Move ();
				var step = Bool ();
				var body = Block ();
				return ASTNode.For (body, iterator, start, end, step);
			} else {
				var body = Block ();
				return ASTNode.For (body, iterator, start, end);
			}
		}
		ASTNode ForEach ()
		{
			Match (Tag.ForEach);
			var iterator = look.ToString ();
			Match (Tag.ID);
			Match (Tag.In);
			var collection = Bool ();
			var body = Block ();
			return ASTNode.ForEach (body, iterator, collection);
		}
		
		public ASTNode DefFunc ()
		{
			Match (Tag.Define);
			string name = look.ToString ();
			Match (Tag.ID);
			Match (Tag.LeftParenthesis);
			var list = new List<string> ();
			while (look.tag != Tag.RightParenthesis) {
				if (look.tag == Tag.ID) {
					list.Add (look.ToString ());
					Move ();
					if (look.tag == Tag.Comma)
						Move ();
					else if (look.tag == Tag.RightParenthesis)
						break;
					else
						throw new SyntaxException ("line {0}: Expecting ',' or ')'", lexer.LineNumber);
				} else
					throw new SyntaxException ("line {0}: Wrong arguments definition", lexer.LineNumber);
			}
			Move ();
			var body = Block ();
			return ASTNode.Define (name, list.ToArray (), body);
		}

		public ASTNode DefClass ()
		{
			Match (Tag.Class);
			string name = look.ToString ();
			Match (Tag.ID);
			if (look.tag == Tag.Colon) { /* Inheritance */
				Move ();
				var parents = new List<string> ();
				while (true) {
					if (look.tag == Tag.ID) {
						parents.Add (look.ToString ());
						Move ();
						if (look.tag == Tag.Comma) {
							Move ();
							continue;
						}
						if (look.tag == Tag.LeftBrace)
							break;
					}
					throw new SyntaxException ("line {0}: Wrong parent classes for class '{1}'", lexer.LineNumber, name);
				}
				var body = Block ();
				return ASTNode.Class (name, body, parents.ToArray ());
			} else {
				var body = Block ();
				return ASTNode.Class (name, body);
			}
		}

		ASTNode Command ()
		{
			var name = look.ToString ();
			Match (Tag.ID);
			if (CommandEx.NonArgCmd.Contains(name)) {
				return ASTNode.Command (name, new ASTNode[0]);
			}
			var list = new List<ASTNode> ();

			do {
				list.Add (Bool ());
				if (look.tag == Tag.Comma) {
					Move ();
				} else
					break;
			} while (look.tag != Tag.EOF);
			return ASTNode.Command (name, list);
		}

		ASTNode Assign ()
		{
			ASTNode x = Bool ();
			if (look.tag == Tag.Assign) {
				Move ();
				x = ASTNode.Assign (x, Assign ());
				return x;
			}
			return x;
		}

		ASTNode Bool ()
		{
			ASTNode x = Join ();
			while (look.tag == Tag.Or) {
				Move ();
				x = ASTNode.Or (x, Join ());
			}
			return x;
		}

		ASTNode Join ()
		{
			ASTNode x = Equality ();
			while (look.tag == Tag.And) {
				Move ();
				x = ASTNode.And (x, Equality ());
			}
			return x;
		}

		ASTNode Equality ()
		{
			ASTNode x = Relation ();
			while (look.tag == Tag.Equal || look.tag == Tag.NotEqual) {
				Token tok = look;
				Move ();
				if (tok.tag == Tag.Equal)
					x = ASTNode.Equal (x, Relation ());
				else
					x = ASTNode.NotEqual (x, Relation ());
			}
			return x;
		}

		ASTNode Relation ()
		{
			ASTNode x = Expr ();
			switch (look.tag) {
			case Tag.Less:
				Move ();
				return ASTNode.Less (x, Expr ());
			case Tag.Greater:
				Move ();
				return ASTNode.Greater (x, Expr ());
			case Tag.LessOrEqual:
				Move ();
				return ASTNode.LessOrEqual (x, Expr ());
			case Tag.GreaterOrEqual:
				Move ();
				return ASTNode.GreaterOrEqual (x, Expr ());
			default:
				return x;
			}
		}

		ASTNode Expr ()
		{
			ASTNode x = Term ();
			;
			while (look.tag == Tag.Add || look.tag == Tag.Sub) {
				Token tok = look;
				Move ();
				if (tok.tag == Tag.Add)
					x = ASTNode.Add (x, Term ());
				else
					x = ASTNode.Subtract (x, Term ());
			}
			return x;
		}

		ASTNode Term ()
		{
			ASTNode x = Power ();
			while (look.tag == Tag.Mul || look.tag == Tag.Div || look.tag == Tag.Mod) {
				Token tok = look;
				Move ();
				if (tok.tag == Tag.Mul)
					x = ASTNode.Multiply (x, Power ());
				else if (tok.tag == Tag.Div)
					x = ASTNode.Divide (x, Power ());
				else
					x = ASTNode.Modulo (x, Power ());
			}
			return x;
		}

		ASTNode Power ()
		{
			ASTNode x = Unary ();
			while (look.tag == Tag.Pow) {
				Move ();
				x = ASTNode.Power (x, Unary ());
			}
			return x;
		}

		ASTNode Unary ()
		{
			switch (look.tag) {
			case Tag.Add:
				Move ();
				return ASTNode.UnaryPlus (Unary ());
			case Tag.Sub:
				Move ();
				return ASTNode.UnaryMinus (Unary ());
			case Tag.Not:
				Move ();
				return ASTNode.Negate (Unary ());
			default:
				return Postfix ();
			}
		}

		ASTNode Postfix ()
		{
			ASTNode x = Factor ();
			
			while (look.tag == Tag.LeftParenthesis || look.tag == Tag.LeftBracket || look.tag == Tag.Dot) {
			
				Token tok = look;
				Move ();
				
				if (tok.tag == Tag.LeftParenthesis) {
					var list = new List<ASTNode> ();
					while (look.tag != Tag.RightParenthesis) {
						list.Add (Bool ());
						if (look.tag == Tag.Comma) {
							Move ();
							continue;
						}
						if (look.tag == Tag.RightParenthesis)
							break;
						
						throw new SyntaxException ("line {0}: Unexpected '{1}'", lexer.LineNumber, look);
					}
					Move ();
					//return ASTNode.Invoke (x, list);
					x = ASTNode.Invoke (x, list);
					continue;
				}
			
			
				
				if (tok.tag == Tag.LeftBracket) {
					var indexes = new List<ASTNode> ();
					while (look.tag != Tag.RightBracket) {
						indexes.Add (Bool ());
						if (look.tag == Tag.Comma) {
							Move ();
							continue;
						}
						if (look.tag == Tag.RightBracket)
							break;
						
						throw new SyntaxException ("line {0}: Unexpected '{1}'", lexer.LineNumber, look);
					}
					Move ();
					x = ASTNode.Index (x, indexes);
					continue;
				}
			
				if (tok.tag == Tag.Dot) {
					var fieldname = look.ToString ();
					Match (Tag.ID);
					x = ASTNode.Dot (x, fieldname);
					continue;
				}
			
			}
			return x;
		}

		ASTNode Factor ()
		{
			ASTNode x = null;
			switch (look.tag) {
			case Tag.LeftParenthesis:
				Move ();
				x = Bool ();
				Match (Tag.RightParenthesis);
				return x;
			case Tag.Number:
				x = ASTNode.Number ((look as NumToken).Value);
				Move ();
				return x;
			case Tag.True:
				x = ASTNode.True;
				Move ();
				return x;
			case Tag.False:
				x = ASTNode.False;
				Move ();
				return x;
			case Tag.String:
				x = ASTNode.String ((look as StrToken).Literal);
				Move ();
				return x;
			case Tag.Break:
				Move ();
				return ASTNode.Break;
			case Tag.Continue:
				Move ();
				return ASTNode.Continue;
			case Tag.Null:
				Move ();
				return ASTNode.Null;
			case Tag.ID:
				string name = look.ToString ();
				x = ASTNode.Variable (name);
				Move ();
				return x;
			case Tag.LeftBracket:
				Move ();
				var list = new List<ASTNode> ();
				while (look.tag != Tag.RightBracket) {
					list.Add (Bool ());
					if (look.tag == Tag.Comma) {
						Move ();
						continue;
					}
					if (look.tag == Tag.RightBracket)
						break;
						
					throw new SyntaxException ("line {0}: Unexpected '{1}'", lexer.LineNumber, look);
				}
				Move ();
				x = ASTNode.ListInit (list);
				return x;
			default:
				throw new SyntaxException ("line {0}: Unexpected '{1}'", lexer.LineNumber, look);
			}
		}

	}
}
