using FICS.App.ConfigSubsystem;
using FICS.App.GameSubsystem;
using FICS.App.NetworkSubsystem;
using Helpers.ColorfulConsole;
using Helpers.Loggers.Text;

namespace FICS.App
{
    /// <summary>
    /// Represents a set of methods to manage a game with FICS.
    /// </summary>
    public class FICSCore
    {
        private ColorfulConsoleManager _consoleManager;
        private ConfigManager _configManager;
        private FICSClient _ficsClient;
        private FICSModeBase _ficsMode;
        private TextLogger _textLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FICSCore"/> class.
        /// </summary>
        /// <param name="consoleManager">The console manager.</param>
        /// <param name="configManager">The configuration manager.</param>
        /// <param name="textLogger">The text logger.</param>
        public FICSCore(ColorfulConsoleManager consoleManager, ConfigManager configManager, TextLogger textLogger)
        {
            _consoleManager = consoleManager;
            _configManager = configManager;
            _textLogger = textLogger;

            _ficsClient = new FICSClient(_configManager);
            _ficsClient.OnDataReceive += FicsClient_OnDataReceive;
            _ficsClient.OnDataSend += FicsClient_OnDataSend;

            ChangeMode(FICSModeType.Auth);
        }

        /// <summary>
        /// Opens FICS session and runs games.
        /// </summary>
        public void Run()
        {
            _ficsClient.OpenSession();
        }

        /// <summary>
        /// The event handler for OnDataReceive.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void FicsClient_OnDataReceive(object sender, DataReceivedEventArgs e)
        {
            // FICS can sometimes send text with false '$' character which can cause exception due to
            // unrecognised color symbol.
            var textWithoutFalseColorSymbols = e.Text.Replace("$", "");

            _consoleManager.WriteLine($"$R{FICSConstants.ReceivePrefix}: $c{textWithoutFalseColorSymbols}");
            _textLogger.WriteLine($"{FICSConstants.ReceivePrefix}: {e.Text}");

            _ficsMode.ProcessMessage(e.Text);
        }

        /// <summary>
        /// The event handler for OnDataSend.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void FicsClient_OnDataSend(object sender, DataSentEventArgs e)
        {
            _consoleManager.WriteLine($"$R{FICSConstants.SendPrefix}: $r{e.Text}");
            _textLogger.WriteLine($"{FICSConstants.SendPrefix}: {e.Text}");
        }

        /// <summary>
        /// The event handler for OnSendData.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void FICSMode_OnSendData(object sender, SendDataEventArgs e)
        {
            _ficsClient.Send(e.Text);
        }

        /// <summary>
        /// The event handler for OnChangeMode.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void FICSMode_OnChangeMode(object sender, ChangeModeEventArgs e)
        {
            if (_ficsMode != null)
            {
                _ficsMode.OnSendData -= FICSMode_OnSendData;
                _ficsMode.OnChangeMode -= FICSMode_OnChangeMode;
            }

            ChangeMode(e.NewModeType);
        }

        /// <summary>
        /// Changes mode to the specified one and logs it on the console.
        /// </summary>
        /// <param name="modeType">The FICS mode type.</param>
        private void ChangeMode(FICSModeType modeType)
        {
            _consoleManager.WriteLine($"$G{FICSConstants.EnginePrefix}: $gMode changed to {modeType}.");
            _textLogger.WriteLine($"{FICSConstants.EnginePrefix}: Mode changed to {modeType}.");

            var ficsModeFactory = new FICSModeFactory(_configManager);

            _ficsMode = ficsModeFactory.Create(modeType);
            _ficsMode.OnSendData += FICSMode_OnSendData;
            _ficsMode.OnChangeMode += FICSMode_OnChangeMode;
        }
    }
}
