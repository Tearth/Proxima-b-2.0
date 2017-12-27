using System;
using FICS.App.ConfigSubsystem;
using Helpers.ColorfulConsole;
using Helpers.Loggers.Text;
using Proxima.Core;

namespace FICS.App
{
    /// <summary>
    /// Represents the entry point class with Main method.
    /// </summary>
    public class Program
    {
        private const string LogsDirectory = "Logs";
        private const string ApplicationName = "Proxima b 2.0dev FICS";
        private const string ConfigFilename = "FICSConfig.xml";

        private static TextLogger _textLogger;

        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">Program arguments.</param>
        public static void Main(string[] args)
        {
            _textLogger = new TextLogger(LogsDirectory);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            ProximaCore.Init();

            var consoleManager = new ColorfulConsoleManager(ApplicationName);
            var configManager = new ConfigManager(ConfigFilename);

            var ficsCore = new FicsCore(consoleManager, configManager, _textLogger);
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

            _textLogger.WriteLine(exception.Message);
            _textLogger.WriteLine(exception.StackTrace);
        }
    }
}
