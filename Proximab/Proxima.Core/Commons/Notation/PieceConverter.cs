using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxima.Core.Commons.Notation
{
    public static class PieceConverter
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

        public static PieceType GetPiece(char piece)
        {
            switch(piece)
            {
                case 'P': return PieceType.Pawn;
                case 'R': return PieceType.Rook;
                case 'N': return PieceType.Knight;
                case 'B': return PieceType.Bishop;
                case 'Q': return PieceType.Queen;
                case 'K': return PieceType.King;
            }

            return PieceType.Pawn;
        }
    }
}
