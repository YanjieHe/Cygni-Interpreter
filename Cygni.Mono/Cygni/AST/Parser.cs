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
		readonly Lexer lexer;
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
				throw new SyntaxException ("line {0}: Expecting '{1}'.", lexer.LineNumber, tag);
		}

		void MatchOrThrows (Tag tag, string message)
		{
			if (look.tag == tag) {
				Move ();
			} else {
				throw new SyntaxException ("line {0}: {1}.", lexer.LineNumber, message); 
			}
		}

		void Match (Tag tag1, Tag tag2)
		{
			if (look.tag == tag1 || look.tag == tag2)
				Move ();
			else
				throw new SyntaxException ("line {0}: Expecting '{1}' or '{2}.", lexer.LineNumber, tag1, tag2);
		}

		public BlockEx Program ()
		{
			var block = Block (matchBrackets: false);

			if (look.tag != Tag.EOF)
				throw SyntaxException.Expecting (lexer.LineNumber, "EOF");

			return block;
		}


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
					Match (Tag.Return);
					list.Add (ASTNode.Return (Statement ()));
					break;
				case Tag.Local:
					list.Add (Local ());
					break;
				case Tag.Global:
					list.Add (Global ());
					break;
				case Tag.Set:
					list.Add (Set ());
					break;
				case Tag.Unpack:
					list.Add (Unpack ());
					break;
				case Tag.EOF:
					if (matchBrackets)
						throw new SyntaxException ("line {0}: Missing '}'", lexer.LineNumber);
					goto Finish;
				case Tag.EOL:
					Move ();
					break;
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
			MatchOrThrows (Tag.ID, "for loop requires a iterator"); 
			MatchOrThrows (Tag.Assign, "for loop requires '=' after the iterator"); 
			var start = Bool ();
			MatchOrThrows (Tag.Comma, "for loop requires ',' after the initial value"); 
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
			MatchOrThrows (Tag.ID, "foreach loop requires a iterator"); 
			MatchOrThrows (Tag.In, "foreach loop requires 'in after the iterator"); 
			var collection = Bool ();
			var body = Block ();
			return ASTNode.ForEach (body, iterator, collection);
		}

		public ASTNode DefFunc ()
		{
			Match (Tag.Define);
			string name = look.ToString ();
			MatchOrThrows (Tag.ID, "function definition requires a function name"); 
			MatchOrThrows (Tag.LeftParenthesis, "function definition requires '(' after function name"); 
			var list = new List<NameEx> ();
			while (look.tag != Tag.RightParenthesis) {
				if (look.tag == Tag.ID) {
					list.Add (ASTNode.Parameter(look.ToString ()));
					Move ();
					if (look.tag == Tag.Comma)
						Move ();
					else if (look.tag == Tag.RightParenthesis)
						break;
					else
						throw new SyntaxException ("line {0}: function definition Expecting ',' or ')'", lexer.LineNumber);
				} else
					throw new SyntaxException ("line {0}: Wrong argument for function definition", lexer.LineNumber);
			}
			Move ();
			var body = Block ();
			NameEx[] parameters = new NameEx[list.Count];
			list.CopyTo(parameters);
			return ASTNode.Define (name, parameters, body);
		}

		public ASTNode DefClosure ()
		{
			Match (Tag.Lambda);
			var list = new List<NameEx> ();
			Match (Tag.LeftParenthesis);
			while (look.tag != Tag.RightParenthesis) {
				if (look.tag == Tag.ID) {
					list.Add (ASTNode.Variable (look.ToString ()));
					Match (Tag.ID);
					if (look.tag == Tag.Comma) {
						Move ();
					} else if (look.tag == Tag.RightParenthesis) {
						break;
					} else {
						throw new SyntaxException ("line {0}: lambda definition Expecting ',' or ')'", lexer.LineNumber);
					}
				} else {
					throw new SyntaxException ("line {0}: Wrong argument for lambda definition", lexer.LineNumber);
				}
			}
			Match (Tag.RightParenthesis);
			NameEx[] parameters = new NameEx[list.Count];
			list.CopyTo (parameters);
			if (look.tag == Tag.GoesTo) {
				Match (Tag.GoesTo);
				ASTNode statement = ASTNode.Return (Bool ());
				return ASTNode.DefineClosure (parameters, statement);
			} else {
				ASTNode body = Block ();
				return ASTNode.DefineClosure (parameters, body);
			}
		}

		public ASTNode DefClass ()
		{
			Match (Tag.Class);
			string name = look.ToString ();
			MatchOrThrows (Tag.ID, "class definition requires a class name"); 
			if (look.tag == Tag.Colon) { /* Inheritance */
				Move ();
				var parent = look.ToString ();
				MatchOrThrows (Tag.ID, "Missing parent class in the class definition"); 
				var body = Block ();
				return ASTNode.Class (name, body, parent);
			} else {
				var body = Block ();
				return ASTNode.Class (name, body);
			}
		}

		ASTNode Local ()
		{
			Match (Tag.Local);
			var names_list = new List<NameEx> ();
			var values_list = new List<ASTNode> ();
			do {
				string name = look.ToString ();
				Match (Tag.ID);
				names_list.Add (ASTNode.Variable (name));

				if (look.tag == Tag.Assign) {
					Match (Tag.Assign);
					values_list.Add (Bool ());
				} else {
					values_list.Add (ASTNode.Nil);
				}

				if (look.tag == Tag.Comma) {
					Match (Tag.Comma);
				} else {
					break;
				}
			} while (true);
			NameEx[] names = new NameEx[names_list.Count];
			ASTNode[] values = new ASTNode[values_list.Count];
			names_list.CopyTo (names);
			values_list.CopyTo (values);
			return ASTNode.Local (names, values);
		}

		ASTNode Global ()
		{
			Match (Tag.Global);
			var names_list = new List<string> ();
			do {
				string name = look.ToString ();
				Match (Tag.ID);
				names_list.Add (name);
				if (look.tag == Tag.Comma) {
					Match (Tag.Comma);
				} else {
					break;
				}
			} while (true);
			Match (Tag.Assign);

			var values_list = new List<ASTNode> ();
			do {
				ASTNode value = Bool ();
				values_list.Add (value);
				if (look.tag == Tag.Comma) {
					Match (Tag.Comma);
				} else {
					break;
				}
			} while (true);
			var names = new string[names_list.Count];
			var values = new ASTNode[values_list.Count];
			names_list.CopyTo (names);
			values_list.CopyTo (values);
			return ASTNode.Global (names, values);
		}

		ASTNode Set ()
		{
			Match (Tag.Set);
			List<ASTNode> targets_list = new List<ASTNode> ();
			do {
				targets_list.Add (Bool ());	
				if (look.tag == Tag.Comma) {
					Match (Tag.Comma);
				} else {
					break;
				}
			} while (true);

			Match (Tag.Assign);

			List<ASTNode> values_list = new List<ASTNode> ();
			do {
				values_list.Add (Bool ());	
				if (look.tag == Tag.Comma) {
					Match (Tag.Comma);
				} else {
					break;
				}
			} while (true);

			if (targets_list.Count != values_list.Count) {
				throw new SyntaxException ("line {0}: 'set' statement requires the number of targets is the same as the number of values.", lexer.LineNumber);
			}

			ASTNode[] targets = new ASTNode[targets_list.Count];
			targets_list.CopyTo (targets);

			ASTNode[] values = new ASTNode[values_list.Count];
			values_list.CopyTo (values);

			return ASTNode.Set (targets, values);
		}

		ASTNode Unpack ()
		{
			Match (Tag.Unpack);
			var items_list = new List<ASTNode> ();
			do {
				items_list.Add (Bool ());
				if (look.tag == Tag.Comma) {
					Match (Tag.Comma);
				} else {
					break;
				}
			} while (true);
			Match (Tag.Assign);
			ASTNode tuple = Bool ();
			ASTNode[] items = new ASTNode[items_list.Count];
			items_list.CopyTo (items);
			return ASTNode.Unpack (items, tuple);
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
						} else if (look.tag == Tag.RightParenthesis) {
							break;
						} else {						
							throw SyntaxException.Expecting (lexer.LineNumber, ")");
						}
					}
					Match (Tag.RightParenthesis);
					x = ASTNode.Invoke (x, list);

				} else if (tok.tag == Tag.LeftBracket) {
					var indexes = new List<ASTNode> ();
					while (look.tag != Tag.RightBracket) {
						indexes.Add (Bool ());
						if (look.tag == Tag.Comma) {
							Move ();
						} else if (look.tag == Tag.RightBracket) {
							break;
						} else {
							throw SyntaxException.Expecting (lexer.LineNumber, "]");
						}
					}
					Match (Tag.RightBracket);
					x = ASTNode.IndexAccess (x, indexes);

				} else { /* if (tok.tag == Tag.Dot) */
					var fieldName = look.ToString ();
					MatchOrThrows (Tag.ID, "Missing field");
					x = ASTNode.Dot (x, fieldName);
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
			case Tag.Nil:
				Move ();
				return ASTNode.Nil;
			case Tag.ID:
				string name = look.ToString ();
				x = ASTNode.Variable (name);
				Move ();
				return x;
			case Tag.LeftBracket:
				{
					Move ();
					var list = new List<ASTNode> ();
					while (look.tag != Tag.RightBracket) {
						list.Add (Bool ());
						if (look.tag == Tag.Comma) {
							Move ();
						} else if (look.tag == Tag.RightBracket) {
							break;
						} else {
							throw SyntaxException.Expecting (lexer.LineNumber, "]");
						}
					}
					Move ();
					ASTNode[] initializers = new ASTNode[list.Count];
					list.CopyTo (initializers);
					x = ASTNode.ListInit (initializers);
					return x;
				}
			case Tag.LeftBrace:
				{
					Move ();
					var list = new List<ASTNode> ();
					while (look.tag != Tag.RightBrace) {
						list.Add (Bool ());
						Match (Tag.Colon);
						list.Add (Bool ());
						if (look.tag == Tag.Comma) {
							Move ();
						} else if (look.tag == Tag.RightBrace) {
							break;
						} else {
							throw SyntaxException.Expecting (lexer.LineNumber, "}");
						}
					}
					Move ();
					ASTNode[] initializers = new ASTNode[list.Count];
					list.CopyTo (initializers);
					x = ASTNode.DictionaryInit (initializers);
					return x;
				}
			case Tag.Lambda:
				x = DefClosure ();
				return x;
			default:
				throw SyntaxException.Unexpected (lexer.LineNumber, look.ToString ());
			}
		}

	}
}
