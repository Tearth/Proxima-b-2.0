using System.Collections.Generic;

namespace GUI.ContentDefinitions.Commands
{
    /// <summary>
    /// Represents information about the command definition
    /// </summary>
    public class CommandDefinition
    {
        /// <summary>
        /// Gets or sets the name of the command.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the CommandType enum value.
        /// </summary>
        public string EnumType { get; set; }

        /// <summary>
        /// Gets or sets the description of the command.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the list of the command arguments.
        /// </summary>
        public List<CommandArgumentDefinition> Arguments { get; set; }
    }
}
