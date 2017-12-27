using CECP.App.ConsoleSubsystem;
using CECP.App.GameSubsystem;
using Helpers.Loggers.Text;

namespace CECP.App
{
    /// <summary>
    /// Represents a set of methods to manage a game using Chess Engine Communication Protocol.
    /// </summary>
    public class CECPCore
    {
        private TextLogger _textLogger;
        private ConsoleManager _consoleManager;
        private CECPModeBase _cecpMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="CECPCore"/> class.
        /// </summary>
        /// <param name="textLogger">The text logger.</param>
        public CECPCore(TextLogger textLogger)
        {
            _textLogger = textLogger;

            _consoleManager = new ConsoleManager(_textLogger);

            ChangeMode(CECPModeType.Init);
        }

        /// <summary>
        /// Runs main loop (waits for commands from CECP interface, executes commands and sends responses).
        /// </summary>
        public void Run()
        {
            while (true)
            {
                var command = _consoleManager.WaitForCommand();
                _cecpMode.ProcessCommand(command);
            }
        }

        /// <summary>
        /// The event handler for OnSendData.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void CECPMode_OnSendData(object sender, SendDataEventArgs e)
        {
            _consoleManager.WriteLine(e.Text);
        }

        /// <summary>
        /// The event handler for OnChangeMode.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void CECPMode_OnChangeMode(object sender, ChangeModeEventArgs e)
        {
            ChangeMode(e.NewModeType);
        }

        /// <summary>
        /// Changes mode to the specified one and logs it on the console.
        /// </summary>
        /// <param name="modeType">The CECP mode type.</param>
        private void ChangeMode(CECPModeType modeType)
        {
            _textLogger.WriteLine($"{CECPConstants.EnginePrefix}: Mode changed to {modeType}.");

            var ficsModeFactory = new CECPModeFactory();

            _cecpMode = ficsModeFactory.Create(modeType);
            _cecpMode.OnSendData += CECPMode_OnSendData;
            _cecpMode.OnChangeMode += CECPMode_OnChangeMode;
        }
    }
}
