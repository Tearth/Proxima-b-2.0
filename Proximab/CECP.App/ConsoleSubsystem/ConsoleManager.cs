using System;
using Helpers.Loggers.Text;

namespace CECP.App.ConsoleSubsystem
{
    /// <summary>
    /// Represents a set of methods to manage console input and output.
    /// </summary>
    public class ConsoleManager
    {
        private TextLogger _textLogger;
        private CommandParser _commandParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleManager"/> class.
        /// </summary>
        /// <param name="textLogger">The text logger.</param>
        public ConsoleManager(TextLogger textLogger)
        {
            _textLogger = textLogger;
            _commandParser = new CommandParser();
        }

        /// <summary>
        /// Writes a text on the console and ends it with new line chars.
        /// </summary>
        /// <param name="text">The text to write.</param>
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
            _textLogger.WriteLine($"SEND: {text}");
        }

        /// <summary>
        /// Waits for new command from CECP interface and returns it.
        /// </summary>
        /// <returns>The received command.</returns>
        public Command WaitForCommand()
        {
            var commandText = Console.ReadLine();
            _textLogger.WriteLine($"RECV: {commandText}");

            return _commandParser.Parse(commandText);
        }
    }
}
