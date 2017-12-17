using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxima.FICS.Source.LogSubsystem
{
    /// <summary>
    /// Represents a base class for add derived log classes.
    /// </summary>
    public abstract class LogBase
    {
        private string _directory;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogBase"/> class.
        /// </summary>
        /// <param name="directory">The directory where all logs will be stored.</param>
        public LogBase(string directory)
        {
            _directory = directory;
        }

        /// <summary>
        /// Creates a new <see cref="StreamWriter"/> object with associated log file.
        /// </summary>
        /// <param name="extension">The log file extension.</param>
        /// <returns>The stream writer with associated log file.</returns>
        protected StreamWriter OpenOrCreateFile(string extension)
        {
            var logFileName = DateTime.Now.ToString("dd-MM-yyyy") + extension;
            var fullLogFilePath = _directory + "/" + logFileName;

            return new StreamWriter(fullLogFilePath, true);
        }

        /// <summary>
        /// Gets current time with the specified format (HH:mm:ss).
        /// </summary>
        /// <returns>The current time string.</returns>
        protected string GetCurrentTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
