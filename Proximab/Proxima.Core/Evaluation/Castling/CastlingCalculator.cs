using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation.Castling
{
    public class CastlingCalculator
    {
        public int Calculate(GamePhase gamePhase, BitBoard bitBoard)
        {
            var whiteCastling = GetCastlingValue(Color.White, gamePhase, bitBoard);
            var blackCastling = GetCastlingValue(Color.Black, gamePhase, bitBoard);

            return whiteCastling - blackCastling;
        }

        public CastlingData CalculateDetailed(GamePhase gamePhase, BitBoard bitBoard)
        {
            return new CastlingData()
            {
                WhiteCastling = GetCastlingValue(Color.White, gamePhase, bitBoard),
                BlackCastling = GetCastlingValue(Color.Black, gamePhase, bitBoard)
            };
        }

        private int GetCastlingValue(Color color, GamePhase gamePhase, BitBoard bitBoard)
        {
            if (bitBoard.CastlingDone[(int)color])
            {
                return CastlingValues.Ratio[(int)gamePhase];
            }

            return 0;
        }
    }
}
