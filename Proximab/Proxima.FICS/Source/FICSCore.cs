using GUI.ColorfulConsole;
using Proxima.FICS.Source.ConfigSubsystem;
using Proxima.FICS.Source.GameSubsystem;
using Proxima.FICS.Source.LogSubsystem;
using Proxima.FICS.Source.NetworkSubsystem;

namespace Proxima.FICS.Source
{
    /// <summary>
    /// Represents a set of methods to manage a game with FICS.
    /// </summary>
    public class FICSCore
    {
        private const string SendPrefix = "SEND";
        private const string ReceivePrefix = "RECV";
        private const string EnginePrefix = "PRXB";

        private ColorfulConsoleManager _consoleManager;
        private ConfigManager _configManager;
        private FICSClient _ficsClient;
        private FICSModeBase _ficsMode;
        private LogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="FICSCore"/> class.
        /// </summary>
        /// <param name="consoleManager">The console manager.</param>
        /// <param name="configManager">The configuration manager.</param>
        /// <param name="logWriter">The log writer.</param>
        public FICSCore(ColorfulConsoleManager consoleManager, ConfigManager configManager, LogWriter logWriter)
        {
            _consoleManager = consoleManager;
            _configManager = configManager;
            _logWriter = logWriter;

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
        private void FicsClient_OnDataReceive(object sender, DataEventArgs e)
        {
            _consoleManager.WriteLine($"$R{ReceivePrefix}: $c{e.Text}");
            _logWriter.WriteLine($"{ReceivePrefix}: {e.Text}");

            var response = _ficsMode.ProcessMessage(e.Text);
            if (response != string.Empty)
            {
                _ficsClient.Send(response);
            }
        }

        /// <summary>
        /// The event handler for OnDataSend.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void FicsClient_OnDataSend(object sender, DataEventArgs e)
        {
            _consoleManager.WriteLine($"$R{SendPrefix}: $r{e.Text}");
            _logWriter.WriteLine($"{SendPrefix}: {e.Text}");
        }

        /// <summary>
        /// The event handler for OnChangeMode.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void FICSMode_OnChangeMode(object sender, ChangeModeEventArgs e)
        {
            ChangeMode(e.NewModeType);
        }

        /// <summary>
        /// Changes mode to the specified one and logs it on the console.
        /// </summary>
        /// <param name="modeType">The FICS mode type.</param>
        private void ChangeMode(FICSModeType modeType)
        {
            _consoleManager.WriteLine($"$G{EnginePrefix}: $gMode changed to {modeType}.");
            _logWriter.WriteLine($"{EnginePrefix}: Mode changed to {modeType}.");

            var ficsModeFactory = new FICSModeFactory(_configManager);

            _ficsMode = ficsModeFactory.Create(modeType);
            _ficsMode.OnChangeMode += FICSMode_OnChangeMode;
        }      
    }
}
