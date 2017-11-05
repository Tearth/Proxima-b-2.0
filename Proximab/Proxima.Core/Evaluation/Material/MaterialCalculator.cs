using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;

namespace Proxima.Core.Evaluation.Material
{
    public class MaterialCalculator
    {
        public MaterialResult Calculate(EvaluationParameters parameters)
        {
            return new MaterialResult
            {
                WhiteMaterial = GetMaterialValue(Color.White, parameters),
                BlackMaterial = GetMaterialValue(Color.Black, parameters)
            };
        }

        int GetMaterialValue(Color color, EvaluationParameters parameters)
        {
            var material = 0;

            for (int piece = 0; piece < 6; piece++)
            {
                var piecesToParse = parameters.Pieces[FastArray.GetPieceIndex(color, (PieceType)piece)];
                var piecesCount = BitOperations.Count(piecesToParse);
                
                material += piecesCount * MaterialValues.PieceValues[piece];
            }

            return material;
        }
    }
}
