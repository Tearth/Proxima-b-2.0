using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECP.App.ConsoleSubsystem
{
    public class ConsoleManager
    {
        private CommandParser _commandParser;

        public ConsoleManager()
        {
            _commandParser = new CommandParser();
        }

        public void WriteLine(string text)
        {

        }

        public Command WaitForCommand()
        {
            var commandText = Console.ReadLine();
            return _commandParser.Parse(commandText);
        }
    }
}
