using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers.Loggers.Text;
using Proxima.Core;

namespace CECP.App
{
    /// <summary>
    /// Represents the entry point class with Main method.
    /// </summary>
    public class Program
    {
        private const string LogsDirectory = "Logs";

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

            var core = new CECPCore(_textLogger);
            core.Run();
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
