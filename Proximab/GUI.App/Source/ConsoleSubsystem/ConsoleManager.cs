using System;
using System.Linq;
using System.Threading.Tasks;
using ColorfulConsole;
using GUI.App.Source.CommandsSubsystem;
using GUI.App.Source.DiagnosticSubsystem;
using GUI.ColorfulConsole;
using GUI.ContentDefinitions.Commands;
using Microsoft.Xna.Framework.Content;

namespace GUI.App.Source.ConsoleSubsystem
{
    /// <summary>
    /// Represents a set of methods to manage a console.
    /// </summary>
    internal class ConsoleManager
    {
        private ColorfulConsoleManager _colorfulConsole;

        private Task _consoleLoop;
        private CommandsManager _commandsManager;
        private EnvironmentInfoProvider _environmentInfoProvider;

        private CommandDefinitionsContainer _commandDefinitionsContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleManager"/> class.
        /// </summary>
        /// <param name="commandsManager">The commands manager instance.</param>
        public ConsoleManager(CommandsManager commandsManager)
        {
            _commandsManager = commandsManager;
            _consoleLoop = new Task(() => Loop());

            _colorfulConsole = new ColorfulConsoleManager();
            _environmentInfoProvider = new EnvironmentInfoProvider();

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

            WriteConsoleHeader();
        }

        /// <summary>
        /// Runs asynchronously the task with console loop.
        /// </summary>
        public async void RunAsync()
        {
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
        /// Runs console loop.
        /// </summary>
        private void Loop()
        {
            while (true)
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
            WriteLine($"$rCommand not found");
        }

        /// <summary>
        /// Writes invalid command format message to the user console.
        /// </summary>
        private void WriteInvalidCommandFormatMessage()
        {
            WriteLine($"$rInvalid command format");
        }

        /// <summary>
        /// Writes header to the user console (should be called only once at program startup).
        /// </summary>
        private void WriteConsoleHeader()
        {
            var osInfo = _environmentInfoProvider.OSInfo;
            var cpuPlatform = _environmentInfoProvider.CPUPlatformVersion;
            var processPlatform = _environmentInfoProvider.ProcessPlatformVersion;
            var coresCount = _environmentInfoProvider.CPUCoresCount;

            WriteLine($"$gProxima b 2.0dev GUI$w | {osInfo} " +
                      $"(CPU $c{cpuPlatform}$w, {coresCount}$w | Process $c{processPlatform}$w)");
        }
    }
}