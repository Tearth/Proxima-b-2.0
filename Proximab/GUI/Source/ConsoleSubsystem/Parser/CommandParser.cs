using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Source.ConsoleSubsystem.Parser
{
    internal class CommandParser
    {
        readonly char[] Separators = { ' ' };

        public CommandParser()
        {

        }

        public RawCommand Parse(string input)
        {
            var splittedInput = SplitInput(input.Trim());

            if (splittedInput.Count == 0)
                return null;

            var name = splittedInput.First();
            var arguments = splittedInput.Skip(1).ToList();

            return new RawCommand(name, arguments);
        }

        IList<string> SplitInput(string input)
        {
            return input.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
