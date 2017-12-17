using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxima.FICS.Source.LogSubsystem
{
    /// <summary>
    /// Represents a set of methods to writing in log file.
    /// </summary>
    public class LogWriter : LogBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriter"/> class.
        /// </summary>
        /// <param name="directory">The directory where all logs will be stored.</param>
        public LogWriter(string directory) : base(directory)
        {
        }

        /// <summary>
        /// Writes a new line with the specified text to the log file and immedialy saves it.
        /// </summary>
        /// <param name="text">The text to save (without end line chars).</param>
        public void WriteLine(string text)
        {
            using (var logWriter = OpenOrCreateFile(".log"))
            {
                var output = $"{GetCurrentTime()} - {text}";
                logWriter.WriteLine(output);
            }
        }
    }
}
