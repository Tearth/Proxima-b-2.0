using System.Collections.Generic;
using Proxima.Core.AI.SEE.Exceptions;
using Proxima.Core.Boards;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Evaluation.Material;

namespace Proxima.Core.AI.SEE
{
    /// <summary>
    /// Represents a set of methods to do a static exchange evaluation (SEE).
    /// </summary>
    public class SEECalculator
    {
        /// <summary>
        /// Calculates SEE for the specified bitboard. All fields that are attacked by the passed color will be processed.
        /// </summary>
        /// <param name="initialColor">The color of the first attacker.</param>
        /// <param name="bitboard">The bitboard.</param>
        /// <returns>The list of all attacked fields with associated scores (relative to the first color attacker).</returns>
        public LinkedList<SEEResult> Calculate(Color initialColor, Bitboard bitboard)
        {
            var seeResults = new LinkedList<SEEResult>();

            var enemyColor = ColorOperations.Invert(initialColor);
            var possibleAttacks = bitboard.AttacksSummary[(int)initialColor] & bitboard.Occupancy[(int)enemyColor];

            while (possibleAttacks != 0)
            {
                var field = BitOperations.GetLSB(possibleAttacks);
                possibleAttacks = BitOperations.PopLSB(possibleAttacks);

                RunSEEForField(field, initialColor, bitboard, seeResults);
            }

            return seeResults;
        }

        /// <summary>
        /// Runs SEE for the specified field. All combinations of first attackers will be processed.
        /// </summary>
        /// <param name="field">The field to analyze.</param>
        /// <param name="initialColor">The color of the first attacker.</param>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="seeResults">The list of processed fields with associated scores.</param>
        private void RunSEEForField(ulong field, Color initialColor, Bitboard bitboard, LinkedList<SEEResult> seeResults)
        {
            var fieldIndex = BitOperations.GetBitIndex(field);

            var fieldAttackers = bitboard.Attacks[fieldIndex];
            var fieldAttackersWithInitialColor = fieldAttackers & bitboard.Occupancy[(int)initialColor];

            while (fieldAttackersWithInitialColor != 0)
            {
                var initialAttacker = BitOperations.GetLSB(fieldAttackersWithInitialColor);
                fieldAttackersWithInitialColor = BitOperations.PopLSB(fieldAttackersWithInitialColor);

                var seeResult = CalculateScoreForField(field, initialAttacker, fieldAttackers, initialColor, bitboard);
                seeResults.AddLast(seeResult);
            }
        }

        /// <summary>
        /// Calculates SEE for the specified field and initial attacker.
        /// </summary>
        /// <param name="field">The field to analyze.</param>
        /// <param name="initialAttacker">The initial attacker.</param>
        /// <param name="attackers">The bitboard with all attackers that will be a part of SEE.</param>
        /// <param name="initialColor">The color of the first attacker.</param>
        /// <param name="bitboard">The bitboard.</param>
        /// <returns>The SEE result with the score and other data.</returns>
        private SEEResult CalculateScoreForField(ulong field, ulong initialAttacker, ulong attackers, Color initialColor, Bitboard bitboard)
        {
            var enemyColor = ColorOperations.Invert(initialColor);

            var seeResult = new SEEResult
            {
                InitialAttackerFrom = BitPositionConverter.ToPosition(BitOperations.GetBitIndex(initialAttacker)),
                InitialAttackerTo = BitPositionConverter.ToPosition(BitOperations.GetBitIndex(field)),

                InitialAttackerType = GetPieceType(initialAttacker, initialColor, bitboard),
                AttackedPieceType = GetPieceType(field, enemyColor, bitboard)
            };

            seeResult.Score = MaterialValues.PieceValues[(int)seeResult.AttackedPieceType];
            attackers &= ~initialAttacker;

            var currentColor = enemyColor;
            var currentSign = -1;

            var currentPieceOnField = seeResult.InitialAttackerType;

            while (attackers != 0)
            {
                var leastValuablePieceType = GetAndPopLeastValuablePiece(ref attackers, currentColor, bitboard);
                if (!leastValuablePieceType.HasValue)
                {
                    break;
                }

                seeResult.Score += currentSign * MaterialValues.PieceValues[(int)currentPieceOnField];

                currentPieceOnField = leastValuablePieceType.Value;
                currentColor = ColorOperations.Invert(currentColor);
                currentSign *= -1;
            }

            return seeResult;
        }

        /// <summary>
        /// Gets a piece type on the specified field.
        /// </summary>
        /// <param name="field">The field with piece.</param>
        /// <param name="pieceColor">The piece color.</param>
        /// <param name="bitboard">The bitboard.</param>
        /// <exception cref="PieceTypeNotFoundException">Thrown when there is no piece on the specified field.</exception>
        /// <returns>The piece type on the specified field.</returns>
        private PieceType GetPieceType(ulong field, Color pieceColor, Bitboard bitboard)
        {
            for (var piece = 0; piece < 6; piece++)
            {
                if ((field & bitboard.Pieces[FastArray.GetPieceIndex(pieceColor, (PieceType)piece)]) != 0)
                {
                    return (PieceType)piece;
                }
            }

            throw new PieceTypeNotFoundException();
        }

        /// <summary>
        /// Gets a least valuable piece for the specified bitboard with field attackers.
        /// </summary>
        /// <param name="attackers">The bitboard with field attackers.</param>
        /// <param name="color">The color of a least valuable piece.</param>
        /// <param name="bitboard">The bitboard.</param>
        /// <returns>The least valuable piece (null if there is no more available pieces).</returns>
        private PieceType? GetAndPopLeastValuablePiece(ref ulong attackers, Color color, Bitboard bitboard)
        {
            for (var piece = 0; piece < 6; piece++)
            {
                var attackersWithType = attackers & bitboard.Pieces[FastArray.GetPieceIndex(color, (PieceType)piece)];

                if (attackersWithType != 0)
                {
                    var attackerLSB = BitOperations.GetLSB(attackersWithType);
                    attackers &= ~attackerLSB;

                    return (PieceType)piece;
                }
            }

            return null;
        }
    }
}
