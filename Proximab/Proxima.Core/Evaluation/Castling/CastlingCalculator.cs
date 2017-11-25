using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation.Castling
{
    public class CastlingCalculator
    {
        public int Calculate(EvaluationParameters parameters)
        {
            var whiteCastling = GetCastlingValue(Color.White, parameters);
            var blackCastling = GetCastlingValue(Color.Black, parameters);

            return whiteCastling - blackCastling;
        }

        public CastlingData CalculateDetailed(EvaluationParameters parameters)
        {
            return new CastlingData()
            {
                WhiteCastling = GetCastlingValue(Color.White, parameters),
                BlackCastling = GetCastlingValue(Color.Black, parameters)
            };
        }

        private int GetCastlingValue(Color color, EvaluationParameters parameters)
        {
            if (parameters.CastlingDone[(int)color])
            {
                return CastlingValues.Ratio[(int)parameters.GamePhase];
            }

            return 0;
        }
    }
}
