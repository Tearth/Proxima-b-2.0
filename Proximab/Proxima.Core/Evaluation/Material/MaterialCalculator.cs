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
                var piecesCount = BitOperations.Count(piecesToParse);
                
                material += piecesCount * MaterialValues.PieceValues[piece];
            }

            return material;
        }
    }
}
