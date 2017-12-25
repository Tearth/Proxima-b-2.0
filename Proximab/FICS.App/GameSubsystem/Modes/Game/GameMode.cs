using System.Collections.Generic;
using System.Linq;
using FICS.App.ConfigSubsystem;
using FICS.App.GameSubsystem.Modes.Game.Style12;
using Helpers.Loggers.CSV;
using Proxima.Core.AI;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Exceptions;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators;
using Proxima.Core.MoveGenerators.Moves;
using Proxima.Core.Session;
using Proxima.Core.Time;

namespace FICS.App.GameSubsystem.Modes.Game
{
    /// <summary>
    /// Represents the FICS game mode. All AI calculations and interactions with enemies will be done here.
    /// </summary>
    public class GameMode : FICSModeBase
    {
        private const string AILogsDirectory = "AILogs";
        private const string CreatingPrefix = "Creating:";
        private const string Style12Prefix = "<12>";

        private GameSession _gameSession;
        private CsvLogger _csvLogger;

        private Dictionary<string, GameResult> _gameResultTokens;
        private Color _engineColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameMode"/> class.
        /// </summary>
        /// <param name="configManager">The configuration manager.</param>
        public GameMode(ConfigManager configManager) : base(configManager)
        {
            _gameSession = new GameSession();
            _csvLogger = new CsvLogger(AILogsDirectory);

            _gameResultTokens = new Dictionary<string, GameResult>()
            {
                { "1-0", GameResult.WhiteWon },
                { "0-1", GameResult.BlackWon },
                { "1/2-1/2", GameResult.Draw },
                { "aborted on move 1", GameResult.Aborted }
            };
        }

        /// <summary>
        /// Processes message (does incoming moves, runs AI calculating and changes mode when game has ended.).
        /// </summary>
        /// <param name="message">The message to process.</param>
        /// <returns>The response for the message (<see cref="string.Empty"/> if none).</returns>
        public override void ProcessMessage(string message)
        {
            if (message.StartsWith(CreatingPrefix))
            {
                InitGameSession(message);
            }
            else if (message.StartsWith(Style12Prefix))
            {
                var response = ProcessMoveCommand(message);
                if (response != string.Empty)
                {
                    SendData(response);
                }
            }
            else if (_gameResultTokens.Any(p => message.Contains(p.Key)))
            {
                SaveGameResult(message);
                ChangeMode(FICSModeType.Seek);
            }
        }

        private void InitGameSession(string message)
        {
            var username = ConfigManager.GetValue<string>("Username");
            if(message.StartsWith($"{CreatingPrefix} {username}"))
            {
                _engineColor = Color.White;
            }
            else
            {
                _engineColor = Color.Black;
            }
        }

        /// <summary>
        /// Processes move command and runs AI calculating if necessary.
        /// </summary>
        /// <param name="message">The move message.</param>
        /// <returns>The response for the message (<see cref="string.Empty"/> if none).</returns>
        private string ProcessMoveCommand(string message)
        {
            var style12Parser = new Style12Parser();
            var style12Container = style12Parser.Parse(message);

            if (style12Container != null && style12Container.Relation == Style12RelationType.EngineMove)
            {
                _gameSession.UpdateRemainingTime(Color.White, style12Container.RemainingTime[(int)Color.White]);
                _gameSession.UpdateRemainingTime(Color.Black, style12Container.RemainingTime[(int)Color.Black]);

                CalculateEnemyMove(style12Container);
                return CalculateAIMove(style12Container);
            }

            return string.Empty;
        }

        /// <summary>
        /// Applies enemy move to the bitboard.
        /// </summary>
        /// <param name="style12Container">The data from FICS.</param>
        private void CalculateEnemyMove(Style12Container style12Container)
        {
            if (style12Container.PreviousMove != null)
            {
                if (style12Container.PreviousMove.PromotionPieceType.HasValue)
                {
                    _gameSession.Move(style12Container.ColorToMove, style12Container.PreviousMove.From, style12Container.PreviousMove.To,
                                      style12Container.PreviousMove.PromotionPieceType.Value);
                }
                else
                {
                    _gameSession.Move(style12Container.ColorToMove, style12Container.PreviousMove.From, style12Container.PreviousMove.To);
                }
            }
        }

        /// <summary>
        /// Runs AI calculation and applies best move to the bitboard.
        /// </summary>
        /// <param name="style12Container">The data from FICS.</param>
        /// <returns>The response (best move) to FICS.</returns>
        private string CalculateAIMove(Style12Container style12Container)
        {
            var aiResult = _gameSession.MoveAI(_engineColor);

            var fromConverted = PositionConverter.ToString(aiResult.BestMove.From);
            var toConverted = PositionConverter.ToString(aiResult.BestMove.To);

            _csvLogger.WriteLine(aiResult, _gameSession.Bitboard);
            return $"{fromConverted}-{toConverted}";
        }

        /// <summary>
        /// Saves a game result to the log file.
        /// </summary>
        /// <param name="message">The data from FICS with the result message.</param>
        private void SaveGameResult(string message)
        {
            var gameResult = _gameResultTokens.First(p => message.Contains(p.Key)).Value;
            _csvLogger.WriteLine(gameResult, _engineColor);
        }
    }
}
