using System;
using GUI.App.Source.CommandsSubsystem;

namespace GUI.App.Source.ConsoleSubsystem
{
    /// <summary>
    /// Represents the NewCommand event arguments.
    /// </summary>
    internal class NewCommandEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the time of command execution.
        /// </summary>
        public DateTime Time { get; private set; }

        /// <summary>
        /// Gets the command instance.
        /// </summary>
        public Command Command { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewCommandEventArgs"/> class.
        /// </summary>
        /// <param name="time">The execution time of the command.</param>
        /// <param name="command">The command instance.</param>
        public NewCommandEventArgs(DateTime time, Command command)
        {
            Time = time;
            Command = command;
        }
    }
}
