using System;
using Cygni.Executors;
using Cygni.Settings;

namespace Cygni_CLI
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Engine engine = Engine.CreateInstance();
            if (args.Length == 1)
            {
                string filePath = args[0];
                engine.ExecuteFile(filePath);
            }
            else
            {
                GlobalSettings.IsDebug = true;
                // GlobalSettings.CompleteErrorOutput = true;
                engine.ExecuteInConsole(); 
            }
        }
    }
}
