using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers.Loggers.Text;

namespace CECP.App.ConsoleSubsystem
{
    public class ConsoleManager
    {
        private TextLogger _textLogger;
        private CommandParser _commandParser;

        public ConsoleManager(TextLogger textLogger)
        {
            _textLogger = textLogger;
            _commandParser = new CommandParser();
        }

        public void WriteLine(string text)
        {
            _textLogger.WriteLine($"SEND: {text}");
        }

        public Command WaitForCommand()
        {
            var commandText = Console.ReadLine();
            _textLogger.WriteLine($"RECV: {commandText}");

            return _commandParser.Parse(commandText);
        }
    }
}
