using System.Collections.Generic;

namespace ContentDefinitions.Commands
{
    public class CommandDefinition
    {
        public string Name { get; set; }
        public string EnumType { get; set; }
        public List<string> Arguments { get; set; }
    }
}
