using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Evaluation.Position.Values;

namespace Proxima.Core.Evaluation.Position
{
    public class PositionCalculator
    {
        public int Calculate(EvaluationParameters parameters)
        {
            var whitePosition = GetPosition(Color.White, parameters);
            var blackPosition = GetPosition(Color.Black, parameters);

            return whitePosition - blackPosition;
        }

        public PositionData CalculateDetailed(EvaluationParameters parameters)
        {
            return new PositionData()
            {
                WhitePosition = GetPosition(Color.White, parameters),
                BlackPosition = GetPosition(Color.Black, parameters)
            };
        }

        private int GetPosition(Color color, EvaluationParameters parameters)
        {
            var position = 0;

            for (int piece = 0; piece < 6; piece++)
            {
                var piecesToParse = parameters.Pieces[FastArray.GetPieceIndex(color, (PieceType)piece)];
                position += GetPositionValue(color, (PieceType)piece, parameters.GamePhase, piecesToParse);
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
