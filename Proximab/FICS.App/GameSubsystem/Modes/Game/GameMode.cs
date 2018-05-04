using System;
using System.Collections.Generic;
using System.Linq;
using FICS.App.ConfigSubsystem;
using FICS.App.GameSubsystem.Modes.Game.Style12;
using Helpers.Loggers.CSV;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;
using Proxima.Core.Session;

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
        private const string HelperThreadsCountConfigKeyName = "HelperThreadsCount";

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
            var helperThreadsCount = configManager.GetValue<int>(HelperThreadsCountConfigKeyName);

            _gameSession = new GameSession(helperThreadsCount);
            _csvLogger = new CsvLogger(AILogsDirectory);

            _gameResultTokens = new Dictionary<string, GameResult>
            {
                { "1-0", GameResult.WhiteWon },
                { "0-1", GameResult.BlackWon },
                { "1/2-1/2", GameResult.Draw },
                { "aborted on move 1", GameResult.Aborted }
            };

            _gameSession.OnGameEnded += GameSession_OnGameEnded;
        }

        /// <summary>
        /// Processes message (does incoming moves, runs AI calculating and changes mode when game has ended.).
        /// </summary>
        /// <param name="message">The message to process.</param>
        public override void ProcessMessage(string message)
        {
            if (message.StartsWith(CreatingPrefix, StringComparison.Ordinal))
            {
                InitGameSession(message);
            }
            else if (message.StartsWith(Style12Prefix, StringComparison.Ordinal))
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

        /// <summary>
        /// Inits game session (sets engine color).
        /// </summary>
        /// <param name="message">The message to process.</param>
        private void InitGameSession(string message)
        {
            var username = ConfigManager.GetValue<string>("Username");
            _engineColor = message.StartsWith($"{CreatingPrefix} {username}", StringComparison.Ordinal) ? Color.White : Color.Black;
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

            if (style12Container?.Relation == Style12RelationType.EngineMove)
            {
                if (_gameSession.WhiteRemainingTime >= style12Container.RemainingTime[(int)Color.White] &&
                    _gameSession.BlackRemainingTime >= style12Container.RemainingTime[(int)Color.Black])
                {
                    _gameSession.UpdateRemainingTime(Color.White, style12Container.RemainingTime[(int)Color.White]);
                    _gameSession.UpdateRemainingTime(Color.Black, style12Container.RemainingTime[(int)Color.Black]);

                    CalculateEnemyMove(style12Container);
                    return CalculateAIMove();
                }
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
                var color = style12Container.ColorToMove;
                var from = style12Container.PreviousMove.From;
                var to = style12Container.PreviousMove.To;

                if (style12Container.PreviousMove.PromotionPieceType.HasValue)
                {
                    var promotionPieceType = style12Container.PreviousMove.PromotionPieceType.Value;
                    _gameSession.Move(color, from, to, promotionPieceType);
                }
                else
                {
                    _gameSession.Move(color, from, to);
                }
            }
        }

        /// <summary>
        /// Runs AI calculation and applies best move to the bitboard.
        /// </summary>
        /// <returns>The response (best move) to FICS.</returns>
        private string CalculateAIMove()
        {
            var aiResult = _gameSession.MoveAI(_engineColor);
            if (aiResult == null)
            {
                return null;
            }

            var fromConverted = PositionConverter.ToString(aiResult.PVNodes[0].From);
            var toConverted = PositionConverter.ToString(aiResult.PVNodes[0].To);

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

        private void GameSession_OnGameEnded(object sender, GameEndedEventArgs e)
        {
            if (e.GameResult == GameResult.Draw)
            {
                SendData("draw");
            }
        }
    }
}
