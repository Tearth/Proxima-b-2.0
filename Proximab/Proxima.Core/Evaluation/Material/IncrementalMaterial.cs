using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxima.Core.Evaluation.Material
{
    public static class IncrementalMaterial
    {
        public static int AddPiece(int material, PieceType pieceType, Color color)
        {
            var pieceValue = MaterialValues.PieceValues[(int)pieceType];

            switch (color)
            {
                case Color.White: return material + pieceValue;
                case Color.Black: return material - pieceValue;
            }

            return 0;
        }

        public static int RemovePiece(int material, PieceType pieceType, Color color)
        {
            var pieceValue = MaterialValues.PieceValues[(int)pieceType];

            switch (color)
            {
                case Color.White: return material - pieceValue;
                case Color.Black: return material + pieceValue;
            }

            return 0;
        }
    }
}
