using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CECP.App.ConsoleSubsystem;

namespace CECP.App
{
    public class CECPCore
    {
        private ConsoleManager _consoleManager;

        public CECPCore()
        {
            _consoleManager = new ConsoleManager();
        }

        public void Run()
        {
            var command = _consoleManager.WaitForCommand();
        }
    }
}
