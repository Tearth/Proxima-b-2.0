using System;
using System.Collections.Generic;
using System.Linq;

namespace GUI.App.Source.CommandsSubsystem.Parsers
{
    /// <summary>
    /// Represents a set of methods to parse commands (from text to RawCommand objects).
    /// </summary>
    internal class CommandParser
    {
        private readonly char[] _separators = { ' ' };

        /// <summary>
        /// Processes input to RawCommand.
        /// </summary>
        /// <param name="input">The input entered by user.</param>
        /// <returns>RawCommand if possible, otherwise null.</returns>
        public RawCommand Parse(string input)
        {
            var splittedInput = SplitInput(input.Trim());

            if (splittedInput.Count == 0)
            {
                return null;
            }

            var name = splittedInput.First();
            var arguments = splittedInput.Skip(1).ToList();

            return new RawCommand(name, arguments);
        }

        /// <summary>
        /// Splits text info chunks with separators specified in the <see cref="_separators"/> field.
        /// </summary>
        /// <param name="input">The text to split.</param>
        /// <returns>The list of splitted chunks.</returns>
        private IList<string> SplitInput(string input)
        {
            return input.Split(_separators, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
