using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Source.ConsoleSubsystem.Parser
{
    internal class CommandParser
    {
        const char Separator = ' ';

        public CommandParser()
        {

        }

        public Command Parse(string input)
        {
            var splittedInput = SplitInput(input);

            throw new NotImplementedException();
        }

        IList<string> SplitInput(string input)
        {
            return input.Split(Separator);
        }
    }
}
