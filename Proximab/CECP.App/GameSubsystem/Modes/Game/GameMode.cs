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
using Proxima.Core.Session;
using Proxima.Core.Time;

namespace CECP.App.GameSubsystem.Modes.Game
{
    /// <summary>
    /// Represents the CECP game mode. All AI calculations and interactions with enemies will be done here.
    /// </summary>
    public class GameMode : CECPModeBase
    {
        private const string AILogsDirectory = "AILogs";

        private GameSession _gameSession;
        private CsvLogger _csvLogger;

        private bool _thinkingOutputEnabled;

        private Color _engineColor;
        private Color _enemyColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameMode"/> class.
        /// </summary>
        public GameMode() : base()
        {
            _gameSession = new GameSession();
            _csvLogger = new CsvLogger(AILogsDirectory);

            _thinkingOutputEnabled = false;

            _engineColor = Color.Black;
            _enemyColor = Color.White;

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
            _gameSession.UpdateRemainingTime(_engineColor, time);
        }

        /// <summary>
        /// Executes OTim command (sets opponent time to the specified variable).
        /// </summary>
        /// <param name="command">The OTim command to execute.</param>
        /// <returns>The response (<see cref="string.Empty"/> if none).</returns>
        private void ExecuteOTimCommand(Command command)
        {
            var time = command.GetArgument<int>(0) / 100;
            _gameSession.UpdateRemainingTime(_enemyColor, time);
        }

        /// <summary>
        /// Executes White command (sets engine color to white).
        /// </summary>
        /// <param name="command">The White command to execute.</param>
        /// <returns>The response (<see cref="string.Empty"/> if none).</returns>
        private void ExecuteWhiteCommand(Command command)
        {
            _engineColor = Color.White;
            _enemyColor = Color.Black;
        }

        /// <summary>
        /// Executes Black command (sets engine color to black).
        /// </summary>
        /// <param name="command">The Black command to execute.</param>
        /// <returns>The response (<see cref="string.Empty"/> if none).</returns>
        private void ExecuteBlackCommand(Command command)
        {
            _engineColor = Color.Black;
            _enemyColor = Color.White;
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
            if (cecpMove.PromotionPiece.HasValue)
            {
                _gameSession.Move(_enemyColor, cecpMove.From, cecpMove.To,
                                  cecpMove.PromotionPiece.Value);
            }
            else
            {
                _gameSession.Move(_enemyColor, cecpMove.From, cecpMove.To);
            }
        }

        /// <summary>
        /// Runs AI calculation and applies best move to the bitboard.
        /// </summary>
        /// <returns>The response (best move).</returns>
        private string CalculateAIMove()
        {
            var aiResult = _gameSession.MoveAI(_engineColor);

            var fromConverted = PositionConverter.ToString(aiResult.BestMove.From);
            var toConverted = PositionConverter.ToString(aiResult.BestMove.To);

            _csvLogger.WriteLine(aiResult, _gameSession.Bitboard);
            return $"{fromConverted}{toConverted}";
        }
    }
}
