using System;
using Cygni;
using Cygni.DataTypes;
using NUnit.Framework;
using System.Diagnostics;

namespace TestRunner
{
    class MainClass
    {
        static Engine engine = Engine.CreateInstance();

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        private static void FactorialTest(int n)
        {
            engine.Evaluate(@"
					def fact(n) {
						if n == 0 {
							return 1
						} else {
							return n * fact(n - 1)
						}
                    }
					");
            Function function = engine.GetSymbol("fact").As<Function>();
            DynValue[] args = new DynValue[]{ n };
            var watch = new Stopwatch();
            watch.Start();
            DynValue result = function.DynInvoke(args);
            watch.Stop();
            Console.WriteLine("Factorial Test: ElapsedMilliseconds-> {0}", watch.ElapsedMilliseconds);
            Assert.AreEqual(result.AsInt32(), Factorial(n));
        }

        private static int Factorial(int n)
        {
            if (n == 0)
            {
                return 1;
            }
            else
            {
                return n * Factorial(n - 1);
            }
        }

        private static void FibTest(int n)
        {
            engine.Evaluate(@"
                 def fib(n) {
                    if n < 2 {
                        return n
                    }
                    else {
                        return fib(n-1)+fib(n-2)
                    }
                }
            ");
            Function function = engine.GetSymbol("test").As<Function>();

            DynValue[] args = new DynValue[]{ (long)n };
            var watch = new Stopwatch();
            watch.Start();
            DynValue result = function.DynInvoke(args);
            watch.Stop();
            Console.WriteLine("Fib Test: ElapsedMilliseconds-> {0}", watch.ElapsedMilliseconds);
            Assert.AreEqual(result.AsInt32(), Fib(n));
        }

        private static int Fib(int n)
        {
            if (n < 2)
            {
                return n;
            }
            else
            {
                return Fib(n - 1) + Fib(n - 2);
            }
        }

    }
}
