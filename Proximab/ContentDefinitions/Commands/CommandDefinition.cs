using System.Collections.Generic;

namespace ContentDefinitions.Commands
{
    public class CommandDefinition
    {
        public string Name { get; set; }
        public string EnumType { get; set; }
        public string Description { get; set; }
        public List<CommandArgumentDefinition> Arguments { get; set; }
    }
}
