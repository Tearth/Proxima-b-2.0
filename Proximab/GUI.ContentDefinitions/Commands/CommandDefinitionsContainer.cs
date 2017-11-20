using System.Collections.Generic;

namespace GUI.ContentDefinitions.Commands
{
    /// <summary>
    /// Container of piece definitions (required by MonoGame Pipeline Tool).
    /// </summary>
    public class CommandDefinitionsContainer
    {
        /// <summary>
        /// Gets or sets the command definitions list.
        /// </summary>
        public List<CommandDefinition> Definitions { get; set; }
    }
}
