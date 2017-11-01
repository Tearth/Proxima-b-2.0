using System.Collections.Generic;

namespace GUI.App.Source.ConsoleSubsystem.Parser
{
    public class RawCommand
    {
        public string Name { get; private set; }
        public IList<string> Arguments { get; private set; }

        public RawCommand()
        {
            Arguments = new List<string>();
        }

        public RawCommand(string name, IList<string> arguments)
        {
            Name = name;
            Arguments = arguments;
        }
    }
}
