using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST;
using Cygni.AST.Scopes;
using Cygni.Settings;

namespace Cygni.Loaders
{
    /// <summary>
    /// Description of Executor.
    /// </summary>
    public abstract class Loader
    {
        public abstract void Run();

        public abstract DynValue Result { get; }

        protected IScope GlobalScope;

        protected Loader(IScope GlobalScope)
        {
            this.GlobalScope = GlobalScope;
        }

        protected void HandleException(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (GlobalSettings.CompleteErrorOutput)
            {
                Console.WriteLine("error: {0}", ex);
            }
            else
            {
                Console.WriteLine("error: {0}", ex.Message);
            }
        }
    }
}
