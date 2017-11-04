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
                WhiteCastling = GetCastling(Color.White, parameters),
                BlackCastling = GetCastling(Color.Black, parameters)
            };
        }

        int GetCastling(Color color, EvaluationParameters parameters)
        {
            return Convert.ToInt32(parameters.CastlingDone[(int)color]) * CastlingValues.Ratio;
        }
    }
}
