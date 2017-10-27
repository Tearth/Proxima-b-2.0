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
        protected FriendlyPiecesList _pieces;
        protected FriendlyAttacksList _attacks;
        protected FriendlyCastling _castling;
        protected FriendlyEnPassant _enPassant;

        public FriendlyBoard()
        {
            _pieces = new FriendlyPiecesList();
            _attacks = new FriendlyAttacksList();
            _castling = new FriendlyCastling(true, true, true, true);
            _enPassant = new FriendlyEnPassant();
        }

        public FriendlyBoard(ulong[] pieces, ulong[] attacks, bool[] castling, ulong[] enPassant)
        {
            _pieces = new FriendlyPiecesList(pieces);
            _attacks = new FriendlyAttacksList(attacks, _pieces);
            _castling = new FriendlyCastling(castling);
            _enPassant = new FriendlyEnPassant(enPassant);
        }

        public FriendlyBoard(FriendlyPiecesList pieces, FriendlyCastling castling, FriendlyEnPassant enPassant)
        {
            _pieces = pieces;
            _castling = castling;
            _enPassant = enPassant;
        }

        public FriendlyPiece GetPiece(Position position)
        {
            return _pieces.FirstOrDefault(p => p.Position == position);
        }

        public void SetPiece(FriendlyPiece piece)
        {
            RemovePiece(piece.Position);
            _pieces.Add(piece);
        }

        public void RemovePiece(Position position)
        {
            var existingPiece = _pieces.FirstOrDefault(p => p.Position == position);

            if (existingPiece != null)
            {
                _pieces.Remove(existingPiece);
            }
        }

        public ulong[] GetPiecesArray()
        {
            var pieces = new ulong[12];

            foreach(var piece in _pieces)
            {
                var bitPosition = BitPositionConverter.ToULong(piece.Position);
                pieces[FastArray.GetPieceIndex(piece.Color, piece.Type)] |= bitPosition;
            }

            return pieces;
        }

        public bool[] GetCastlingArray()
        {
            var castling = new bool[4];

            castling[0] = _castling.WhiteShortCastling;
            castling[1] = _castling.WhiteLongCastling;
            castling[2] = _castling.BlackShortCastling;
            castling[3] = _castling.BlackLongCastling;

            return castling;
        }

        public ulong[] GetEnPassantArray()
        {
            ulong[] enPassant = new ulong[2];

            if (_enPassant.WhiteEnPassant != null)
            {
                enPassant[(int)Color.White] = BitPositionConverter.ToULong(_enPassant.WhiteEnPassant);
            }

            if (_enPassant.BlackEnPassant != null)
            {
                enPassant[(int)Color.Black] = BitPositionConverter.ToULong(_enPassant.BlackEnPassant);
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
            return _pieces.Where(p => p.Color == color).Select(p => p.Position).ToList();
        }

        public List<Position> GetAttacks()
        {
            var whiteAttacks = GetAttacks(Color.White);
            var blackAttacks = GetAttacks(Color.Black);

            return whiteAttacks.Concat(blackAttacks).ToList();
        }

        public List<Position> GetAttacks(Color color)
        {
            return _attacks.Where(p => p.Color == color).Select(p => p.To).ToList();
        }

        public List<Position> GetFieldAttackers(Position position)
        {
            var whiteAttacks = GetFieldAttackers(Color.White, position);
            var blackAttacks = GetFieldAttackers(Color.Black, position);

            return whiteAttacks.Concat(blackAttacks).ToList();
        }

        public List<Position> GetFieldAttackers(Color color, Position position)
        {
            return _attacks.Where(p => p.Color == color && p.To == position).Select(p => p.From).ToList();
        }
    }
}
