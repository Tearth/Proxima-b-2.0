using System.Collections.Generic;

namespace GUI.ContentDefinitions.Commands
{
    /// <summary>
    /// Container od piece definitions (required by MonoGame Pipeline Tool).
    /// </summary>
    public class CommandDefinitionsContainer
    {
        public List<CommandDefinition> Definitions { get; set; }
    }
}
