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
        private static ColorfulConsoleManager _consoleManager;
        private static ConfigManager _configManager;
        private static FICSCore _ficsCore;

        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">Program arguments.</param>
        public static void Main(string[] args)
        {
            ProximaCore.Init();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            _textLogger = new TextLogger(LogsDirectory);
            _consoleManager = new ColorfulConsoleManager(ApplicationName);
            _configManager = new ConfigManager(ConfigFilename);
            _ficsCore = new FICSCore(_consoleManager, _configManager, _textLogger);

            _ficsCore.Run();

            while (true)
            {
                var text = Console.ReadLine();
                if (text != null)
                {
                    if (text.Contains("quit"))
                    {
                        break;
                    }

                    _ficsCore.Send(text);
                }
            }
        }

        /// <summary>
        /// Catches all unhandled exceptions and logs them in a log file.
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
