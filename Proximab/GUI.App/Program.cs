using System;
using GUI.App.Source;
using GUI.App.Source.ConsoleSubsystem;
using Proxima.Core;

namespace GUI.App
{
    /// <summary>
    /// Represents the entry point class with Main method.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            var consoleManager = new ConsoleManager();
            consoleManager.RunAsync();

            ProximaCore.Init();

            using (var game = new GUICore(consoleManager))
            {
                game.Run();
            }
        }
    }
}
