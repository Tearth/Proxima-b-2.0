using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.AI.SEE.Exceptions;
using Proxima.Core.Boards;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Evaluation.Material;

namespace Proxima.Core.AI.SEE
{
    public class SEECalculator
    {
        public LinkedList<SEEResult> Calculate(Color initialColor, Bitboard bitboard)
        {
            var seeResults = new LinkedList<SEEResult>();

            var enemyColor = ColorOperations.Invert(initialColor);
            var possibleAttacks = bitboard.AttacksSummary[(int)initialColor] & bitboard.Occupancy[(int)enemyColor];

            while (possibleAttacks != 0)
            {
                var field = BitOperations.GetLSB(possibleAttacks);
                possibleAttacks = BitOperations.PopLSB(possibleAttacks);

                RunSSEForField(field, initialColor, bitboard, seeResults);
            }

            return seeResults;
        }

        private void RunSSEForField(ulong field, Color initialColor, Bitboard bitboard, LinkedList<SEEResult> seeResults)
        {
            var fieldIndex = BitOperations.GetBitIndex(field);

            var fieldAttackers = bitboard.Attacks[fieldIndex];
            var fieldAttackersWithInitialcolor = fieldAttackers & bitboard.Occupancy[(int)initialColor];

            while (fieldAttackersWithInitialcolor != 0)
            {
                var initialAttacker = BitOperations.GetLSB(fieldAttackersWithInitialcolor);
                fieldAttackersWithInitialcolor = BitOperations.PopLSB(fieldAttackersWithInitialcolor);

                var seeResult = CalculateScoreForField(field, initialAttacker, fieldAttackers, initialColor, bitboard);
                seeResults.AddLast(seeResult);
            }
        }

        private SEEResult CalculateScoreForField(ulong fieldBitboard, ulong initialAttackerBitboard, ulong attackers, Color initialColor, Bitboard bitboard)
        {
            var seeResult = new SEEResult();
            seeResult.InitialAttackerFrom = BitPositionConverter.ToPosition(BitOperations.GetBitIndex(initialAttackerBitboard));
            seeResult.InitialAttackerTo = BitPositionConverter.ToPosition(BitOperations.GetBitIndex(fieldBitboard));

            var enemyColor = ColorOperations.Invert(initialColor);

            seeResult.InitialAttackerType = GetPieceType(initialAttackerBitboard, initialColor, bitboard);
            seeResult.AttackedPieceType = GetPieceType(fieldBitboard, enemyColor, bitboard);

            seeResult.Score += MaterialValues.PieceValues[(int)seeResult.AttackedPieceType];

            attackers &= ~initialAttackerBitboard;
            var fieldAttackers = new ulong[2]
            {
                attackers & bitboard.Occupancy[(int)initialColor],
                attackers & bitboard.Occupancy[(int)enemyColor]
            };
            
            var currentColor = enemyColor;
            var currentSign = -1;

            var currentPieceOnField = seeResult.InitialAttackerType;

            while (attackers != 0)
            {
                var leastValuablePieceType = GetAndPopLeastValuablePiece(ref attackers, currentColor, bitboard);
                if(!leastValuablePieceType.HasValue)
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

        private PieceType GetPieceType(ulong pieceBitboard, Color pieceColor, Bitboard bitboard)
        {
            for (int piece = 0; piece < 6; piece++)
            {
                if ((pieceBitboard & bitboard.Pieces[FastArray.GetPieceIndex(pieceColor, (PieceType)piece)]) != 0)
                {
                    return (PieceType)piece;
                }
            }

            throw new InitialAttackerNotFoundException();
        }

        private PieceType? GetAndPopLeastValuablePiece(ref ulong attackers, Color color, Bitboard bitboard)
        {
            for (int piece = 0; piece < 6; piece++)
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
