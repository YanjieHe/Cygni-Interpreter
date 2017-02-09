using System;

namespace Cygni.AST.Interfaces
{
    public interface IArgumentProvider
    {
        int ArgumentCount { get; }

        ASTNode GetArgument(int index);
    }
}

