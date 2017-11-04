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
                WhiteCastling = GetCastling(Color.White, parameters.CastlingDone),
                BlackCastling = GetCastling(Color.Black, parameters.CastlingDone)
            };
        }

        int GetCastling(Color color, bool[] castlingDone)
        {
            return Convert.ToInt32(castlingDone[(int)color]) * CastlingValues.Ratio;
        }
    }
}
