using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CECP.App.ConsoleSubsystem;
using CECP.App.GameSubsystem.Modes.Game.Moves;
using Helpers.Loggers.CSV;
using Proxima.Core.AI;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators;
using Proxima.Core.MoveGenerators.Moves;
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

        private int _movesCount;
        private int _engineTime;
        private int _opponentTime;

        public GameMode() : base()
        {
            _bitboard = new Bitboard(new DefaultFriendlyBoard());
            _csvLogger = new CsvLogger(AILogsDirectory);
            _preferredTimeCalculator = new PreferredTimeCalculator(60);

            _thinkingOutputEnabled = false;
            _engineColor = Color.Black;

            _movesCount = 0;
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
            var aiResponse = CalculateAIMove();
            return $"move {aiResponse}";
        }

        private string ExecuteUserMoveCommand(Command command)
        {
            var cecpMoveParser = new CECPMoveParser();

            var moveText = command.GetArgument<string>(0);
            var cecpMove = cecpMoveParser.Parse(moveText);

            CalculateEnemyMove(cecpMove);

            var aiResponse = CalculateAIMove();
            return $"move {aiResponse}";
        }

        /// <summary>
        /// Applies enemy move to the bitboard.
        /// </summary>
        /// <param name="style12Container">The data from FICS.</param>
        private void CalculateEnemyMove(CECPMove cecpMove)
        {
            Move moveToApply;
            _bitboard.Calculate(GeneratorMode.CalculateMoves, GeneratorMode.CalculateMoves);
            
            if (cecpMove.PromotionPiece.HasValue)
            {
                moveToApply = _bitboard.Moves.First(p => p.From == cecpMove.From && p.To == cecpMove.To &&
                                                   (p as PromotionMove).PromotionPiece == cecpMove.PromotionPiece);
            }
            else
            {
                moveToApply = _bitboard.Moves.First(p => p.From == cecpMove.From && p.To == cecpMove.To);
            }

            _bitboard = _bitboard.Move(moveToApply);
        }

        /// <summary>
        /// Runs AI calculation and applies best move to the bitboard.
        /// </summary>
        /// <param name="style12Container">The data from FICS.</param>
        /// <returns>The response (best move) to FICS.</returns>
        private string CalculateAIMove()
        {
            var ai = new AICore();
            var preferredTime = _preferredTimeCalculator.Calculate(_movesCount, _engineTime);

            var aiResult = ai.Calculate(_engineColor, new Bitboard(_bitboard), preferredTime);

            _bitboard = _bitboard.Move(aiResult.BestMove);

            var fromConverted = PositionConverter.ToString(aiResult.BestMove.From);
            var toConverted = PositionConverter.ToString(aiResult.BestMove.To);

            _csvLogger.WriteLine(aiResult, _bitboard);
            return $"{fromConverted}{toConverted}";
        }
    }
}
