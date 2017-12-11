using GUI.ColorfulConsole;
using Proxima.FICS.Source.ConfigSubsystem;

namespace Proxima.FICS.Source
{
    public class FICSCore
    {
        private ColorfulConsoleManager _consoleManager;
        private ConfigManager _configManager;

        public FICSCore(ColorfulConsoleManager consoleManager, ConfigManager configManager)
        {
            _consoleManager = consoleManager;
            _configManager = configManager;
        }
        
        public void Run()
        {

        }
    }
}
