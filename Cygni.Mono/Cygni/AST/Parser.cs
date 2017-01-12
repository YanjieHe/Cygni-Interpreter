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

        public Parser(Lexer lexer)
        {
            this.lexer = lexer;
            Move();
        }

        void Move()
        {
            look = lexer.Scan();
        }

        SyntaxException Error(string message, params object[] objs)
        {
            return new SyntaxException("line {0}: {1}.", lexer.LineNumber, string.Format(message, objs));
        }

        void Match(Tag tag)
        {
            if (look.tag == tag)
            {
                Move();
            }
            else
            {
                throw new SyntaxException("line {0}: Expecting '{1}'.", lexer.LineNumber, tag);
            }
        }

        void MatchOrThrows(Tag tag, string message)
        {
            if (look.tag == tag)
            {
                Move();
            }
            else
            {
                throw new SyntaxException("line {0}: {1}.", lexer.LineNumber, message); 
            }
        }

        public BlockEx Program()
        {
            BlockEx block = Block(matchBrackets: false);

            if (look.tag != Tag.EOF)
            {
                throw SyntaxException.Expecting(lexer.LineNumber, "EOF");
            }
            else
            {
                return block;
            }
        }

        BlockEx Block(bool matchBrackets = true)
        {
            var list = new List<ASTNode>();
            if (matchBrackets)
            {
                Match(Tag.LeftBrace);
            }
            while (look.tag != Tag.RightBrace)
            {
                switch (look.tag)
                {
                    case Tag.If:
                        list.Add(If());
                        break;
                    case Tag.While:
                        list.Add(While());
                        break;
                    case Tag.For:
                        list.Add(For());
                        break;
                    case Tag.Define:
                        list.Add(DefFunc());
                        break;
                    case Tag.Class:
                        list.Add(DefClass());
                        break;
                    case Tag.Return:
                        Match(Tag.Return);
                        list.Add(ASTNode.Return(Statement()));
                        break;
                    case Tag.Var:
                        list.Add(DefineLocalVariable());
                        break;
                    case Tag.EOF:
                        if (matchBrackets)
                        {
                            throw new SyntaxException("line {0}: Missing '}'", lexer.LineNumber);
                        }
                        else
                        {
                            return ASTNode.Block(list.ToArray());
                        }
                    case Tag.EOL:
                        Move();
                        break;
                    default:
                        list.Add(Statement());
                        break;
                }
            }
            Match(Tag.RightBrace);
            return ASTNode.Block(list.ToArray());
        }

        ASTNode Statement()
        {
            ASTNode statement = Assign();
            return statement;
        }

        ASTNode If()
        {
            Match(Tag.If);
            ASTNode test = Bool();
            BlockEx body = Block();
            if (look.tag == Tag.Else)
            {
                Move();
                return ASTNode.IfThenElse(test, body, Block());
            }
            else if (look.tag == Tag.ElseIf)
            {
                return Conditions(test, body);
            }
            else
            {
                return ASTNode.IfThen(test, body);
            }
        }

        ASTNode Conditions(ASTNode IfTrue, BlockEx TrueBody)
        {
            var conditions_list = new List<ASTNode>();
            var bodys_list = new List<BlockEx>();
            while (look.tag == Tag.ElseIf || look.tag == Tag.Else)
            {
                if (look.tag == Tag.ElseIf)
                {
                    Move();
                    conditions_list.Add(Bool());
                    bodys_list.Add(Block());
                    return ASTNode.Conditions(conditions_list.ToArray(), bodys_list.ToArray());
                }
                else if (look.tag == Tag.Else)
                {
                    Move();
                    return ASTNode.Conditions(
                        conditions_list.ToArray(), bodys_list.ToArray(), Block());
                }
            }
            return ASTNode.Conditions(conditions_list.ToArray(), bodys_list.ToArray());
        }

        ASTNode While()
        {
            Match(Tag.While);
            ASTNode condition = Bool();
            BlockEx body = Block();
            return ASTNode.While(condition, body);
        }

        ASTNode For()
        {
            Match(Tag.For);
            NameEx iterator = ASTNode.Variable(look.ToString());
            MatchOrThrows(Tag.ID, "for loop requires a iterator"); 
            MatchOrThrows(Tag.In, "for loop requires 'in' after the iterator");
            ASTNode collection = Range();
            BlockEx body = Block();
            return ASTNode.For(body, iterator, collection);
        }

        public ASTNode DefFunc()
        {
            Match(Tag.Define);
            string name = look.ToString();
            MatchOrThrows(Tag.ID, "function definition requires a function name"); 
            MatchOrThrows(Tag.LeftParenthesis, "function definition requires '(' after function name"); 
            var list = new List<NameEx>();

            while (look.tag != Tag.RightParenthesis)
            {
                if (look.tag == Tag.ID)
                {
                    list.Add(ASTNode.Parameter(look.ToString()));
                    Move();
                    if (look.tag == Tag.Comma)
                    {
                        Move();
                    }
                    else if (look.tag == Tag.RightParenthesis)
                    {
                        break;
                    }
                    else
                    {
                        throw new SyntaxException("line {0}: function definition Expecting ',' or ')'", lexer.LineNumber);
                    }
                }
                else
                    throw new SyntaxException("line {0}: Wrong argument for function definition", lexer.LineNumber);
            }
            Match(Tag.RightParenthesis);
            BlockEx body = Block();
            NameEx[] parameters = new NameEx[list.Count];
            list.CopyTo(parameters);
            return ASTNode.Define(name, parameters, body);
        }


        public ASTNode DefClass()
        {
            Match(Tag.Class);
            string name = look.ToString();
            MatchOrThrows(Tag.ID, "class definition requires a class name"); 
            if (look.tag == Tag.Colon)
            { /* Inheritance */
                Move();
                NameEx parent = ASTNode.Variable(look.ToString());
                MatchOrThrows(Tag.ID, "Missing parent class in the class definition"); 
                BlockEx body = Block();
                return ASTNode.DefineClass(name, body, parent);
            }
            else
            {
                BlockEx body = Block();
                return ASTNode.DefineClass(name, body);
            }
        }

        ASTNode DefineLocalVariable()
        {
            Match(Tag.Var);
            var names_list = new List<NameEx>();
            var values_list = new List<ASTNode>();

            do
            {
                string name = look.ToString();
                Match(Tag.ID);
                names_list.Add(ASTNode.Variable(name));

                if (look.tag == Tag.Assign)
                {
                    Move();
                    values_list.Add(Range());
                }
                else
                {
                    values_list.Add(ASTNode.Nil);
                }

                if (look.tag == Tag.Comma)
                {
                    Move();
                }
                else
                {
                    break;
                }
            } while (true);

            NameEx[] names = new NameEx[names_list.Count];
            ASTNode[] values = new ASTNode[values_list.Count];
            names_list.CopyTo(names);
            values_list.CopyTo(values);
            return ASTNode.DefineVariable(names, values);
        }

        ASTNode Assign()
        {
            ASTNode x = Range();
            if (look.tag == Tag.Assign)
            {
                Move();
                x = ASTNode.Assign(x, Assign());
                return x;
            }
            return x;
        }

        ASTNode Range()
        {
            ASTNode x = Bool();
            if (look.tag == Tag.Colon)
            {
                Move();
                ASTNode start = x;
                ASTNode end = Bool();
                if (look.tag == Tag.Colon)
                {
                    Move();
                    ASTNode step = Bool();
                    return ASTNode.RangeInit(start, end, step);
                }
                else
                {
                    return ASTNode.RangeInit(start, end);
                }
            }
            return x;
        }

        ASTNode Bool()
        {
            ASTNode x = Join();
            while (look.tag == Tag.Or)
            {
                Move();
                x = ASTNode.Or(x, Join());
            }
            return x;
        }

        ASTNode Join()
        {
            ASTNode x = Equality();
            while (look.tag == Tag.And)
            {
                Move();
                x = ASTNode.And(x, Equality());
            }
            return x;
        }

        ASTNode Equality()
        {
            ASTNode x = Relation();
            while (look.tag == Tag.Equal || look.tag == Tag.NotEqual)
            {
                Token tok = look;
                Move();
                if (tok.tag == Tag.Equal)
                {
                    x = ASTNode.Equal(x, Relation());
                }
                else
                {
                    x = ASTNode.NotEqual(x, Relation());
                }
            }
            return x;
        }

        ASTNode Relation()
        {
            ASTNode x = Concatenation();
            if (
                look.tag == Tag.Less ||
                look.tag == Tag.Greater ||
                look.tag == Tag.LessOrEqual ||
                look.tag == Tag.GreaterOrEqual)
            {
                Token tok = look;
                Move();
                switch (tok.tag)
                {
                    case Tag.Less:
                        return ASTNode.Less(x, Concatenation());
                    case Tag.Greater:
                        return ASTNode.Greater(x, Concatenation());
                    case Tag.LessOrEqual:
                        return ASTNode.LessOrEqual(x, Concatenation());
                    default: /* Tag.GreaterOrEqual */
                        return ASTNode.GreaterOrEqual(x, Concatenation());
                }
            }
            else
            {
                return x;
            }
        }

        ASTNode Concatenation()
        {
            ASTNode x = Expr();
            while (look.tag == Tag.Concatenate)
            {
                Move();
                x = ASTNode.Concatenate(x, Expr());
            }
            return x;
        }

        ASTNode Expr()
        {
            ASTNode x = Term();
            while (look.tag == Tag.Add || look.tag == Tag.Sub)
            {
                Token tok = look;
                Move();
                if (tok.tag == Tag.Add)
                {
                    x = ASTNode.Add(x, Term());
                }
                else
                {
                    x = ASTNode.Subtract(x, Term());
                }
            }
            return x;
        }

        ASTNode Term()
        {
            ASTNode x = Unary();
            while (look.tag == Tag.Mul ||
                   look.tag == Tag.Div ||
                   look.tag == Tag.IntDiv ||
                   look.tag == Tag.Mod)
            {
                Token tok = look;
                Move();
                if (tok.tag == Tag.Mul)
                {
                    x = ASTNode.Multiply(x, Unary());
                }
                else if (tok.tag == Tag.Div)
                {
                    x = ASTNode.Divide(x, Unary());
                }
                else if (tok.tag == Tag.IntDiv)
                {
                    x = ASTNode.IntDivide(x, Unary());
                }
                else
                {
                    x = ASTNode.Modulo(x, Unary());
                }
            }
            return x;
        }

        ASTNode Unary()
        {
            if (look.tag == Tag.Add ||
                look.tag == Tag.Sub ||
                look.tag == Tag.Not)
            {
                Token tok = look;
                Move();
                switch (tok.tag)
                {
                    case Tag.Add:
                        return ASTNode.UnaryPlus(Unary());
                    case Tag.Sub:
                        return ASTNode.UnaryMinus(Unary());
                    default : /* Tag.Not: */
                        return ASTNode.Negate(Unary());
                }
            }
            else
            {
                return Power();
            }
        }

        ASTNode Power()
        {
            ASTNode x = Postfix();
            while (look.tag == Tag.Pow)
            {
                Move();
                x = ASTNode.Power(x, Unary());
            }
            return x;
        }

        ASTNode Postfix()
        {
            ASTNode x = Factor();

            while (look.tag == Tag.LeftParenthesis ||
                   look.tag == Tag.LeftBracket ||
                   look.tag == Tag.Dot)
            {

                Token tok = look;
                Move();

                if (tok.tag == Tag.LeftParenthesis)
                {
                    x = Invoke(x);
                }
                else if (tok.tag == Tag.LeftBracket)
                {
                    x = Index(x);
                }
                else
                { /* Tag.Dot */
                    string fieldName = look.ToString();
                    MatchOrThrows(Tag.ID, "Missing field");
                    x = ASTNode.Dot(x, fieldName);
                }

            }
            return x;
        }

        ASTNode Factor()
        {
            Token tok = look;
            Move();
            switch (tok.tag)
            {
                case Tag.LeftParenthesis:
                    return Parentheses();
                case Tag.Integer:
                    return ASTNode.Integer((tok as IntToken).Value);
                case Tag.Number:
                    return ASTNode.Number((tok as NumToken).Value);
                case Tag.True:
                    return ASTNode.True;
                case Tag.False:
                    return ASTNode.False;
                case Tag.String:
                    return ASTNode.String((tok as StrToken).Literal);
                case Tag.Break:
                    return ASTNode.Break;
                case Tag.Continue:
                    return ASTNode.Continue;
                case Tag.Nil:
                    return ASTNode.Nil;
                case Tag.ID:
                    return ASTNode.Variable(tok.ToString());
                case Tag.LeftBracket:
                    return ListInit();
                case Tag.LeftBrace:
                    return DictionaryInit();
                case Tag.Lambda:
                    return Closure();
                default:
                    throw SyntaxException.Unexpected(lexer.LineNumber, tok.ToString());
            }
        }

        ASTNode Invoke(ASTNode function)
        {
            var arguments = new List<ASTNode>();
            while (look.tag != Tag.RightParenthesis)
            {
                arguments.Add(Range());
                if (look.tag == Tag.Comma)
                {
                    Move();
                }
                else if (look.tag == Tag.RightParenthesis)
                {
                    break;
                }
                else
                {                       
                    throw SyntaxException.Expecting(lexer.LineNumber, ")");
                }
            }
            Match(Tag.RightParenthesis);
            return ASTNode.Invoke(function, arguments);
        }

        ASTNode Index(ASTNode collection)
        {
            var indexes = new List<ASTNode>();
            while (look.tag != Tag.RightBracket)
            {
                indexes.Add(Range());
                if (look.tag == Tag.Comma)
                {
                    Move();
                }
                else if (look.tag == Tag.RightBracket)
                {
                    break;
                }
                else
                {
                    throw SyntaxException.Expecting(lexer.LineNumber, "]");
                }
            }
            Match(Tag.RightBracket);
            return ASTNode.IndexAccess(collection, indexes);
        }

        ASTNode Parentheses()
        {
            ASTNode value = Range();
            Match(Tag.RightParenthesis);
            return value;
        }

        ASTNode ListInit()
        {
            var list = new List<ASTNode>();
            while (look.tag != Tag.RightBracket)
            {
                list.Add(Range());
                if (look.tag == Tag.Comma)
                {
                    Move();
                }
                else if (look.tag == Tag.RightBracket)
                {
                    break;
                }
                else
                {
                    throw SyntaxException.Expecting(lexer.LineNumber, "]");
                }
            }
            Match(Tag.RightBracket);
            ASTNode[] initializers = new ASTNode[list.Count];
            list.CopyTo(initializers);
            return ASTNode.ListInit(initializers);
        }

        ASTNode DictionaryInit()
        {
            var list = new List<ASTNode>();
            while (look.tag != Tag.RightBrace)
            {
                list.Add(Bool());
                Match(Tag.Colon);
                list.Add(Range());
                if (look.tag == Tag.Comma)
                {
                    Move();
                }
                else if (look.tag == Tag.RightBrace)
                {
                    break;
                }
                else
                {
                    throw SyntaxException.Expecting(lexer.LineNumber, "}");
                }
            }
            Match(Tag.RightBrace);
            ASTNode[] initializers = new ASTNode[list.Count];
            list.CopyTo(initializers);
            return ASTNode.DictionaryInit(initializers);
        }

        ASTNode Closure()
        {
            var list = new List<NameEx>();
            Match(Tag.LeftParenthesis);
            while (look.tag != Tag.RightParenthesis)
            {
                if (look.tag == Tag.ID)
                {
                    list.Add(ASTNode.Variable(look.ToString()));
                    Move();
                    if (look.tag == Tag.Comma)
                    {
                        Move();
                    }
                    else if (look.tag == Tag.RightParenthesis)
                    {
                        break;
                    }
                    else
                    {
                        throw new SyntaxException("line {0}: lambda definition Expecting ',' or ')'", lexer.LineNumber);
                    }
                }
                else
                {
                    throw new SyntaxException("line {0}: Wrong argument for lambda definition", lexer.LineNumber);
                }
            }
            Match(Tag.RightParenthesis);
            NameEx[] parameters = new NameEx[list.Count];
            list.CopyTo(parameters);
            if (look.tag == Tag.GoesTo)
            {
                Move();
                ASTNode statement = Range();
                return ASTNode.DefineClosure(parameters, statement);
            }
            else
            {
                ASTNode body = Block();
                return ASTNode.DefineClosure(parameters, body);
            }
        }

    }
}
