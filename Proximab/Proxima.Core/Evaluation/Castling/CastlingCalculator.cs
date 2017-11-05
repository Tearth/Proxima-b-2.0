using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using System;

namespace Proxima.Core.Evaluation.Castling
{
    public class CastlingCalculator
    {
        public CastlingResult Calculate(EvaluationParameters parameters)
        {
            return new CastlingResult()
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
