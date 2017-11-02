using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;

namespace Proxima.Core.Evaluation.Calculators
{
    public class MaterialCalculator
    {
        public int[] Calculate(EvaluationParameters parameters)
        {
            var material = new int[2];

            for (int color = 0; color < 2; color++)
            {
                for (int piece = 0; piece < 6; piece++)
                {
                    var pieces = parameters.Pieces[FastArray.GetPieceIndex((Color)color, (PieceType)piece)];
                    while(pieces != 0)
                    {
                        var lsb = BitOperations.GetLSB(ref pieces);
                        material[color] += GetPieceValue((PieceType)piece);
                    }
                }
            }

            return material;
        }

        int GetPieceValue(PieceType piece)
        {
            switch (piece)
            {
                case (PieceType.Pawn):   return EvaluationConstants.PawnValue;
                case (PieceType.Rook):   return EvaluationConstants.RookValue;
                case (PieceType.Knight): return EvaluationConstants.KnightValue;
                case (PieceType.Bishop): return EvaluationConstants.BishopValue;
                case (PieceType.Queen):  return EvaluationConstants.QueenValue;
                case (PieceType.King):   return EvaluationConstants.KingValue;
            }

            return 0;
        }
    }
}
