using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECP.App.ConsoleSubsystem
{
    public class CommandParser
    {
        private readonly char[] _separators = { ' ' };

        public Command Parse(string commandText)
        {
            var splittedInput = Split(commandText.Trim());

            if (splittedInput.Count == 0)
            {
                return null;
            }

            var name = splittedInput.First();
            var arguments = splittedInput.Skip(1).ToList();

            var commandType = GetCommandType(name);
            return new Command(commandType, arguments);
        }

        /// <summary>
        /// Splits the text into chunks with separators specified in the <see cref="_separators"/> field.
        /// </summary>
        /// <param name="text">The text to split.</param>
        /// <returns>The list of splitted chunks.</returns>
        private IList<string> Split(string text)
        {
            return text.Split(_separators, StringSplitOptions.RemoveEmptyEntries);
        }

        private CommandType GetCommandType(string commandNameText)
        {
            if(!Enum.TryParse(commandNameText, true, out CommandType commandType))
            {
                return CommandType.Unrecognised;
            }

            return commandType;
        }
    }
}
