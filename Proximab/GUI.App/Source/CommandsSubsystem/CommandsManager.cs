using System.Collections.Generic;
using GUI.App.Source.CommandsSubsystem.Exceptions;

namespace GUI.App.Source.CommandsSubsystem
{
    internal delegate void ExecuteCommandDelegate(Command command);

    internal class CommandsManager
    {
        private Dictionary<CommandType, ExecuteCommandDelegate> _commandHandlers;

        public CommandsManager()
        {
            _commandHandlers = new Dictionary<CommandType, ExecuteCommandDelegate>();
        }

        public void AddCommandHandler(CommandType commandType, ExecuteCommandDelegate handler)
        {
            if (_commandHandlers.ContainsKey(commandType))
            {
                throw new CommandHandlerAlreadyRegisteredException();
            }

            _commandHandlers[commandType] = handler;
        }

        public void RemoveCommandHandler(CommandType commandType)
        {
            _commandHandlers.Remove(commandType);
        }

        public void Execute(Command command)
        {
            if (!_commandHandlers.ContainsKey(command.Type))
            {
                throw new CommandHandlerNotFoundException();
            }

            var commandHandler = _commandHandlers[command.Type];
            commandHandler.Invoke(command);
        }
    }
}
