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
        private bool _thinkingOutputEnabled;

        public GameMode(ConsoleManager consoleManager) : base(consoleManager)
        {
            _thinkingOutputEnabled = false;
        }

        public override void ProcessCommand(Command command)
        {
            switch(command.Type)
            {
                case CommandType.Post:
                {
                    EnableThinkingOutput();
                    break;
                }

                case CommandType.NoPost:
                {
                    DisableThinkingOutput();
                    break;
                }
            }

            base.ProcessCommand(command);
        }

        private void EnableThinkingOutput()
        {
            _thinkingOutputEnabled = true;
        }

        private void DisableThinkingOutput()
        {
            _thinkingOutputEnabled = false;
        }
    }
}
