using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Loggers.Text
{
    /// <summary>
    /// Represents a set of methods to writing in log file.
    /// </summary>
    public class TextLogger : LogBase
    {
        private const string FileExtension = ".log";

        /// <summary>
        /// Initializes a new instance of the <see cref="TextLogger"/> class.
        /// </summary>
        /// <param name="directory">The directory where all logs will be stored.</param>
        public TextLogger(string directory) : base(directory)
        {
        }

        /// <summary>
        /// Writes a new line with the specified text to the log file and immedialy saves it.
        /// </summary>
        /// <param name="text">The text to save (without end line chars).</param>
        public void WriteLine(string text)
        {
            using (var textLogger = OpenOrCreateFile(FileExtension))
            {
                var output = $"{GetCurrentTime()} - {text}";
                textLogger.WriteLine(output);
            }
        }
    }
}
