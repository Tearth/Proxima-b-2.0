using System;
using GUI.App.Source;
using GUI.App.Source.CommandsSubsystem;
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
            var commandsManager = new CommandsManager();

            var consoleManager = new ConsoleManager(commandsManager);
            consoleManager.RunAsync();

            ProximaCore.Init();

            using (var game = new GUICore(consoleManager, commandsManager))
            {
                game.Run();
            }
        }
    }
}
