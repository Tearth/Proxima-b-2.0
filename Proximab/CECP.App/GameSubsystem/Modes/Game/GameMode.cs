using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CECP.App.ConsoleSubsystem;
using Helpers.Loggers.CSV;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Time;

namespace CECP.App.GameSubsystem.Modes.Game
{
    public class GameMode : CECPModeBase
    {
        private const string AILogsDirectory = "AILogs";

        private Bitboard _bitboard;
        private CsvLogger _csvLogger;
        private PreferredTimeCalculator _preferredTimeCalculator;

        private bool _thinkingOutputEnabled;
        private Color _engineColor;

        private int _engineTime;
        private int _opponentTime;

        public GameMode() : base()
        {
            _bitboard = new Bitboard(new DefaultFriendlyBoard());
            _csvLogger = new CsvLogger(AILogsDirectory);
            _preferredTimeCalculator = new PreferredTimeCalculator(60);

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
