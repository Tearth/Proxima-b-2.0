using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CECP.App.ConsoleSubsystem;
using Proxima.Core.Commons.Colors;

namespace CECP.App.GameSubsystem.Modes
{
    public class GameMode : CECPModeBase
    {
        private bool _thinkingOutputEnabled;
        private Color _engineColor;

        public GameMode(ConsoleManager consoleManager) : base(consoleManager)
        {
            _thinkingOutputEnabled = false;
            _engineColor = Color.White;
        }

        public override void ProcessCommand(Command command)
        {
            switch(command.Type)
            {
                case CommandType.Post:
                {
                    SetThinkingOutput(true);
                    break;
                }

                case CommandType.NoPost:
                {
                    SetThinkingOutput(false);
                    break;
                }

                case CommandType.White:
                {
                    SetEngineColor(Color.White);
                    break;
                }

                case CommandType.Black:
                {
                    SetEngineColor(Color.Black);
                    break;
                }

                case CommandType.Go:
                {
                    break;
                }
            }

            base.ProcessCommand(command);
        }

        private void SetThinkingOutput(bool state)
        {
            _thinkingOutputEnabled = state;
        }

        private void SetEngineColor(Color engineColor)
        {
            _engineColor = engineColor;
        }
    }
}
