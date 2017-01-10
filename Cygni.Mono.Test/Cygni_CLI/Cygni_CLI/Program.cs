using System;
using Cygni.Settings;
using Cygni;

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
