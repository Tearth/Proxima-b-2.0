using GUI.App.Source.CommandsSubsystem;
using GUI.App.Source.ConsoleSubsystem;
using GUI.App.Source.GameModeSubsystem.Editor;
using GUI.App.Source.GameModeSubsystem.Exceptions;

namespace GUI.App.Source.GameModeSubsystem
{
    /// <summary>
    /// Represents a factory of modes.
    /// </summary>
    internal class ModeFactory
    {
        private ConsoleManager _consoleManager;
        private CommandsManager _commandsManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModeFactory"/> class.
        /// </summary>
        /// <param name="consoleManager">The console manager.</param>
        /// <param name="commandsManager">The commands manager.</param>
        public ModeFactory(ConsoleManager consoleManager, CommandsManager commandsManager)
        {
            _consoleManager = consoleManager;
            _commandsManager = commandsManager;
        }

        /// <summary>
        /// Creates a new instance of the mode specified in the parameter.
        /// </summary>
        /// <param name="modeType">The mode type.</param>
        /// <returns>The mode instance.</returns>
        public ModeBase Create(ModeType modeType)
        {
            switch (modeType)
            {
                case ModeType.Editor: return new EditorMode(_consoleManager, _commandsManager);
                case ModeType.AIvsAI: return new AIvsAIMode(_consoleManager, _commandsManager);
            }

            throw new ModeNotFoundException();
        }
    }
}
