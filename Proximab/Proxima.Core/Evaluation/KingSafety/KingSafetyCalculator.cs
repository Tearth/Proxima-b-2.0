using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.MoveGenerators.PatternGenerators;

namespace Proxima.Core.Evaluation.KingSafety
{
    public class KingSafetyCalculator
    {
        public int Calculate(GamePhase gamePhase, BitBoard bitBoard)
        {
            var whiteKingSafety = GetAttackedNeighboursValue(Color.White, gamePhase, bitBoard);
            var blackKingSafety = GetAttackedNeighboursValue(Color.Black, gamePhase, bitBoard);

            return whiteKingSafety - blackKingSafety;
        }

        public KingSafetyData CalculateDetailed(GamePhase gamePhase, BitBoard bitBoard)
        {
            return new KingSafetyData
            {
                WhiteAttackedNeighbours = GetAttackedNeighboursValue(Color.White, gamePhase, bitBoard),
                BlackAttackedNeighbours = GetAttackedNeighboursValue(Color.Black, gamePhase, bitBoard)
            };
        }

        private int GetAttackedNeighboursValue(Color color, GamePhase gamePhase, BitBoard bitBoard)
        {
            var attackedNeightbours = 0;

            var king = bitBoard.Pieces[FastArray.GetPieceIndex(color, PieceType.King)];
            var kingIndex = BitOperations.GetBitIndex(king);
            var kingMoves = PatternsContainer.KingPattern[kingIndex];

            while (kingMoves != 0)
            {
                var fieldLSB = BitOperations.GetLSB(kingMoves);
                kingMoves = BitOperations.PopLSB(kingMoves);

                var fieldIndex = BitOperations.GetBitIndex(fieldLSB);
                var attacks = bitBoard.Attacks[fieldIndex] & ~bitBoard.Occupancy[(int)color];

                attackedNeightbours += BitOperations.Count(attacks);
            }

            return attackedNeightbours * KingSafetyValues.AttackedNeighboursRatio[(int)gamePhase];
        }
    }
}
