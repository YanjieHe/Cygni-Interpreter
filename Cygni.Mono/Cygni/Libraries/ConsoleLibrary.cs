using System;
using Cygni.DataTypes;
using Cygni.Errors;

namespace Cygni.Libraries
{
    public static class ConsoleLibrary
    {
        public static DynValue clear(DynValue[]args)
        {
            Console.Clear();
            return DynValue.Void;
        }

        public static DynValue write(DynValue[]args)
        {
            RuntimeException.FuncArgsCheck(args.Length >= 1, "write");
            if (args.Length == 1)
                Console.Write(args[0].Value);
            else
            {
                string format = args[0].AsString();
                var arguments = new object[args.Length - 1];
                for (int i = 0; i < arguments.Length; i++)
                {
                    arguments[i] = args[i + 1].Value;
                }
                Console.Write(format, arguments);
            }
            return DynValue.Void;
        }

        public static DynValue writeLine(DynValue[]args)
        {
            RuntimeException.FuncArgsCheck(args.Length >= 1, "writeLine");
            if (args.Length == 1)
            {
                Console.WriteLine(args[0].Value);
            }
            else
            {
                string format = args[0].AsString();
                var arguments = new object[args.Length - 1];
                for (int i = 0; i < arguments.Length; i++)
                {
                    arguments[i] = args[i + 1].Value;
                }
                Console.WriteLine(format, arguments);
            }
            return DynValue.Void;
        }

        public static DynValue read(DynValue[]args)
        {
            return (long)Console.Read();
        }

        public static DynValue readLine(DynValue[]args)
        {
            return Console.ReadLine();
        }

        public static DynValue readKey(DynValue[]args)
        {
            return (long)Console.ReadKey().KeyChar;
        }

    }
}

