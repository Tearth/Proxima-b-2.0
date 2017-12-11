using System;
using GUI.ColorfulConsole;
using Proxima.FICS.Source.ConfigSubsystem;
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
        }

        /// <summary>
        /// Opens FICS session and runs games.
        /// </summary>
        public void Run()
        {
            _ficsClient.OpenSession();
            LogIn();
        }

        /// <summary>
        /// The event handler for OnDataReceive.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void FicsClient_OnDataReceive(object sender, DataReceivedEventArgs e)
        {
            _consoleManager.Write($"$c{e.Text}");
        }

        /// <summary>
        /// The event handler for OnDataSend.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void FicsClient_OnDataSend(object sender, DataSentEventArgs e)
        {
            _consoleManager.WriteLine($"$g{e.Text}");
        }

        /// <summary>
        /// Sends username and password to the server.
        /// </summary>
        private void LogIn()
        {
            var username = _configManager.GetValue<string>("Username");
            var password = _configManager.GetValue<string>("Password");

            _ficsClient.Send($"{username}\r\n{password}");
        }
    }
}
