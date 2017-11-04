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
            var castling = new CastlingResult();

            castling.WhiteCastling = GetCastling(Color.White, parameters.CastlingDone);
            castling.BlackCastling = GetCastling(Color.Black, parameters.CastlingDone);

            return castling;
        }

        int GetCastling(Color color, bool[] castlingDone)
        {
            return Convert.ToInt32(castlingDone[(int)color]) * CastlingValues.Ratio;
        }
    }
}
