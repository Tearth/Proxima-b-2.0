using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Source.ConsoleSubsystem.Parser
{
    public class RawCommand
    {
        public string Name { get; set; }
        public IList<string> Arguments { get; set; }

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
