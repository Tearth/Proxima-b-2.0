using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxima.FICS.Source.LogSubsystem
{
    public class LogWriter : LogBase
    {
        public LogWriter(string directory) : base(directory)
        {
        }

        public void WriteLine(string text)
        {
            using (var logWriter = OpenOrCreateFile(".log"))
            {
                var output = $"{DateTime.Now.ToLongTimeString()} - {text}";
                logWriter.WriteLine(output);
            }
        }
    }
}
