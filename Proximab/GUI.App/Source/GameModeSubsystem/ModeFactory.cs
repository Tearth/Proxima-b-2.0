using GUI.App.Source.CommandsSubsystem;
using GUI.App.Source.ConsoleSubsystem;
using GUI.App.Source.GameModeSubsystem.Editor;
using GUI.App.Source.GameModeSubsystem.Exceptions;

namespace GUI.App.Source.GameModeSubsystem
{
    internal class ModeFactory
    {
        ConsoleManager _consoleManager;
        CommandsManager _commandsManager;

        public ModeFactory(ConsoleManager consoleManager, CommandsManager commandsManager)
        {
            _consoleManager = consoleManager;
            _commandsManager = commandsManager;
        }

        public ModeBase Create(ModeType modeType)
        {
            switch(modeType)
            {
                case ModeType.Editor: return new EditorMode(_consoleManager, _commandsManager);
                case ModeType.AIvsAI: return new AIvsAIMode(_consoleManager, _commandsManager);
            }

            throw new ModeNotFoundException();
        }
    }
}
