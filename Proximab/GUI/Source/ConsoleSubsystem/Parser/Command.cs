using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Source.ConsoleSubsystem.Parser
{
    internal class Command
    {
        public CommandType Type { get; set; }
        public List<string> Arguments { get; set; }

        public Command()
        {
            Type = CommandType.None;
            Arguments = new List<string>();
        }

        public T GetArgument<T>(int index)
        {
            return (T)Convert.ChangeType(Arguments[index], typeof(T));
        }
    }
}
