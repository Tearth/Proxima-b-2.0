using System.Collections.Generic;
using CECP.App.ConsoleSubsystem;
using CECP.App.GameSubsystem.Modes.Game.Moves;
using Helpers.Loggers.CSV;
using Proxima.Core.AI;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;
using Proxima.Core.Session;

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

        private Dictionary<string, GameResult> _gameResultTokens;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameMode"/> class.
        /// </summary>
        public GameMode()
        {
            _gameSession = new GameSession();
            _gameSession.OnThinkingOutput += GameSession_OnThinkingOutput;

            _csvLogger = new CsvLogger(AILogsDirectory);

            _thinkingOutputEnabled = false;

            _engineColor = Color.Black;
            _enemyColor = Color.White;

            _gameResultTokens = new Dictionary<string, GameResult>
            {
                { "1-0", GameResult.WhiteWon },
                { "0-1", GameResult.BlackWon },
                { "1/2-1/2", GameResult.Draw }
            };

            CommandsManager.AddCommandHandler(CommandType.Post, ExecutePostCommand);
            CommandsManager.AddCommandHandler(CommandType.NoPost, ExecuteNoPostCommand);
            CommandsManager.AddCommandHandler(CommandType.Time, ExecuteTimeCommand);
            CommandsManager.AddCommandHandler(CommandType.OTim, ExecuteOTimCommand);
            CommandsManager.AddCommandHandler(CommandType.White, ExecuteWhiteCommand);
            CommandsManager.AddCommandHandler(CommandType.Black, ExecuteBlackCommand);
            CommandsManager.AddCommandHandler(CommandType.Go, ExecuteGoCommand);
            CommandsManager.AddCommandHandler(CommandType.UserMove, ExecuteUserMoveCommand);
            CommandsManager.AddCommandHandler(CommandType.Result, ExecuteResultCommand);
        }

        /// <summary>
        /// Processes message (done in derived class) and prepares a response.
        /// </summary>
        /// <param name="command">The command to process.</param>
        public override void ProcessCommand(Command command)
        {
            CommandsManager.Execute(command);
        }

        /// <summary>
        /// The event handler for OnThinkingOutput.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void GameSession_OnThinkingOutput(object sender, ThinkingOutputEventArgs e)
        {
            if (_thinkingOutputEnabled)
            {
                var depth = e.AIResult.Depth;
                var score = e.AIResult.Score;
                var time = (int)(e.AIResult.Time * 100);
                var totalNodes = e.AIResult.Stats.TotalNodes;

                var pvNodes = string.Empty;
                foreach (var pvNode in e.AIResult.PVNodes)
                {
                    pvNodes += pvNode + " ";
                }

                SendData($"{depth} {score} {time} {totalNodes} {pvNodes}");
            }
        }

        /// <summary>
        /// Executes Post command (enables thinking output).
        /// </summary>
        /// <param name="command">The Post command to execute.</param>
        private void ExecutePostCommand(Command command)
        {
            _thinkingOutputEnabled = true;
        }

        /// <summary>
        /// Executes NoPost command (disables thinking output).
        /// </summary>
        /// <param name="command">The NoPost command to execute.</param>
        private void ExecuteNoPostCommand(Command command)
        {
            _thinkingOutputEnabled = false;
        }

        /// <summary>
        /// Executes Time command (sets engine time to the specified variable).
        /// </summary>
        /// <param name="command">The Time command to execute.</param>
        private void ExecuteTimeCommand(Command command)
        {
            var time = command.GetArgument<int>(0) / 100;
            _gameSession.UpdateRemainingTime(_engineColor, time);
        }

        /// <summary>
        /// Executes OTim command (sets opponent time to the specified variable).
        /// </summary>
        /// <param name="command">The OTim command to execute.</param>
        private void ExecuteOTimCommand(Command command)
        {
            var time = command.GetArgument<int>(0) / 100;
            _gameSession.UpdateRemainingTime(_enemyColor, time);
        }

        /// <summary>
        /// Executes White command (sets engine color to white).
        /// </summary>
        /// <param name="command">The White command to execute.</param>
        private void ExecuteWhiteCommand(Command command)
        {
            _engineColor = Color.White;
            _enemyColor = Color.Black;
        }

        /// <summary>
        /// Executes Black command (sets engine color to black).
        /// </summary>
        /// <param name="command">The Black command to execute.</param>
        private void ExecuteBlackCommand(Command command)
        {
            _engineColor = Color.Black;
            _enemyColor = Color.White;
        }

        /// <summary>
        /// Executes Go command (runs AI and does best move).
        /// </summary>
        /// <param name="command">The Go command to execute.</param>
        private void ExecuteGoCommand(Command command)
        {
            var aiResponse = CalculateAIMove();
            SendData($"move {aiResponse}");
        }

        /// <summary>
        /// Executes UserMove command (applies enemy move and runs AI).
        /// </summary>
        /// <param name="command">The UserMove command to execute.</param>
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
        /// Executes Result command (ends game with the specified result).
        /// </summary>
        /// <param name="command">The Result command to execute.</param>
        private void ExecuteResultCommand(Command command)
        {
            var gameResultArgument = command.GetArgument<string>(0);
            var gameResult = _gameResultTokens[gameResultArgument];

            _csvLogger.WriteLine(gameResult, _engineColor);
        }

        /// <summary>
        /// Applies enemy move to the bitboard.
        /// </summary>
        /// <param name="cecpMove">The CECP move to apply.</param>
        private void CalculateEnemyMove(CECPMove cecpMove)
        {
            if (cecpMove.PromotionPiece.HasValue)
            {
                _gameSession.Move(_enemyColor, cecpMove.From, cecpMove.To, cecpMove.PromotionPiece.Value);
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

            var fromConverted = PositionConverter.ToString(aiResult.PVNodes[0].From);
            var toConverted = PositionConverter.ToString(aiResult.PVNodes[0].To);

            _csvLogger.WriteLine(aiResult, _gameSession.Bitboard);
            return $"{fromConverted}{toConverted}";
        }
    }
}
