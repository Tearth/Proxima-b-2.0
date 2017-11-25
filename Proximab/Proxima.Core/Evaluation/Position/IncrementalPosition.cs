using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Evaluation.Position.Values;

namespace Proxima.Core.Evaluation.Position
{
    public static class IncrementalPosition
    {
        public static int AddPiece(int position, Color color, PieceType pieceType, ulong piece, GamePhase gamePhase)
        {
            var pieceIndex = BitOperations.GetBitIndex(piece);
            var array = PositionValues.GetValues(color, pieceType);
            var delta = array[FastArray.GetEvaluationValueIndex(gamePhase, pieceIndex)];

            switch (color)
            {
                case Color.White: return position + delta;
                case Color.Black: return position - delta;
            }

            return 0;
        }

        public static int RemovePiece(int position, Color color, PieceType pieceType, ulong piece, GamePhase gamePhase)
        {
            var pieceIndex = BitOperations.GetBitIndex(piece);
            var array = PositionValues.GetValues(color, pieceType);
            var delta = array[FastArray.GetEvaluationValueIndex(gamePhase, pieceIndex)];

            switch (color)
            {
                case Color.White: return position - delta;
                case Color.Black: return position + delta;
            }

            return 0;
        }
    }
}
