using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CECP.App.ConsoleSubsystem;
using Helpers.Loggers.Text;

namespace CECP.App
{
    public class CECPCore
    {
        private TextLogger _textLogger;
        private ConsoleManager _consoleManager;

        public CECPCore(TextLogger textLogger)
        {
            _textLogger = textLogger;

            _consoleManager = new ConsoleManager(_textLogger);
        }

        public void Run()
        {
            while(true)
            {
                var command = _consoleManager.WaitForCommand();
            }
        }
    }
}
