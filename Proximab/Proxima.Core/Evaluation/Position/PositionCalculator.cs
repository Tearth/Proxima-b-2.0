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
                WhitePosition = GetPosition(Color.White, parameters),
                BlackPosition = GetPosition(Color.Black, parameters)
            };
        }

        int GetPosition(Color color, EvaluationParameters parameters)
        {
            var position = 0;

            for (int piece = 0; piece < 6; piece++)
            {
                var piecesToParse = parameters.Pieces[FastArray.GetPieceIndex(color, (PieceType)piece)];
                position += GetPositionValue(color, (PieceType)piece, parameters.GamePhase, piecesToParse);
            }

            return position;
        }

        int GetPositionValue(Color color, PieceType pieceType, GamePhase gamePhase, ulong piecesToParse)
        {
            var position = 0;
            var array = GetPositionValuesArray(color, pieceType);

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(piecesToParse);
                piecesToParse = BitOperations.PopLSB(piecesToParse);

                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                position += array[FastArray.GetEvaluationValueIndex(gamePhase, pieceIndex)];
            }

            return position;
        }

        int[] GetPositionValuesArray(Color color, PieceType pieceType)
        {
            switch (pieceType)
            {
                case (PieceType.Pawn):      return PawnValues.GetValues(color);
                case (PieceType.Knight):    return KnightValues.GetValues(color);
                case (PieceType.Bishop):    return BishopValues.GetValues(color);
                case (PieceType.Rook):      return RookValues.GetValues(color);
                case (PieceType.Queen):     return QueenValues.GetValues(color);
                case (PieceType.King):      return KingValues.GetValues(color);
            }

            return null;
        }
    }
}
