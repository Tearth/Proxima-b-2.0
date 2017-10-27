using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Positions;
using System;
using System.Collections.Generic;
using System.Linq;

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
            Castling = new FriendlyCastling(true, true, true, true);
            EnPassant = new FriendlyEnPassant();
        }

        public FriendlyBoard(ulong[] pieces, ulong[] attacks, bool[] castling, ulong[] enPassant)
        {
            Pieces = new FriendlyPiecesList(pieces);
            Attacks = new FriendlyAttacksList(attacks, Pieces);
            Castling = new FriendlyCastling(castling);
            EnPassant = new FriendlyEnPassant(enPassant);
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

            foreach(var piece in Pieces)
            {
                var bitPosition = BitPositionConverter.ToULong(piece.Position);
                pieces[FastArray.GetPieceIndex(piece.Color, piece.Type)] |= bitPosition;
            }

            return pieces;
        }

        public bool[] GetCastlingArray()
        {
            var castling = new bool[4];

            castling[0] = Castling.WhiteShortCastling;
            castling[1] = Castling.WhiteLongCastling;
            castling[2] = Castling.BlackShortCastling;
            castling[3] = Castling.BlackLongCastling;

            return castling;
        }

        public ulong[] GetEnPassantArray()
        {
            ulong[] enPassant = new ulong[2];

            if (EnPassant.WhiteEnPassant != null)
            {
                enPassant[(int)Color.White] = BitPositionConverter.ToULong(EnPassant.WhiteEnPassant);
            }

            if (EnPassant.BlackEnPassant != null)
            {
                enPassant[(int)Color.Black] = BitPositionConverter.ToULong(EnPassant.BlackEnPassant);
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
