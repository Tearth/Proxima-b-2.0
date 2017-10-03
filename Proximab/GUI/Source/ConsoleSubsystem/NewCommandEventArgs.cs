using GUI.Source.ConsoleSubsystem.Parser;
using System;

namespace GUI.Source.ConsoleSubsystem
{
    internal class NewCommandEventArgs : EventArgs
    {
        public DateTime Time { get; set; }
        public Command Command { get; set; }

        public NewCommandEventArgs(DateTime time, Command command)
        {
            Time = time;
            Command = command;
        }
    }
}
