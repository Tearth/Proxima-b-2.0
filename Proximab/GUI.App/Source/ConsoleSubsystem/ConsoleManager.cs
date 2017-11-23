using System;
using System.Linq;
using System.Threading.Tasks;
using GUI.App.Source.ConsoleSubsystem.Parser;
using GUI.App.Source.DiagnosticSubsystem;
using GUI.ColorfulConsole;
using GUI.ContentDefinitions.Colors;
using GUI.ContentDefinitions.Commands;
using Microsoft.Xna.Framework.Content;

namespace GUI.App.Source.ConsoleSubsystem
{
    /// <summary>
    /// Represents a set of methods to manage a console.
    /// </summary>
    internal class ConsoleManager
    {
        /// <summary>
        /// Event triggered when there is a new command from user.
        /// </summary>
        public event EventHandler<NewCommandEventArgs> OnNewCommand;

        private ColorfulConsoleManager _colorfulConsole;

        private Task _consoleLoop;
        private CommandParser _commandParser;
        private CommandValidator _commandValidator;
        private EnvironmentInfoProvider _environmentInfoProvider;

        private CommandDefinitionsContainer _commandDefinitionsContainer;
        private ColorDefinitionsContainer _colorDefinitionsContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleManager"/> class.
        /// </summary>
        public ConsoleManager()
        {
            _consoleLoop = new Task(() => Loop());

            OnNewCommand += ConsoleManager_OnNewCommand;

            _colorfulConsole = new ColorfulConsoleManager();
            _commandParser = new CommandParser();
            _commandValidator = new CommandValidator();
            _environmentInfoProvider = new EnvironmentInfoProvider();
        }

        /// <summary>
        /// Loads resources. Must be called before first use.
        /// </summary>
        /// <param name="contentManager">Monogame content manager.</param>
        public void LoadContent(ContentManager contentManager)
        {
            _commandDefinitionsContainer = contentManager.Load<CommandDefinitionsContainer>("XML\\CommandDefinitions");
            _colorDefinitionsContainer = contentManager.Load<ColorDefinitionsContainer>("XML\\ColorDefinitions");

            _colorfulConsole.LoadContent(_colorDefinitionsContainer);

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
        /// Writes empty line to the output console.
        /// </summary>
        public void WriteLine()
        {
            _colorfulConsole.WriteLine();
        }

        /// <summary>
        /// Writes the specified text to the output console.
        /// </summary>
        /// <param name="output">The text to write.</param>
        public void WriteLine(string output)
        {
            _colorfulConsole.WriteLine(output);
        }

        /// <summary>
        /// The event handler for new commands.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ConsoleManager_OnNewCommand(object sender, NewCommandEventArgs e)
        {
            var command = e.Command;

            switch (command.Type)
            {
                case CommandType.Help: { WriteCommandsList(); break; }
                case CommandType.Colors: { WriteColorsList(); break; }
            }
        }

        /// <summary>
        /// Writes a list of all commands to the output console.
        /// </summary>
        private void WriteCommandsList()
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
        /// Writes a list of all colors to the output console.
        /// </summary>
        private void WriteColorsList()
        {
            WriteLine($"$wAvailable colors ({_colorDefinitionsContainer.Definitions.Count}):");

            foreach (var colorDefinition in _colorDefinitionsContainer.Definitions)
            {
                WriteLine($"$w - ${colorDefinition.Symbol}{colorDefinition.Color} - {colorDefinition.Symbol}");
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
        /// Processes all incoming commands from output console.
        /// </summary>
        /// <param name="input">The console input to parse.</param>
        private void ProcessCommand(string input)
        {
            var rawCommand = _commandParser.Parse(input.ToLower().Trim());
            if (rawCommand == null)
            {
                WriteEmptyCommandMessage();
                return;
            }

            var definition = _commandDefinitionsContainer.Definitions
                .FirstOrDefault(p => p.Name.ToLower().Trim() == rawCommand.Name);

            if (definition == null)
            {
                WriteCommandNotFoundMessage(input);
                return;
            }

            var validationResult = _commandValidator.Validate(rawCommand, definition);
            if (!validationResult)
            {
                WriteInvalidCommandFormatMessage(input);
                return;
            }

            var enumType = (CommandType)Enum.Parse(typeof(CommandType), definition.EnumType);

            var command = new Command(enumType, rawCommand.Arguments);
            var commandEventArgs = new NewCommandEventArgs(DateTime.Now, command);

            OnNewCommand?.Invoke(this, commandEventArgs);
        }

        /// <summary>
        /// Writes empty command message to the output console.
        /// </summary>
        private void WriteEmptyCommandMessage()
        {
            WriteLine("$rEmpty command");
        }

        /// <summary>
        /// Writes command not found message to the output console.
        /// </summary>
        /// <param name="command">The invalid command.</param>
        private void WriteCommandNotFoundMessage(string command)
        {
            WriteLine($"$rCommand not found: {command}");
        }

        /// <summary>
        /// Writes invalid command format message to the output console.
        /// </summary>
        /// <param name="command">The invalid command.</param>
        private void WriteInvalidCommandFormatMessage(string command)
        {
            WriteLine($"$rInvalid command format: {command}");
        }

        /// <summary>
        /// Writes header to the output console (should be called only once at program startup).
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