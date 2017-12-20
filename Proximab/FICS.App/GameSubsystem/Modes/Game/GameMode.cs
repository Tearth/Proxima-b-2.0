using System.Collections.Generic;
using System.Linq;
using FICS.App.ConfigSubsystem;
using FICS.App.GameSubsystem.Modes.Game.Exceptions;
using FICS.App.GameSubsystem.Modes.Game.Style12;
using Helpers.Loggers.CSV;
using Proxima.Core.AI;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators;
using Proxima.Core.MoveGenerators.Moves;
using Proxima.Core.Time;

namespace FICS.App.GameSubsystem.Modes.Game
{
    /// <summary>
    /// Represents the FICS game mode. All AI calculations and interactions with enemies will be done here.
    /// </summary>
    public class GameMode : FICSModeBase
    {
        private const string AILogsDirectory = "AILogs";
        private const string Style12Prefix = "<12>";

        private Bitboard _bitboard;
        private CsvLogger _csvLogger;
        private PreferredTimeCalculator _preferredTimeCalculator;

        private Dictionary<string, GameResult> _gameResultTokens;
        private Color? _engineColor;

        private int _movesCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameMode"/> class.
        /// </summary>
        /// <param name="configManager">The configuration manager.</param>
        public GameMode(ConfigManager configManager) : base(configManager)
        {
            _bitboard = new Bitboard(new DefaultFriendlyBoard());
            _csvLogger = new CsvLogger(AILogsDirectory);
            _preferredTimeCalculator = new PreferredTimeCalculator(60);

            _gameResultTokens = new Dictionary<string, GameResult>()
            {
                { "1-0", GameResult.WhiteWon },
                { "0-1", GameResult.BlackWon },
                { "1/2-1/2", GameResult.Draw },
                { "aborted on move 1", GameResult.Aborted }
            };

            _movesCount = 0;
        }

        /// <summary>
        /// Processes message (does incoming moves, runs AI calculating and changes mode when game has ended.).
        /// </summary>
        /// <param name="message">The message to process.</param>
        /// <returns>The response for the message (<see cref="string.Empty"/> if none).</returns>
        public override string ProcessMessage(string message)
        {
            var response = string.Empty;

            if (message.StartsWith(Style12Prefix))
            {
                response = ProcessMoveCommand(message);
            }
            else if (_gameResultTokens.Any(p => message.Contains(p.Key)))
            {
                SaveGameResult(message);
                ChangeMode(FICSModeType.Seek);
            }

            return response;
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

            InitEngineColor(style12Container);

            if (style12Container != null && style12Container.Relation == Style12RelationType.EngineMove)
            {
                _bitboard.Calculate(GeneratorMode.CalculateMoves, GeneratorMode.CalculateMoves);

                CalculateEnemyMove(style12Container);
                var aiResponse = CalculateAIMove(style12Container);

                if (!_bitboard.VerifyIntegrity())
                {
                    throw new BitboardDisintegratedException();
                }

                _movesCount++;
                return aiResponse;
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
                Move moveToApply;

                if (style12Container.PreviousMove.PromotionPieceType.HasValue)
                {
                    moveToApply = _bitboard.Moves.First(
                        p => p.From == style12Container.PreviousMove.From &&
                        p.To == style12Container.PreviousMove.To &&
                        (p as PromotionMove).PromotionPiece == style12Container.PreviousMove.PromotionPieceType);
                }
                else
                {
                    moveToApply = _bitboard.Moves.First(
                        p => p.From == style12Container.PreviousMove.From &&
                        p.To == style12Container.PreviousMove.To);
                }

                _bitboard = _bitboard.Move(moveToApply);
            }
        }

        /// <summary>
        /// Runs AI calculation and applies best move to the bitboard.
        /// </summary>
        /// <param name="style12Container">The data from FICS.</param>
        /// <returns>The response (best move) to FICS.</returns>
        private string CalculateAIMove(Style12Container style12Container)
        {
            var ai = new AICore();
            var preferredTime = _preferredTimeCalculator.Calculate(_movesCount, style12Container.RemainingTime[(int)_engineColor]);

            var aiResult = ai.Calculate(style12Container.ColorToMove, new Bitboard(_bitboard), preferredTime);

            _bitboard = _bitboard.Move(aiResult.BestMove);

            var fromConverted = PositionConverter.ToString(aiResult.BestMove.From);
            var toConverted = PositionConverter.ToString(aiResult.BestMove.To);

            _csvLogger.WriteLine(aiResult, _bitboard);
            return $"{fromConverted}-{toConverted}";
        }

        /// <summary>
        /// Inits the engine color if it was not done earlier.
        /// </summary>
        /// <param name="style12Container">The data from FICS.</param>
        private void InitEngineColor(Style12Container style12Container)
        {
            if (!_engineColor.HasValue)
            {
                switch (style12Container.Relation)
                {
                    case Style12RelationType.EngineMove:
                    {
                        _engineColor = style12Container.ColorToMove;
                        break;
                    }

                    case Style12RelationType.EnemyMove:
                    {
                        _engineColor = style12Container.EnemyColor;
                        break;
                    }
                }
            }
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
