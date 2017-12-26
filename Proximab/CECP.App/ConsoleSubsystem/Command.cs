using System;
using System.Collections.Generic;
using System.Globalization;

namespace CECP.App.ConsoleSubsystem
{
    /// <summary>
    /// Represents information about the command.
    /// </summary>
    public class Command
    {
        /// <summary>
        /// Gets the command type.
        /// </summary>
        public CommandType Type { get; }

        /// <summary>
        /// Gets the command arguments.
        /// </summary>
        public IList<string> Arguments { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class with default values.
        /// </summary>
        public Command()
        {
            Type = CommandType.Unrecognised;
            Arguments = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="type">The command type.</param>
        /// <param name="arguments">The command arguments.</param>
        public Command(CommandType type, IList<string> arguments)
        {
            Type = type;
            Arguments = arguments;
        }

        /// <summary>
        /// Calculates and returns command argument with type specified in T.
        /// </summary>
        /// <typeparam name="T">Expected type of the argument</typeparam>
        /// <param name="index">Index of the argument (0, 1, 2, ...)</param>
        /// <returns>The command argument with specified index and type.</returns>
        public T GetArgument<T>(int index)
        {
            return (T)Convert.ChangeType(Arguments[index], typeof(T), CultureInfo.InvariantCulture);
        }
    }
}
