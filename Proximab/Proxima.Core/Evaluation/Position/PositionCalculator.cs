using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Evaluation.Position.Values;

namespace Proxima.Core.Evaluation.Position
{
    public class PositionCalculator
    {
        public int Calculate(GamePhase gamePhase, BitBoard bitBoard)
        {
            var whitePosition = GetPosition(Color.White, gamePhase, bitBoard);
            var blackPosition = GetPosition(Color.Black, gamePhase, bitBoard);

            return whitePosition - blackPosition;
        }

        public PositionData CalculateDetailed(GamePhase gamePhase, BitBoard bitBoard)
        {
            return new PositionData()
            {
                WhitePosition = GetPosition(Color.White, gamePhase, bitBoard),
                BlackPosition = GetPosition(Color.Black, gamePhase, bitBoard)
            };
        }

        private int GetPosition(Color color, GamePhase gamePhase, BitBoard bitBoard)
        {
            var position = 0;

            for (int piece = 0; piece < 6; piece++)
            {
                var piecesToParse = bitBoard.Pieces[FastArray.GetPieceIndex(color, (PieceType)piece)];
                position += GetPositionValue(color, (PieceType)piece, gamePhase, piecesToParse);
            }

            return position;
        }

        private int GetPositionValue(Color color, PieceType pieceType, GamePhase gamePhase, ulong piecesToParse)
        {
            var position = 0;
            var array = PositionValues.GetValues(color, pieceType);

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(piecesToParse);
                piecesToParse = BitOperations.PopLSB(piecesToParse);

                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                position += array[FastArray.GetEvaluationValueIndex(gamePhase, pieceIndex)];
            }

            return position;
        }
    }
}
