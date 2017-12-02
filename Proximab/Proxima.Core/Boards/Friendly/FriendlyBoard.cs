using System.Collections.Generic;
using System.Linq;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Boards.Friendly
{
    public class FriendlyBoard
    {
        public FriendlyPiecesList Pieces { get; private set; }
        public FriendlyAttacksList Attacks { get; private set; }
        public FriendlyCastling Castling { get; private set; }
        public FriendlyEnPassant EnPassant { get; private set; }

        public FriendlyBoard()
        {
            Pieces = new FriendlyPiecesList();
            Attacks = new FriendlyAttacksList();
            Castling = new FriendlyCastling();
            EnPassant = new FriendlyEnPassant();
        }

        public FriendlyBoard(Bitboard bitboard)
        {
            Pieces = new FriendlyPiecesList(bitboard.Pieces);
            Attacks = new FriendlyAttacksList(bitboard.Attacks, Pieces);
            Castling = new FriendlyCastling(bitboard.CastlingPossibility, bitboard.CastlingDone);
            EnPassant = new FriendlyEnPassant(bitboard.EnPassant);
        }

        public FriendlyBoard(FriendlyPiecesList pieces, FriendlyCastling castling, FriendlyEnPassant enPassant)
        {
            Pieces = pieces;
            Castling = castling;
            EnPassant = enPassant;
        }

        public FriendlyPiece GetPiece(Position position)
        {
            return Pieces.FirstOrDefault(p => p.Position == position);
        }

        public void SetPiece(FriendlyPiece piece)
        {
            RemovePiece(piece.Position);
            Pieces.Add(piece);
        }

        public void RemovePiece(Position position)
        {
            var existingPiece = Pieces.FirstOrDefault(p => p.Position == position);

            if (existingPiece != null)
            {
                Pieces.Remove(existingPiece);
            }
        }

        public ulong[] GetPiecesArray()
        {
            var pieces = new ulong[12];

            foreach (var piece in Pieces)
            {
                var bitPosition = BitPositionConverter.ToULong(piece.Position);
                pieces[FastArray.GetPieceIndex(piece.Color, piece.Type)] |= bitPosition;
            }

            return pieces;
        }

        public bool[] GetCastlingPossibilityArray()
        {
            var castling = new bool[4];

            castling[0] = Castling.WhiteShortCastlingPossibility;
            castling[1] = Castling.WhiteLongCastlingPossibility;
            castling[2] = Castling.BlackShortCastlingPossibility;
            castling[3] = Castling.BlackLongCastlingPossibility;

            return castling;
        }

        public bool[] GetCastlingDoneArray()
        {
            var castling = new bool[2];

            castling[0] = Castling.WhiteCastlingDone;
            castling[1] = Castling.BlackCastlingDone;

            return castling;
        }

        public ulong[] GetEnPassantArray()
        {
            ulong[] enPassant = new ulong[2];

            if (EnPassant.WhiteEnPassant != null)
            {
                enPassant[(int)Color.White] = BitPositionConverter.ToULong(EnPassant.WhiteEnPassant.Value);
            }

            if (EnPassant.BlackEnPassant != null)
            {
                enPassant[(int)Color.Black] = BitPositionConverter.ToULong(EnPassant.BlackEnPassant.Value);
            }

            return enPassant;
        }

        public List<Position> GetOccupancy()
        {
            var whiteOccupancy = GetOccupancy(Color.White);
            var blackOccupancy = GetOccupancy(Color.Black);

            return whiteOccupancy.Concat(blackOccupancy).ToList();
        }

        public List<Position> GetOccupancy(Color color)
        {
            return Pieces.Where(p => p.Color == color).Select(p => p.Position).ToList();
        }

        public List<Position> GetAttacks()
        {
            var whiteAttacks = GetAttacks(Color.White);
            var blackAttacks = GetAttacks(Color.Black);

            return whiteAttacks.Concat(blackAttacks).ToList();
        }

        public List<Position> GetAttacks(Color color)
        {
            return Attacks.Where(p => p.Color == color).Select(p => p.To).ToList();
        }

        public List<Position> GetFieldAttackers(Position position)
        {
            var whiteAttacks = GetFieldAttackers(Color.White, position);
            var blackAttacks = GetFieldAttackers(Color.Black, position);

            return whiteAttacks.Concat(blackAttacks).ToList();
        }

        public List<Position> GetFieldAttackers(Color color, Position position)
        {
            return Attacks.Where(p => p.Color == color && p.To == position).Select(p => p.From).ToList();
        }
    }
}
