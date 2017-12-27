using System.Diagnostics;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Hashing;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;
using Proxima.Core.Evaluation.Material;
using Proxima.Core.Evaluation.Position;

namespace Proxima.Core.MoveGenerators.Moves
{
    /// <summary>
    /// Represents a base class for all move types.
    /// </summary>
    [DebuggerDisplay("{Color} {Piece} from [{From.X} {From.Y}] to [{To.X} {To.Y}]")]
    public abstract class Move
    {
        /// <summary>
        /// Gets the source piece position.
        /// </summary>
        public Position From { get; }

        /// <summary>
        /// Gets the destination piece position.
        /// </summary>
        public Position To { get; }

        /// <summary>
        /// Gets the piece type.
        /// </summary>
        public PieceType Piece { get; }

        /// <summary>
        /// Gets the piece color.
        /// </summary>
        public Color Color { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Move"/> class.
        /// </summary>
        /// <param name="from">The source piece position.</param>
        /// <param name="to">The destination piece position.</param>
        /// <param name="piece">The piece type.</param>
        /// <param name="color">The piece color.</param>
        protected Move(Position from, Position to, PieceType piece, Color color)
        {
            From = from;
            To = to;
            Piece = piece;
            Color = color;
        }

        /// <summary>
        /// Checks if the move is valid.
        /// </summary>
        /// <returns>True if the move is valid, otherwise false.</returns>
        public bool IsValid()
        {
            return From.IsValid() && To.IsValid();
        }

        /// <summary>
        /// Does move (specified by the derived class).
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        public void Do(Bitboard bitboard)
        {
            CalculateMove(bitboard);
            CalculateCastling(bitboard);
        }

        /// <summary>
        /// Calculates move specified in the derived class.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        public abstract void CalculateMove(Bitboard bitboard);

        /// <summary>
        /// Converts move to its string representation.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return From.ToString() + " -> " + To.ToString();
        }

        /// <summary>
        /// Helper method for derived classes, calculates move for current piece type with the specified parameters.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="from">The piece source position.</param>
        /// <param name="to">The piece destination position.</param>
        protected void CalculatePieceMove(Bitboard bitboard, ulong from, ulong to)
        {
            CalculatePieceMove(bitboard, Piece, from, Piece, to);
        }

        /// <summary>
        /// Helper method for derived classes, calculates move with the specified parameters.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="pieceType">The piece type.</param>
        /// <param name="from">The piece source position.</param>
        /// <param name="to">The piece destination position.</param>
        protected void CalculatePieceMove(Bitboard bitboard, PieceType pieceType, ulong from, ulong to)
        {
            CalculatePieceMove(bitboard, pieceType, from, pieceType, to);
        }

        /// <summary>
        /// Helper method for derived classes, calculates move with the specified parameters.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="pieceFrom">The source piece type.</param>
        /// <param name="from">The piece source position.</param>
        /// <param name="pieceTo">The destination piece type.</param>
        /// <param name="to">The piece destination position.</param>
        protected void CalculatePieceMove(Bitboard bitboard, PieceType pieceFrom, ulong from, PieceType pieceTo, ulong to)
        {
            bitboard.Pieces[FastArray.GetPieceIndex(Color, pieceFrom)] &= ~from;
            bitboard.Pieces[FastArray.GetPieceIndex(Color, pieceTo)] |= to;
            bitboard.Occupancy[(int)Color] ^= from | to;

            IncrementalPosition.RemovePiece(bitboard, Color, pieceFrom, from);
            IncrementalPosition.AddPiece(bitboard, Color, pieceTo, to);

            IncrementalZobrist.AddOrRemovePiece(Color, pieceFrom, from, bitboard);
            IncrementalZobrist.AddOrRemovePiece(Color, pieceTo, to, bitboard);
        }

        /// <summary>
        /// Removes killed piece from the specified bitboard.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="enemyColor">The enemy color.</param>
        /// <param name="fieldLsb">The bitboard with set field.</param>
        protected void CalculateKill(Bitboard bitboard, Color enemyColor, ulong fieldLsb)
        {
            for (int piece = 0; piece < 6; piece++)
            {
                var index = FastArray.GetPieceIndex(enemyColor, (PieceType)piece);
                if ((bitboard.Pieces[index] & fieldLsb) != 0)
                {
                    bitboard.Pieces[index] &= ~fieldLsb;
                    bitboard.Occupancy[(int)enemyColor] &= ~fieldLsb;

                    IncrementalMaterial.RemovePiece(bitboard, enemyColor, (PieceType)piece);
                    IncrementalPosition.RemovePiece(bitboard, enemyColor, (PieceType)piece, fieldLsb);
                    IncrementalZobrist.AddOrRemovePiece(enemyColor, (PieceType)piece, fieldLsb, bitboard);

                    break;
                }
            }
        }

        /// <summary>
        /// Removes castling possibility if needed.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        private void CalculateCastling(Bitboard bitboard)
        {
            if (Piece == PieceType.King)
            {
                var shortCastlingIndex = FastArray.GetCastlingIndex(Color, CastlingType.Short);
                var longCastlingIndex = FastArray.GetCastlingIndex(Color, CastlingType.Long);

                IncrementalZobrist.RemoveCastlingPossibility(Color, CastlingType.Short, bitboard);
                IncrementalZobrist.RemoveCastlingPossibility(Color, CastlingType.Long, bitboard);

                bitboard.CastlingPossibility[shortCastlingIndex] = false;
                bitboard.CastlingPossibility[longCastlingIndex] = false;
            }
            else if (Piece == PieceType.Rook)
            {
                if (From == new Position(1, 1) || From == new Position(1, 8))
                {
                    IncrementalZobrist.RemoveCastlingPossibility(Color, CastlingType.Long, bitboard);
                    bitboard.CastlingPossibility[FastArray.GetCastlingIndex(Color, CastlingType.Long)] = false;
                }
                else if (From == new Position(8, 1) || From == new Position(8, 8))
                {
                    IncrementalZobrist.RemoveCastlingPossibility(Color, CastlingType.Short, bitboard);
                    bitboard.CastlingPossibility[FastArray.GetCastlingIndex(Color, CastlingType.Short)] = false;
                }
            }
        }
    }
}
