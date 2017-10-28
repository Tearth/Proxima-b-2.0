using Proxima.Core.Commons.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxima.Core.Commons.Notation
{
    public static class PieceConverter
    {
        public const char PawnSymbol = 'P';
        public const char RookSymbol = 'R';
        public const char KnightSymbol = 'N';
        public const char BishopSymbol = 'B';
        public const char QueenSymbol = 'Q';
        public const char KingSymbol = 'K';

        public static char GetSymbol(PieceType piece)
        {
            switch(piece)
            {
                case PieceType.Pawn: return PawnSymbol;
                case PieceType.Rook: return RookSymbol;
                case PieceType.Knight: return KnightSymbol;
                case PieceType.Bishop: return BishopSymbol;
                case PieceType.Queen: return QueenSymbol;
                case PieceType.King: return KingSymbol;
            }

            throw new PieceSymbolNotFoundException();
        }

        public static PieceType GetPiece(char pieceSymbol)
        {
            switch(pieceSymbol)
            {
                case PawnSymbol: return PieceType.Pawn;
                case RookSymbol: return PieceType.Rook;
                case KnightSymbol: return PieceType.Knight;
                case BishopSymbol: return PieceType.Bishop;
                case QueenSymbol: return PieceType.Queen;
                case KingSymbol: return PieceType.King;
            }

            throw new PieceSymbolNotFoundException();
        }
    }
}
