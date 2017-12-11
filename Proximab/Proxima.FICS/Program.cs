using System;
using GUI.ColorfulConsole;
using Proxima.FICS.Source;
using Proxima.FICS.Source.ConfigSubsystem;

namespace Proxima.FICS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var consoleManager = new ColorfulConsoleManager("Proxima b 2.0dev FICS");
            var configManager = new ConfigManager("FICSConfig.xml");

            var ficsCore = new FICSCore(consoleManager, configManager);
            ficsCore.Run();
        }
    }
}
