using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Evaluation.Position.Values;

namespace Proxima.Core.Evaluation.Position
{
    public class PositionCalculator
    {
        public PositionResult Calculate(EvaluationParameters parameters)
        {
            return new PositionResult()
            {
                WhitePosition = GetPosition(Color.White, parameters.GamePhase, parameters.Pieces),
                BlackPosition = GetPosition(Color.Black, parameters.GamePhase, parameters.Pieces)
            };
        }

        int GetPosition(Color color, GamePhase gamePhase, ulong[] pieces)
        {
            var position = 0;

            for (int piece = 0; piece < 6; piece++)
            {
                var piecesToParse = pieces[FastArray.GetPieceIndex(color, (PieceType)piece)];
                position += GetPositionValue(color, (PieceType)piece, gamePhase, piecesToParse);
            }

            return position;
        }

        int GetPositionValue(Color color, PieceType pieceType, GamePhase gamePhase, ulong piecesToParse)
        {
            var position = 0;
            var array = GetPositionValuesArray(pieceType);

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref piecesToParse);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                position += array[FastArray.GetPositionValueIndex(color, gamePhase, pieceIndex)];
            }

            return position;
        }

        int[] GetPositionValuesArray(PieceType pieceType)
        {
            switch(pieceType)
            {
                case (PieceType.Pawn):      return PawnValues.Values;
                case (PieceType.Knight):    return KnightValues.Values;
                case (PieceType.Bishop):    return BishopValues.Values;
                case (PieceType.Rook):      return RookValues.Values;
                case (PieceType.Queen):     return QueenValues.Values;
                case (PieceType.King):      return KingValues.Values;
            }

            return null;
        }
    }
}
