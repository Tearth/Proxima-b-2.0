using Proxima.Core;
using GUI.App.Source;
using GUI.App.Source.ConsoleSubsystem;
using System;

namespace GUI.App
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            var consoleManager = new ConsoleManager();
            consoleManager.Run();

            ProximaCore.Init();

            using (var game = new GUICore(consoleManager))
                game.Run();
        }
    }
}
