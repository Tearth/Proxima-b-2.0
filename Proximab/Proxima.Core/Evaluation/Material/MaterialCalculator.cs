using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;

namespace Proxima.Core.Evaluation.Material
{
    public class MaterialCalculator
    {
        public int Calculate(GamePhase gamePhase, BitBoard bitBoard)
        {
            var whiteMaterial = GetMaterialValue(Color.White, gamePhase, bitBoard);
            var blackMaterial = GetMaterialValue(Color.Black, gamePhase, bitBoard);

            return whiteMaterial - blackMaterial;
        }

        public MaterialData CalculateDetailed(GamePhase gamePhase, BitBoard bitBoard)
        {
            return new MaterialData
            {
                WhiteMaterial = GetMaterialValue(Color.White, gamePhase, bitBoard),
                BlackMaterial = GetMaterialValue(Color.Black, gamePhase, bitBoard)
            };
        }

        private int GetMaterialValue(Color color, GamePhase gamePhase, BitBoard bitBoard)
        {
            var material = 0;

            for (int piece = 0; piece < 6; piece++)
            {
                var piecesToParse = bitBoard.Pieces[FastArray.GetPieceIndex(color, (PieceType)piece)];
                var piecesCount = BitOperations.Count(piecesToParse);
                
                material += piecesCount * MaterialValues.PieceValues[piece];
            }

            return material;
        }
    }
}
