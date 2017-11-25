using System;
using System.Collections.Generic;
using System.Linq;
using GUI.App.Source.CommandsSubsystem.Exceptions;
using GUI.App.Source.CommandsSubsystem.Parsers;
using GUI.App.Source.CommandsSubsystem.Validators;
using GUI.ContentDefinitions.Commands;

namespace GUI.App.Source.CommandsSubsystem
{
    /// <summary>
    /// The delegate used in <see cref="CommandsManager"/> class to manage command handlers.
    /// </summary>
    /// <param name="command">The command instance.</param>
    internal delegate void ExecuteCommandDelegate(Command command);

    /// <summary>
    /// Represents a set of methods to manage and execute commands.
    /// </summary>
    internal class CommandsManager
    {
        private CommandParser _commandParser;
        private CommandValidator _commandValidator;

        private CommandDefinitionsContainer _commandDefinitionsContainer;
        private Dictionary<CommandType, ExecuteCommandDelegate> _commandHandlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandsManager"/> class.
        /// </summary>
        public CommandsManager()
        {
            _commandParser = new CommandParser();
            _commandValidator = new CommandValidator();

            _commandHandlers = new Dictionary<CommandType, ExecuteCommandDelegate>();
        }

        /// <summary>
        /// Loads command definitions. Must be called before first use of any other class method.
        /// </summary>
        /// <param name="commandDefinitionsContainer">The container of command definitions.</param>
        public void LoadContent(CommandDefinitionsContainer commandDefinitionsContainer)
        {
            _commandDefinitionsContainer = commandDefinitionsContainer;
        }

        /// <summary>
        /// Adds the specified command handler associated with the command type.
        /// </summary>
        /// <param name="commandType">The command type.</param>
        /// <param name="handler">The command handler.</param>
        public void AddCommandHandler(CommandType commandType, ExecuteCommandDelegate handler)
        {
            if (_commandHandlers.ContainsKey(commandType))
            {
                throw new CommandHandlerAlreadyRegisteredException();
            }

            _commandHandlers[commandType] = handler;
        }

        /// <summary>
        /// Removes a command handler for the specified command type.
        /// </summary>
        /// <param name="commandType">The command type.</param>
        public void RemoveCommandHandler(CommandType commandType)
        {
            _commandHandlers.Remove(commandType);
        }

        /// <summary>
        /// Processes and executes a command specified in the input.
        /// </summary>
        /// <param name="userInput">The command to execute.</param>
        /// <returns>The status of command execution.</returns>
        public ExecutionResult Execute(string userInput)
        {
            var rawCommand = GetRawCommand(userInput);
            if (rawCommand == null)
            {
                return ExecutionResult.EmptyCommand;
            }

            var commandDefinition = GetCommandDefinition(rawCommand);
            if (commandDefinition == null)
            {
                return ExecutionResult.CommandNotFound;
            }

            var validationResult = ValidateCommand(rawCommand, commandDefinition);
            if (!validationResult)
            {
                return ExecutionResult.InvalidCommandFormat;
            }

            var command = GetCommand(rawCommand, commandDefinition);
            if (!_commandHandlers.ContainsKey(command.Type))
            {
                throw new CommandHandlerNotFoundException();
            }

            var commandHandler = _commandHandlers[command.Type];
            commandHandler.Invoke(command);

            return ExecutionResult.Success;
        }

        /// <summary>
        /// Processes a text command into RawCommand object.
        /// </summary>
        /// <param name="command">The command to process.</param>
        /// <returns>RawCommand object.</returns>
        private RawCommand GetRawCommand(string command)
        {
            var fixedCommand = command.ToLower().Trim();
            return _commandParser.Parse(fixedCommand);
        }

        /// <summary>
        /// Searches a command definition for the specified command name included in RawCommand object.
        /// </summary>
        /// <param name="rawCommand">The initially processed command.</param>
        /// <returns>The command definition.</returns>
        private CommandDefinition GetCommandDefinition(RawCommand rawCommand)
        {
            return _commandDefinitionsContainer.Definitions
                        .FirstOrDefault(p => p.Name.ToLower().Trim() == rawCommand.Name);
        }

        /// <summary>
        /// Checks if the initially processed command is compatible with his definition.
        /// </summary>
        /// <param name="rawCommand">The initially processed command.</param>
        /// <param name="commandDefinition">The command definition.</param>
        /// <returns>True if the command is compatible with his definition, otherwise false.</returns>
        private bool ValidateCommand(RawCommand rawCommand, CommandDefinition commandDefinition)
        {
            return _commandValidator.Validate(rawCommand, commandDefinition);
        }

        /// <summary>
        /// Processes the RawCommand object into a full Command object.
        /// </summary>
        /// <param name="rawCommand">The initially processed command.</param>
        /// <param name="commandDefinition">The command definition.</param>
        /// <returns>Command object.</returns>
        private Command GetCommand(RawCommand rawCommand, CommandDefinition commandDefinition)
        {
            var enumType = (CommandType)Enum.Parse(typeof(CommandType), commandDefinition.EnumType);
            return new Command(enumType, rawCommand.Arguments);
        }
    }
}
