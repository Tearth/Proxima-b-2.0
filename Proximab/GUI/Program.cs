using GUI.Source.ConsoleSubsystem;
using System;

namespace GUI
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            var consoleManager = new ConsoleManager();
            consoleManager.Run();

            using (var game = new GUICore(consoleManager))
                game.Run();
        }
    }
}
