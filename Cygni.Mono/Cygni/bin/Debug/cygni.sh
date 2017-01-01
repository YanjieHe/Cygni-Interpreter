#!/usr/bin/env csharp
LoadAssembly("Cygni");

using Cygni.Executors;
using Cygni.Settings;

Engine engine = Engine.CreateInstance ();
GlobalSettings.IsDebug = true;
engine.ExecuteInConsole (); 
