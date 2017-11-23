using System.Collections.Generic;

namespace GUI.App.Source.CommandsSubsystem.Parsers
{
    /// <summary>
    /// Represents information about raw (without enum values) command.
    /// </summary>
    public class RawCommand
    {
        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the list of command arguments.
        /// </summary>
        public IList<string> Arguments { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RawCommand"/> class with default values.
        /// </summary>
        public RawCommand()
        {
            Arguments = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RawCommand"/> class.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <param name="arguments">The list of command arguments.</param>
        public RawCommand(string name, IList<string> arguments)
        {
            Name = name;
            Arguments = arguments;
        }
    }
}
