using System.Collections.Generic;

namespace GUI.ContentDefinitions.Commands
{
    public class CommandDefinition
    {
        /// <summary>
        /// Name of the command.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of the CommandType enum value.
        /// </summary>
        public string EnumType { get; set; }

        /// <summary>
        /// Description of the command.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// List of the command arguments.
        /// </summary>
        public List<CommandArgumentDefinition> Arguments { get; set; }
    }
}
