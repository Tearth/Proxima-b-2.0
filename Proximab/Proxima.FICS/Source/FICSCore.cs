using System;
using System.Threading;
using GUI.ColorfulConsole;
using Proxima.FICS.Source.ConfigSubsystem;
using Proxima.FICS.Source.FICSModes;
using Proxima.FICS.Source.NetworkSubsystem;

namespace Proxima.FICS.Source
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

        /// <summary>
        /// Initializes a new instance of the <see cref="FICSCore"/> class.
        /// </summary>
        /// <param name="consoleManager">The console manager.</param>
        /// <param name="configManager">The configuration manager.</param>
        public FICSCore(ColorfulConsoleManager consoleManager, ConfigManager configManager)
        {
            _consoleManager = consoleManager;
            _configManager = configManager;

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
            _consoleManager.WriteLine($"$RRECV: $c{e.Text}");

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
            _consoleManager.WriteLine($"$RSEND: $r{e.Text}");
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
            _consoleManager.WriteLine($"$GPRXB: $gMode changed to {modeType}.");

            var ficsModeFactory = new FICSModeFactory(_configManager);

            _ficsMode = ficsModeFactory.Create(modeType);
            _ficsMode.OnChangeMode += FICSMode_OnChangeMode;
        }      
    }
}
