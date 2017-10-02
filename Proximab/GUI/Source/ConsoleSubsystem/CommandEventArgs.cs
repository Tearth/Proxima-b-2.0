using GUI.Source.ConsoleSubsystem.Parser;
using System;

namespace GUI.Source.ConsoleSubsystem
{
    internal class CommandEventArgs : EventArgs
    {
        public DateTime Time { get; set; }
        public Command Command { get; set; }

        public CommandEventArgs(DateTime time, Command command)
        {
            Time = time;
            Command = command;
        }
    }
}
