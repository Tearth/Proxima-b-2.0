using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CECP.App.ConsoleSubsystem;
using CECP.App.GameSubsystem;
using Helpers.Loggers.Text;

namespace CECP.App
{
    public class CECPCore
    {
        private TextLogger _textLogger;
        private ConsoleManager _consoleManager;
        private CECPModeBase _cecpMode;

        public CECPCore(TextLogger textLogger)
        {
            _textLogger = textLogger;

            _consoleManager = new ConsoleManager(_textLogger);

            ChangeMode(CECPModeType.Init);
        }

        public void Run()
        {
            while(true)
            {
                var command = _consoleManager.WaitForCommand();
                var response = _cecpMode.ProcessCommand(command);

                if(response != string.Empty)
                {
                    _consoleManager.WriteLine(response);
                }
            }
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
            _textLogger.WriteLine($"PRXB: Mode changed to {modeType}.");

            var ficsModeFactory = new CECPModeFactory();

            _cecpMode = ficsModeFactory.Create(modeType);
            _cecpMode.OnChangeMode += CECPMode_OnChangeMode;
        }
    }
}
