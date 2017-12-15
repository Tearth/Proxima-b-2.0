using System;
using GUI.ColorfulConsole;
using Proxima.Core;
using Proxima.FICS.Source;
using Proxima.FICS.Source.ConfigSubsystem;
using Proxima.FICS.Source.LogSubsystem;

namespace Proxima.FICS
{
    /// <summary>
    /// Represents the entry point class with Main method.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">Program arguments.</param>
        public static void Main(string[] args)
        {
            ProximaCore.Init();

            var consoleManager = new ColorfulConsoleManager("Proxima b 2.0dev FICS");
            var configManager = new ConfigManager("FICSConfig.xml");
            var logWriter = new LogWriter("Logs");

            var ficsCore = new FICSCore(consoleManager, configManager, logWriter);
            ficsCore.Run();

            Console.Read();
        }
    }
}
