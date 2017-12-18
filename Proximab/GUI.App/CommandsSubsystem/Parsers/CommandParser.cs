using System;
using System.Collections.Generic;
using System.Linq;

namespace GUI.App.CommandsSubsystem.Parsers
{
    /// <summary>
    /// Represents a set of methods to parse command (from text to <see cref="RawCommand"/> object).
    /// </summary>
    public class CommandParser
    {
        private readonly char[] _separators = { ' ' };

        /// <summary>
        /// Splits the user input into tokens and stores them in the RawCommand object.
        /// </summary>
        /// <param name="userInput">The user input entered by user.</param>
        /// <returns>RawCommand if possible, otherwise null.</returns>
        public RawCommand Parse(string userInput)
        {
            var splittedInput = Split(userInput.Trim());

            if (splittedInput.Count == 0)
            {
                return null;
            }

            var name = splittedInput.First();
            var arguments = splittedInput.Skip(1).ToList();

            return new RawCommand(name, arguments);
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
    }
}
