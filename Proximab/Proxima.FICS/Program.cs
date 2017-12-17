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
        private static LogWriter _logWriter;

        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">Program arguments.</param>
        public static void Main(string[] args)
        {
            _logWriter = new LogWriter("Logs");
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            ProximaCore.Init();

            var consoleManager = new ColorfulConsoleManager("Proxima b 2.0dev FICS");
            var configManager = new ConfigManager("FICSConfig.xml");

            var ficsCore = new FICSCore(consoleManager, configManager, _logWriter);
            ficsCore.Run();
            
            Console.Read();
        }

        /// <summary>
        /// Catches all unhandles exceptions and logs them in a log file.
        /// </summary>
        /// <param name="sender">The unhandled exception sender.</param>
        /// <param name="e">The unhandled exception data.</param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception)e.ExceptionObject;

            _logWriter.WriteLine(exception.Message);
            _logWriter.WriteLine(exception.StackTrace);
        }
    }
}
