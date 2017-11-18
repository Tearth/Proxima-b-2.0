using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation.Castling
{
    public static class IncrementalCastling
    {
        public static int SetCastlingDone(int castling, Color color, GamePhase gamePhase)
        {
            switch(color)
            {
                case Color.White: return castling + CastlingValues.Ratio[(int)gamePhase];
                case Color.Black: return castling - CastlingValues.Ratio[(int)gamePhase];
            }

            return 0;
        }
    }
}
