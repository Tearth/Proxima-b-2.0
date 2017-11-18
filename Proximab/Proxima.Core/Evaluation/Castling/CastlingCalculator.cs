using Proxima.Core.Commons.Colors;
using System;

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

        int GetCastlingValue(Color color, EvaluationParameters parameters)
        {
            var castlingDone = Convert.ToInt32(parameters.CastlingDone[(int)color]);
            var ratio = CastlingValues.Ratio[(int)parameters.GamePhase];

            return castlingDone * ratio;
        }
    }
}
