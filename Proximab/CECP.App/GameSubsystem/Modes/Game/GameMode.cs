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
using Proxima.Core.Boards.Exceptions;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators;
using Proxima.Core.MoveGenerators.Moves;
using Proxima.Core.Time;

namespace CECP.App.GameSubsystem.Modes.Game
{
    /// <summary>
    /// Represents the CECP game mode. All AI calculations and interactions with enemies will be done here.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="GameMode"/> class.
        /// </summary>
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

        /// <summary>
        /// Processes message (done in derivied class) and prepares a response.
        /// </summary>
        /// <param name="command">The command to process.</param>
        /// <returns>The response (<see cref="string.Empty"/> if none).</returns>
        public override void ProcessCommand(Command command)
        {
            CommandsManager.Execute(command);
        }

        /// <summary>
        /// Executes Post command (enables thinking output).
        /// </summary>
        /// <param name="command">The Post command to execute.</param>
        /// <returns>The response (<see cref="string.Empty"/> if none).</returns>
        private void ExecutePostCommand(Command command)
        {
            _thinkingOutputEnabled = true;
        }

        /// <summary>
        /// Executes NoPost command (disables thinking output).
        /// </summary>
        /// <param name="command">The NoPost command to execute.</param>
        /// <returns>The response (<see cref="string.Empty"/> if none).</returns>
        private void ExecuteNoPostCommand(Command command)
        {
            _thinkingOutputEnabled = false;
        }

        /// <summary>
        /// Executes Time command (sets engine time to the specified variable).
        /// </summary>
        /// <param name="command">The Time command to execute.</param>
        /// <returns>The response (<see cref="string.Empty"/> if none).</returns>
        private void ExecuteTimeCommand(Command command)
        {
            var time = command.GetArgument<int>(0) / 100;
            _engineTime = time;
        }

        /// <summary>
        /// Executes OTim command (sets opponent time to the specified variable).
        /// </summary>
        /// <param name="command">The OTim command to execute.</param>
        /// <returns>The response (<see cref="string.Empty"/> if none).</returns>
        private void ExecuteOTimCommand(Command command)
        {
            var time = command.GetArgument<int>(0) / 100;
            _opponentTime = time;
        }

        /// <summary>
        /// Executes White command (sets engine color to white).
        /// </summary>
        /// <param name="command">The White command to execute.</param>
        /// <returns>The response (<see cref="string.Empty"/> if none).</returns>
        private void ExecuteWhiteCommand(Command command)
        {
            _engineColor = Color.White;
        }

        /// <summary>
        /// Executes Black command (sets engine color to black).
        /// </summary>
        /// <param name="command">The Black command to execute.</param>
        /// <returns>The response (<see cref="string.Empty"/> if none).</returns>
        private void ExecuteBlackCommand(Command command)
        {
            _engineColor = Color.Black;
        }

        /// <summary>
        /// Executes Go command (runs AI and does best move).
        /// </summary>
        /// <param name="command">The Go command to execute.</param>
        /// <returns>The response (<see cref="string.Empty"/> if none).</returns>
        private void ExecuteGoCommand(Command command)
        {
            var aiResponse = CalculateAIMove();
            SendData($"move {aiResponse}");
        }

        /// <summary>
        /// Executes UserMove command (applies enemy move and runs AI).
        /// </summary>
        /// <param name="command">The UserMove command to execute.</param>
        /// <returns>The response (<see cref="string.Empty"/> if none).</returns>
        private void ExecuteUserMoveCommand(Command command)
        {
            var cecpMoveParser = new CECPMoveParser();

            var moveText = command.GetArgument<string>(0);
            var cecpMove = cecpMoveParser.Parse(moveText);

            CalculateEnemyMove(cecpMove);

            var aiResponse = CalculateAIMove();
            SendData($"move {aiResponse}");
        }

        /// <summary>
        /// Applies enemy move to the bitboard.
        /// </summary>
        /// <param name="cecpMove">The CECP move to apply.</param>
        private void CalculateEnemyMove(CECPMove cecpMove)
        {
            _bitboard.Calculate(GeneratorMode.CalculateMoves, GeneratorMode.CalculateMoves);

            var possibleMovesToApply = _bitboard.Moves.Where(p => p.From == cecpMove.From && p.To == cecpMove.To);
            if (cecpMove.PromotionPiece.HasValue)
            {
                possibleMovesToApply = _bitboard.Moves.Cast<PromotionMove>().Where(p => p.PromotionPiece == cecpMove.PromotionPiece);
            }

            _bitboard = _bitboard.Move(possibleMovesToApply.First());

            if (!_bitboard.VerifyIntegrity())
            {
                throw new BitboardDisintegratedException();
            }
        }

        /// <summary>
        /// Runs AI calculation and applies best move to the bitboard.
        /// </summary>
        /// <returns>The response (best move).</returns>
        private string CalculateAIMove()
        {
            var ai = new AICore();
            var preferredTime = _preferredTimeCalculator.Calculate(_movesCount, _engineTime);

            var aiResult = ai.Calculate(_engineColor, _bitboard, preferredTime);

            _bitboard = _bitboard.Move(aiResult.BestMove);

            if (!_bitboard.VerifyIntegrity())
            {
                throw new BitboardDisintegratedException();
            }

            var fromConverted = PositionConverter.ToString(aiResult.BestMove.From);
            var toConverted = PositionConverter.ToString(aiResult.BestMove.To);

            _csvLogger.WriteLine(aiResult, _bitboard);
            return $"{fromConverted}{toConverted}";
        }
    }
}
