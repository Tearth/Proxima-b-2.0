using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;

namespace Proxima.Core.Evaluation.Calculators
{
    public class MaterialCalculator
    {
        public MaterialResult Calculate(EvaluationParameters parameters)
        {
            var material = new MaterialResult();

            material.WhiteMaterial = GetMaterial(Color.White, parameters.Pieces);
            material.BlackMaterial = GetMaterial(Color.Black, parameters.Pieces);

            return material;
        }

        int GetMaterial(Color color, ulong[] pieces)
        {
            var material = 0;

            for (int piece = 0; piece < 6; piece++)
            {
                var piecesToParse = pieces[FastArray.GetPieceIndex(color, (PieceType)piece)];
                while (piecesToParse != 0)
                {
                    var lsb = BitOperations.GetLSB(ref piecesToParse);
                    material += GetPieceValue((PieceType)piece);
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
