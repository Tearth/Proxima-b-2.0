using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.AI;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators;
using Proxima.Core.MoveGenerators.Moves;
using Proxima.FICS.Source.ConfigSubsystem;
using Proxima.FICS.Source.GameSubsystem.Modes.Game.Exceptions;
using Proxima.FICS.Source.GameSubsystem.Modes.Game.Style12;
using Proxima.FICS.Source.LogSubsystem;

namespace Proxima.FICS.Source.GameSubsystem.Modes.Game
{
    /// <summary>
    /// Represents the FICS game mode. All AI calculations and interactions with enemies will be done here.
    /// </summary>
    public class GameMode : FICSModeBase
    {
        private Bitboard _bitboard;
        private CsvWriter _csvWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameMode"/> class.
        /// </summary>
        /// <param name="configManager">The configuration manager.</param>
        public GameMode(ConfigManager configManager) : base(configManager)
        {
            _bitboard = new Bitboard(new DefaultFriendlyBoard());
            _csvWriter = new CsvWriter("AILogs");
        }

        /// <summary>
        /// Processes message (does incoming moves, runs AI calculating and changes mode when game has ended.).
        /// </summary>
        /// <param name="message">The message to process.</param>
        /// <returns>The response for the message (<see cref="string.Empty"/> if none).</returns>
        public override string ProcessMessage(string message)
        {
            var response = string.Empty;

            if (message.StartsWith("<12>"))
            {
                response = ProcessMoveCommand(message);
            }
            else if (message.Contains("0-1") || message.Contains("1-0") || message.Contains("1/2-1/2") ||
                     message.Contains("aborted on move 1"))
            {
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

            if (style12Container != null && style12Container.Relation == Style12RelationType.EngineMove)
            {
                _bitboard.Calculate(GeneratorMode.CalculateMoves, GeneratorMode.CalculateMoves);

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

                if(!_bitboard.VerifyIntegrity())
                {
                    throw new BitboardDisintegratedException();
                }
            }
        }

        /// <summary>
        /// Runs AI calculation and applies best move to the bitboard.
        /// </summary>
        /// <param name="style12Container">The data from FICS</param>
        /// <returns>The response (best move) to FICS.</returns>
        private string CalculateAIMove(Style12Container style12Container)
        {
            var ai = new AICore();
            var aiResult = ai.Calculate(style12Container.ColorToMove, _bitboard, 4);

            _bitboard = _bitboard.Move(aiResult.BestMove);

            var fromConverted = PositionConverter.ToString(aiResult.BestMove.From);
            var toConverted = PositionConverter.ToString(aiResult.BestMove.To);

            _csvWriter.WriteLine(aiResult);
            return $"{fromConverted}-{toConverted}";
        }
    }
}
