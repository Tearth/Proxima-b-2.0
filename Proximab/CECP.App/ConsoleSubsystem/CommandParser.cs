using System;
using System.Collections.Generic;
using System.Linq;

namespace CECP.App.ConsoleSubsystem
{
    /// <summary>
    /// Represents a set of methods to parse CECP moves in text format.
    /// </summary>
    public class CommandParser
    {
        private readonly char[] _separators = { ' ' };

        /// <summary>
        /// Parses command to the <see cref="Command"/> object.
        /// </summary>
        /// <param name="commandText">The text with command to parse.</param>
        /// <returns>The parsed command object (null if input is empty).</returns>
        public Command Parse(string commandText)
        {
            var splitInput = Split(commandText.Trim());

            if (splitInput.Count == 0)
            {
                return null;
            }

            var name = splitInput.First();
            var arguments = splitInput.Skip(1).ToList();

            var commandType = GetCommandType(name);
            return new Command(commandType, arguments);
        }

        /// <summary>
        /// Splits the text into chunks with separators specified in the <see cref="_separators"/> field.
        /// </summary>
        /// <param name="text">The text to split.</param>
        /// <returns>The list of split chunks.</returns>
        private IList<string> Split(string text)
        {
            return text.Split(_separators, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Gets command type based on text name.
        /// </summary>
        /// <param name="commandNameText">The command text name.</param>
        /// <returns>The command type.</returns>
        private CommandType GetCommandType(string commandNameText)
        {
            if (!Enum.TryParse(commandNameText, true, out CommandType commandType))
            {
                return CommandType.Unrecognised;
            }

            return commandType;
        }
    }
}
