using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxima.Core.Commons.Notation
{
    public static class PieceOperations
    {
        public static string GetSymbol(PieceType piece)
        {
            switch(piece)
            {
                case PieceType.Pawn: return "P";
                case PieceType.Rook: return "R";
                case PieceType.Knight: return "N";
                case PieceType.Bishop: return "B";
                case PieceType.Queen: return "Q";
                case PieceType.King: return "K";
            }

            return String.Empty;
        }
    }
}
