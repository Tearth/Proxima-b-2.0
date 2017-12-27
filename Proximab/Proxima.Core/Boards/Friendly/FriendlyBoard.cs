using System.Collections.Generic;
using System.Linq;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Boards.Friendly
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FriendlyBoard"/> class.
    /// </summary>
    public class FriendlyBoard
    {
        /// <summary>
        /// Gets the pieces list.
        /// </summary>
        public FriendlyPiecesList Pieces { get; private set; }

        /// <summary>
        /// Gets the attacks list.
        /// </summary>
        public FriendlyAttacksList Attacks { get; private set; }

        /// <summary>
        /// Gets the castling data.
        /// </summary>
        public FriendlyCastling Castling { get; private set; }

        /// <summary>
        /// Gets the en passant data.
        /// </summary>
        public FriendlyEnPassant EnPassant { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendlyBoard"/> class.
        /// </summary>
        public FriendlyBoard()
        {
            Pieces = new FriendlyPiecesList();
            Attacks = new FriendlyAttacksList();
            Castling = new FriendlyCastling();
            EnPassant = new FriendlyEnPassant();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendlyBoard"/> class.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        public FriendlyBoard(Bitboard bitboard)
        {
            Pieces = new FriendlyPiecesList(bitboard.Pieces);
            Attacks = new FriendlyAttacksList(bitboard.Attacks, Pieces);
            Castling = new FriendlyCastling(bitboard.CastlingPossibility, bitboard.CastlingDone);
            EnPassant = new FriendlyEnPassant(bitboard.EnPassant);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendlyBoard"/> class.
        /// </summary>
        /// <param name="pieces">The pieces list.</param>
        /// <param name="castling">The castling data.</param>
        /// <param name="enPassant">The en passant data.</param>
        public FriendlyBoard(FriendlyPiecesList pieces, FriendlyCastling castling, FriendlyEnPassant enPassant)
        {
            Pieces = pieces;
            Castling = castling;
            EnPassant = enPassant;
        }

        /// <summary>
        /// Gets a piece on the specified position.
        /// </summary>
        /// <param name="position">The piece position.</param>
        /// <returns>The piece on the specified position (or null if field is empty).</returns>
        public FriendlyPiece GetPiece(Position position)
        {
            return Pieces.FirstOrDefault(p => p.Position == position);
        }

        /// <summary>
        /// Sets (or replaces) a piece.
        /// </summary>
        /// <param name="piece">The piece to set.</param>
        public void SetPiece(FriendlyPiece piece)
        {
            RemovePiece(piece.Position);
            Pieces.Add(piece);
        }

        /// <summary>
        /// Removes piece on the specified position (if possible).
        /// </summary>
        /// <param name="position">The piece position.</param>
        public void RemovePiece(Position position)
        {
            var existingPiece = Pieces.FirstOrDefault(p => p.Position == position);

            if (existingPiece != null)
            {
                Pieces.Remove(existingPiece);
            }
        }

        /// <summary>
        /// Gets a pieces array (for bitboards).
        /// </summary>
        /// <returns>The pieces array.</returns>
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

        /// <summary>
        /// Gets a castling possibility array (for bitboards).
        /// </summary>
        /// <returns>The castling possibility array.</returns>
        public bool[] GetCastlingPossibilityArray()
        {
            var castling = new bool[4];

            castling[0] = Castling.WhiteShortCastlingPossibility;
            castling[1] = Castling.WhiteLongCastlingPossibility;
            castling[2] = Castling.BlackShortCastlingPossibility;
            castling[3] = Castling.BlackLongCastlingPossibility;

            return castling;
        }

        /// <summary>
        /// Gets a castling done array (for bitboards).
        /// </summary>
        /// <returns>The castling done array.</returns>
        public bool[] GetCastlingDoneArray()
        {
            var castling = new bool[2];

            castling[0] = Castling.WhiteCastlingDone;
            castling[1] = Castling.BlackCastlingDone;

            return castling;
        }

        /// <summary>
        /// Gets a en passante array (for bitboards).
        /// </summary>
        /// <returns>The en passante array.</returns>
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

        /// <summary>
        /// Gets a list of occupied fields by all pieces.
        /// </summary>
        /// <returns>The list of occupied fields.</returns>
        public List<Position> GetOccupancy()
        {
            var whiteOccupancy = GetOccupancy(Color.White);
            var blackOccupancy = GetOccupancy(Color.Black);

            return whiteOccupancy.Concat(blackOccupancy).ToList();
        }

        /// <summary>
        /// Gets a list of occupied fields by pieces with the specified color.
        /// </summary>
        /// <param name="color">The player color.</param>
        /// <returns>The list of occupied fields.</returns>
        public List<Position> GetOccupancy(Color color)
        {
            return Pieces.Where(p => p.Color == color).Select(p => p.Position).ToList();
        }

        /// <summary>
        /// Gets a list of attacked fields by all pieces.
        /// </summary>
        /// <returns>The list of attacked fields.</returns>
        public List<Position> GetAttacks()
        {
            var whiteAttacks = GetAttacks(Color.White);
            var blackAttacks = GetAttacks(Color.Black);

            return whiteAttacks.Concat(blackAttacks).ToList();
        }

        /// <summary>
        /// Gets a list of attacked fields by pieces with the specified color.
        /// </summary>
        /// <param name="color">The player color.</param>
        /// <returns>The list of attacked fields.</returns>
        public List<Position> GetAttacks(Color color)
        {
            return Attacks.Where(p => p.Color == color).Select(p => p.To).ToList();
        }

        /// <summary>
        /// Gets a list of the all field attackers.
        /// </summary>
        /// <param name="position">The field position.</param>
        /// <returns>The list of field attackers.</returns>
        public List<Position> GetFieldAttackers(Position position)
        {
            var whiteAttacks = GetFieldAttackers(Color.White, position);
            var blackAttacks = GetFieldAttackers(Color.Black, position);

            return whiteAttacks.Concat(blackAttacks).ToList();
        }

        /// <summary>
        /// Gets a list of the field attackers with the specified color.
        /// </summary>
        /// <param name="color">The player color.</param>
        /// <param name="position">The field position.</param>
        /// <returns>The list of field attackers.</returns>
        public List<Position> GetFieldAttackers(Color color, Position position)
        {
            return Attacks.Where(p => p.Color == color && p.To == position).Select(p => p.From).ToList();
        }
    }
}
