using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CECP.App.ConsoleSubsystem;

namespace CECP.App.GameSubsystem.Modes
{
    public class GameMode : CECPModeBase
    {
        public GameMode(ConsoleManager consoleManager) : base(consoleManager)
        {
        }

        public override void ProcessCommand(Command command)
        {
            base.ProcessCommand(command);
        }
    }
}
