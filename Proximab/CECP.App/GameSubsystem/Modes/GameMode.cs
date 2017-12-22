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

        private int _engineTime;
        private int _opponentTime;

        public GameMode(ConsoleManager consoleManager) : base(consoleManager)
        {
            _thinkingOutputEnabled = false;
            _engineColor = Color.White;

            _engineTime = 0;
            _opponentTime = 0;
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

                case CommandType.Time:
                {
                    SetEngineTime(command);
                    break;
                }

                case CommandType.OTim:
                {
                    SetOpponentTime(command);
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

        private void SetEngineTime(Command command)
        {
            var time = command.GetArgument<int>(0) / 100;
            _engineTime = time;
        }

        private void SetOpponentTime(Command command)
        {
            var time = command.GetArgument<int>(0) / 100;
            _opponentTime = time;
        }
    }
}
