using System;
using Helpers.ColorfulConsole.Diagnostic;
using Helpers.ColorfulConsole.Output;

namespace Helpers.ColorfulConsole
{
    /// <summary>
    /// The main class of ColorfulConsole library. 
    /// </summary>
    public class ColorfulConsoleManager
    {
        private OutputParser _outputParser;
        private ColorOutputPrinter _outputPrinter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorfulConsoleManager"/> class.
        /// </summary>
        /// <param name="appName">The application name (to display in console header).</param>
        public ColorfulConsoleManager(string appName)
        {
            _outputParser = new OutputParser();
            _outputPrinter = new ColorOutputPrinter();

            WriteHeader(appName);
        }

        /// <summary>
        /// Writes the specified output on the console. Internal parser replaces every $X
        /// (where X is the color symbol) to the foreground color.
        /// </summary>
        /// <param name="text">The content to display.</param>
        public void Write(string text)
        {
            var outputChunks = _outputParser.GetOutputChunks(text);
            _outputPrinter.Write(outputChunks);
        }

        /// <summary>
        /// Writes the empty line on the console.
        /// </summary>
        public void WriteLine()
        {
            WriteLine(string.Empty);
        }

        /// <summary>
        /// Writes the specified output on the console. Internal parser replaces every $X
        /// (where X is the color symbol) to the foreground color.
        /// </summary>
        /// <param name="text">The content to display.</param>
        public void WriteLine(string text)
        {
            var outputChunks = _outputParser.GetOutputChunks(text);
            _outputPrinter.WriteLine(outputChunks);
        }

        /// <summary>
        /// Writes header to the user console (should be called only once at program startup).
        /// </summary>
        /// <param name="appName">The application name.</param>
        private void WriteHeader(string appName)
        {
            var environmentInfoProvider = new EnvironmentInfoProvider();

            var osInfo = environmentInfoProvider.OSInfo;
            var cpuPlatform = environmentInfoProvider.CPUPlatformVersion;
            var processPlatform = environmentInfoProvider.ProcessPlatformVersion;
            var coresCount = environmentInfoProvider.CPUCoresCount;

            WriteLine($"$g{appName}$w | {osInfo} " +
                      $"(CPU $c{cpuPlatform}$w, {coresCount}$w | Process $c{processPlatform}$w)");
        }
    }
}
