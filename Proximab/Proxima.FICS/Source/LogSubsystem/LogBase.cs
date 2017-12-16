using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxima.FICS.Source.LogSubsystem
{
    public abstract class LogBase
    {
        private string _directory;

        public LogBase(string directory)
        {
            _directory = directory;
        }

        protected StreamWriter OpenOrCreateFile(string extension)
        {
            var logFileName = DateTime.Now.ToString("dd-MM-yyyy") + extension;
            var fullLogFilePath = _directory + "/" + logFileName;

            return new StreamWriter(fullLogFilePath, true);
        }
    }
}
