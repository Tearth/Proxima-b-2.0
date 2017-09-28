using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Source.ConsoleSubsystem.Parser
{
    internal class Command
    {
        public CommandType Type { get; set; }
        public IList<string> Arguments { get; set; }

        public Command()
        {
            Type = CommandType.None;
            Arguments = new List<string>();
        }

        public T GetArgument<T>(int index)
        {
            return (T)Convert.ChangeType(Arguments[index - 1], typeof(T), CultureInfo.InvariantCulture);
        }
    }
}
