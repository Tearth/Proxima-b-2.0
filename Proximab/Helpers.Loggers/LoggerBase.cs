using System;
using System.IO;
using System.Reflection;

namespace Helpers.Loggers
{
    /// <summary>
    /// Represents a base class for add derived log classes.
    /// </summary>
    public abstract class LogBase
    {
        private const string TimeFormat = "HH:mm:ss";
        private const string DateFormat = "dd-MM-yyyy";

        private string _directory;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogBase"/> class.
        /// </summary>
        /// <param name="directory">The directory where all logs will be stored.</param>
        protected LogBase(string directory)
        {
            _directory = directory;

            var logsDirectory = GetLogsDirectory();
            if (!Directory.Exists(logsDirectory))
            {
                Directory.CreateDirectory(logsDirectory);
            }
        }

        /// <summary>
        /// Creates a new <see cref="StreamWriter"/> object with associated log file.
        /// </summary>
        /// <param name="extension">The log file extension.</param>
        /// <returns>The stream writer with associated log file.</returns>
        protected StreamWriter OpenOrCreateFile(string extension)
        {
            var logsDirectory = GetLogsDirectory();

            var logFileName = DateTime.Now.ToString(DateFormat) + extension;
            var fullLogFilePath = logsDirectory + "/" + logFileName;

            return new StreamWriter(fullLogFilePath, true);
        }

        /// <summary>
        /// Gets current time with the specified format (HH:mm:ss).
        /// </summary>
        /// <returns>The current time string.</returns>
        protected string GetCurrentTime()
        {
            return DateTime.Now.ToString(TimeFormat);
        }

        /// <summary>
        /// Gets the directory where app exe is stored.
        /// </summary>
        /// <returns>The app directory.</returns>
        private string GetAppDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        /// <summary>
        /// Gets the logs directory.
        /// </summary>
        /// <returns>The logs directory.</returns>
        private string GetLogsDirectory()
        {
            return GetAppDirectory() + "/" + _directory;
        }
    }
}
