using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CECP.App.ConsoleSubsystem.Exceptions;

namespace CECP.App.ConsoleSubsystem
{
    /// <summary>
    /// The delegate used in <see cref="CommandsManager"/> class to manage command handlers.
    /// </summary>
    /// <param name="command">The command instance.</param>
    /// <returns>The response.</returns>
    public delegate void ExecuteCommandDelegate(Command command);

    /// <summary>
    /// Represents a set of methods to manage and execute commands.
    /// </summary>
    public class CommandsManager
    {
        private Dictionary<CommandType, ExecuteCommandDelegate> _commandHandles;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandsManager"/> class.
        /// </summary>
        public CommandsManager()
        {
            _commandHandles = new Dictionary<CommandType, ExecuteCommandDelegate>();
        }

        /// <summary>
        /// Adds the specified command handler associated with the command type.
        /// </summary>
        /// <param name="commandType">The command type.</param>
        /// <param name="handler">The command handler.</param>
        public void AddCommandHandler(CommandType commandType, ExecuteCommandDelegate handler)
        {
            if (_commandHandles.ContainsKey(commandType))
            {
                throw new CommandTypeAlreadyRegisteredException();
            }

            _commandHandles.Add(commandType, handler);
        }

        /// <summary>
        /// Removes all command handlers for the specified command type.
        /// </summary>
        /// <param name="commandType">The command type.</param>
        public void RemoveCommandHandler(CommandType commandType)
        {
            _commandHandles.Remove(commandType);
        }

        /// <summary>
        /// Processes and executes a command specified in the input.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>The response (<see cref="string.Empty"/> if none).</returns>
        public void Execute(Command command)
        {
            if (_commandHandles.ContainsKey(command.Type))
            {
                var commandHandler = _commandHandles[command.Type];
                commandHandler.Invoke(command);
            }
        }
    }
}
