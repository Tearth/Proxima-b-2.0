using System;
using System.Threading.Tasks;
using GUI.App.CommandsSubsystem;
using GUI.ContentDefinitions.Commands;
using Helpers.ColorfulConsole;
using Microsoft.Xna.Framework.Content;

namespace GUI.App.ConsoleSubsystem
{
    /// <summary>
    /// Represents a set of methods to manage a console.
    /// </summary>
    public class ConsoleManager
    {
        private ColorfulConsoleManager _colorfulConsole;

        private Task _consoleLoop;
        private CommandsManager _commandsManager;

        private CommandDefinitionsContainer _commandDefinitionsContainer;

        private bool _loopRunning;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleManager"/> class.
        /// </summary>
        /// <param name="commandsManager">The commands manager instance.</param>
        /// <param name="appName">The application name (to display in console header).</param>
        public ConsoleManager(CommandsManager commandsManager, string appName)
        {
            _commandsManager = commandsManager;
            _consoleLoop = new Task(Loop);

            _colorfulConsole = new ColorfulConsoleManager(appName);
            _loopRunning = false;

            SetCommandHandlers();
        }

        /// <summary>
        /// Loads the resources. Must be called before first use of any other class method.
        /// </summary>
        /// <param name="contentManager">Monogame content manager.</param>
        public void LoadContent(ContentManager contentManager)
        {
            _commandDefinitionsContainer = contentManager.Load<CommandDefinitionsContainer>("XML\\CommandDefinitions");
            _commandsManager.LoadContent(_commandDefinitionsContainer);
        }

        /// <summary>
        /// Runs asynchronously the task with console loop.
        /// </summary>
        public async void RunAsync()
        {
            _loopRunning = true;

            try
            {
                _consoleLoop.Start();
                await _consoleLoop;
            }
            catch (Exception ex)
            {
                WriteLine("$rCritical error");
                WriteLine($"$r{ex.Message}");

                if (ex.InnerException != null)
                {
                    WriteLine($"$r{ex.InnerException}");
                }
            }
        }

        /// <summary>
        /// Writes empty line to the user console.
        /// </summary>
        public void WriteLine()
        {
            _colorfulConsole.WriteLine();
        }

        /// <summary>
        /// Writes the specified text to the user console.
        /// </summary>
        /// <param name="output">The text to write.</param>
        public void WriteLine(string output)
        {
            _colorfulConsole.WriteLine(output);
        }

        /// <summary>
        /// Adds all command handlers from current class to the commands manager.
        /// </summary>
        private void SetCommandHandlers()
        {
            _commandsManager.AddCommandHandler(CommandType.Help, CommandGroup.ConsoleManager, WriteCommandsList);
            _commandsManager.AddCommandHandler(CommandType.Colors, CommandGroup.ConsoleManager, WriteColorsList);
            _commandsManager.AddCommandHandler(CommandType.Quit, CommandGroup.ConsoleManager, Exit);
        }

        /// <summary>
        /// Runs console loop.
        /// </summary>
        private void Loop()
        {
            while (_loopRunning)
            {
                ProcessCommand(Console.ReadLine());
            }
        }

        /// <summary>
        /// Processes all incoming commands from user console.
        /// </summary>
        /// <param name="userInput">The user input to parse.</param>
        private void ProcessCommand(string userInput)
        {
            var executionResult = _commandsManager.Execute(userInput);
            switch (executionResult)
            {
                case ExecutionResult.Success:
                {
                    break;
                }

                case ExecutionResult.EmptyCommand:
                {
                    WriteEmptyCommandMessage();
                    break;
                }

                case ExecutionResult.CommandNotFound:
                {
                    WriteCommandNotFoundMessage();
                    break;
                }

                case ExecutionResult.InvalidCommandFormat:
                {
                    WriteInvalidCommandFormatMessage();
                    break;
                }
            }
        }

        /// <summary>
        /// Writes empty command message to the user console.
        /// </summary>
        private void WriteEmptyCommandMessage()
        {
            WriteLine("$rEmpty command");
        }

        /// <summary>
        /// Writes command not found message to the user console.
        /// </summary>
        private void WriteCommandNotFoundMessage()
        {
            WriteLine("$rCommand not found");
        }

        /// <summary>
        /// Writes invalid command format message to the user console.
        /// </summary>
        private void WriteInvalidCommandFormatMessage()
        {
            WriteLine("$rInvalid command format");
        }

        /// <summary>
        /// Writes a list of all commands to the user console.
        /// </summary>
        /// <param name="command">The command instance with more specified data.</param>
        private void WriteCommandsList(Command command)
        {
            WriteLine($"$wAvailable commands ({_commandDefinitionsContainer.Definitions.Count}):");

            foreach (var commandDefinition in _commandDefinitionsContainer.Definitions)
            {
                WriteLine($"$g{commandDefinition.Name}$w - {commandDefinition.Description}");

                foreach (var argument in commandDefinition.Arguments)
                {
                    WriteLine($"$c  <{argument.Type}>$w - {argument.Description}");
                }
            }
        }

        /// <summary>
        /// Writes a list of all colors to the user console.
        /// </summary>
        /// <param name="command">The command instance with more specified data.</param>
        private void WriteColorsList(Command command)
        {
            WriteLine($"$wAvailable colors ({ColorDefinitions.Colors.Count}):");

            foreach (var colorDefinition in ColorDefinitions.Colors)
            {
                WriteLine($"$w - ${colorDefinition.Key}{colorDefinition.Value} - {colorDefinition.Key}");
            }
        }

        /// <summary>
        /// Ends console loop and exits.
        /// </summary>
        /// <param name="command">The command instance with more specified data.</param>
        private void Exit(Command command)
        {
            _loopRunning = false;
        }
    }
}