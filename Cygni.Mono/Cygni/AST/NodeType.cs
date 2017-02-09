using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Cygni.AST
{
    /// <summary>
    /// Description of NodeType.
    /// </summary>
    public enum NodeType
    {
        Block,
        Constant,
        DefClass,
        DefFunc,
        DefClosure,
        Dot,
        If,
        Condition,
        Invoke,
        Name,
        Unary,
        While,
        For,
        Return,
        Command,
        ListInit,
        DictionaryInit,
        Index,
        SingleIndex,

        Assign,
        IndexAccess,
        SingleIndexAccess,
        MemberAccess,

        Local,
        Global,
        Unpack,
        Range,
        RightArrow,


        Add,
        Subtract,
        Multiply,
        Divide,
        IntDiv,
        Concatenate,
        Modulo,
        Power,

        And,
        Or,

        Equal,
        NotEqual,

        LessThan,
        GreaterThan,
        LessThanOrEqual,
        GreaterThanOrEqual,

        Plus,
        Minus,
        Not,
    }
}
