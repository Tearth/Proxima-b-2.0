using System;
using GUI.ColorfulConsole.Output;
using GUI.ContentDefinitions.Colors;

namespace GUI.ColorfulConsole
{
    /// <summary>
    /// The main class of ColorfulConsole library. 
    /// </summary>
    public class ColorfulConsoleManager
    {
        /// <summary>
        /// The output parser.
        /// </summary>
        private OutputParser _outputParser;

        /// <summary>
        /// The output printer.
        /// </summary>
        private ColorOutputPrinter _outputPrinter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorfulConsoleManager"/> class.
        /// </summary>
        public ColorfulConsoleManager()
        {
            _outputParser = new OutputParser();
            _outputPrinter = new ColorOutputPrinter();
        }

        /// <summary>
        /// Loads color definitions. Must be called before first use.
        /// </summary>
        /// <param name="colorDefinitionsContainer">The container of color definitions.</param>
        public void LoadContent(ColorDefinitionsContainer colorDefinitionsContainer)
        {
            _outputParser.SetColorDefinitions(colorDefinitionsContainer);
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
    }
}
