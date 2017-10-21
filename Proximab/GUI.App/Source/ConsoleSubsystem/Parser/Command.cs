using System;
using System.Collections.Generic;
using System.Globalization;

namespace GUI.App.Source.ConsoleSubsystem.Parser
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

        public Command(CommandType type, IList<string> arguments)
        {
            Type = type;
            Arguments = arguments;
        }

        public T GetArgument<T>(int index)
        {
            return (T)Convert.ChangeType(Arguments[index], typeof(T), CultureInfo.InvariantCulture);
        }
    }
}
