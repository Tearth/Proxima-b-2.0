using System;
using GUI.App.Source.ConsoleSubsystem.Parser;

namespace GUI.App.Source.ConsoleSubsystem
{
    internal class NewCommandEventArgs : EventArgs
    {
        public DateTime Time { get; private set; }
        public Command Command { get; private set; }

        public NewCommandEventArgs(DateTime time, Command command)
        {
            Time = time;
            Command = command;
        }
    }
}
