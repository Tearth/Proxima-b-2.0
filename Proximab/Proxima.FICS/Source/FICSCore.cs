using System;
using GUI.ColorfulConsole;
using Proxima.FICS.Source.ConfigSubsystem;
using Proxima.FICS.Source.NetworkSubsystem;

namespace Proxima.FICS.Source
{
    public class FICSCore
    {
        private ColorfulConsoleManager _consoleManager;
        private ConfigManager _configManager;
        private FICSClient _ficsClient;

        public FICSCore(ColorfulConsoleManager consoleManager, ConfigManager configManager)
        {
            _consoleManager = consoleManager;
            _configManager = configManager;

            _ficsClient = new FICSClient(_configManager);
            _ficsClient.OnDataReceive += FicsClient_OnDataReceive;
        }

        public void Run()
        {
            _ficsClient.OpenSession();
            LogIn();
        }

        private void FicsClient_OnDataReceive(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine($"{e.Text}");
        }

        private void LogIn()
        {
            var username = _configManager.GetValue<string>("Username");
            var password = _configManager.GetValue<string>("Password");

            _ficsClient.Send($"{username}\r\n{password}");
        }
    }
}
