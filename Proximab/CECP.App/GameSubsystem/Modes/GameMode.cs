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

            CommandsManager.AddCommandHandler(CommandType.Post, ExecutePostCommand);
            CommandsManager.AddCommandHandler(CommandType.NoPost, ExecuteNoPostCommand);
            CommandsManager.AddCommandHandler(CommandType.Time, ExecuteTimeCommand);
            CommandsManager.AddCommandHandler(CommandType.OTim, ExecuteOTimCommand);
            CommandsManager.AddCommandHandler(CommandType.White, ExecuteWhiteCommand);
            CommandsManager.AddCommandHandler(CommandType.Black, ExecuteBlackCommand);
            CommandsManager.AddCommandHandler(CommandType.Go, ExecuteGoCommand);
            CommandsManager.AddCommandHandler(CommandType.UserMove, ExecuteUserMoveCommand);
        }

        public override string ProcessCommand(Command command)
        {
            return CommandsManager.Execute(command);
        }

        private string ExecutePostCommand(Command command)
        {
            _thinkingOutputEnabled = true;
            return string.Empty;
        }

        private string ExecuteNoPostCommand(Command command)
        {
            _thinkingOutputEnabled = false;
            return string.Empty;
        }

        private string ExecuteTimeCommand(Command command)
        {
            var time = command.GetArgument<int>(0) / 100;
            _engineTime = time;

            return string.Empty;
        }

        private string ExecuteOTimCommand(Command command)
        {
            var time = command.GetArgument<int>(0) / 100;
            _opponentTime = time;

            return string.Empty;
        }

        private string ExecuteWhiteCommand(Command command)
        {
            _engineColor = Color.White;
            return string.Empty;
        }

        private string ExecuteBlackCommand(Command command)
        {
            _engineColor = Color.Black;
            return string.Empty;
        }

        private string ExecuteGoCommand(Command command)
        {
            return string.Empty;
        }

        private string ExecuteUserMoveCommand(Command command)
        {
            return string.Empty;
        }
    }
}
