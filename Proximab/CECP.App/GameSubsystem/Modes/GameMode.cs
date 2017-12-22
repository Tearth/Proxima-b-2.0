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

        public GameMode() : base()
        {
            _thinkingOutputEnabled = false;
            _engineColor = Color.White;

            _engineTime = 0;
            _opponentTime = 0;
        }

        public override string ProcessCommand(Command command)
        {
            switch(command.Type)
            {
                case CommandType.Post: return SetThinkingOutput(true);
                case CommandType.NoPost: return SetThinkingOutput(false);
                case CommandType.Time: return SetEngineTime(command);
                case CommandType.OTim: return SetOpponentTime(command);  
                case CommandType.White: return SetEngineColor(Color.White);
                case CommandType.Black: return SetEngineColor(Color.Black);
                case CommandType.Go: return string.Empty;
                case CommandType.UserMove: return string.Empty;
            }

            return base.ProcessCommand(command);
        }

        private string SetThinkingOutput(bool state)
        {
            _thinkingOutputEnabled = state;
            return string.Empty;
        }

        private string SetEngineColor(Color engineColor)
        {
            _engineColor = engineColor;
            return string.Empty;
        }

        private string SetEngineTime(Command command)
        {
            var time = command.GetArgument<int>(0) / 100;
            _engineTime = time;

            return string.Empty;
        }

        private string SetOpponentTime(Command command)
        {
            var time = command.GetArgument<int>(0) / 100;
            _opponentTime = time;

            return string.Empty;
        }
    }
}
