using Proxima.Core.Boards;
using Proxima.Core.Boards.Hashing;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.MoveGenerators.Moves
{
    /// <summary>
    /// Represents a quiet move (without any kills, castling, promotions etc.).
    /// </summary>
    public class QuietMove : Move
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuietMove"/> class.
        /// </summary>
        /// <param name="from">The source piece position.</param>
        /// <param name="to">The destination piece position.</param>
        /// <param name="piece">The piece type.</param>
        /// <param name="color">The piece color.</param>
        public QuietMove(Position from, Position to, PieceType piece, Color color)
            : base(from, to, piece, color)
        {
        }

        /// <summary>
        /// Calculates a quiet move.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        public override void CalculateMove(Bitboard bitboard)
        {
            var from = BitPositionConverter.ToULong(From);
            var to = BitPositionConverter.ToULong(To);

            CalculatePieceMove(bitboard, from, to);
            CalculateEnPassant(bitboard);
        }

        /// <summary>
        /// Calculates en passant fields if current piece type is pawn.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        private void CalculateEnPassant(Bitboard bitboard)
        {
            if (Piece == PieceType.Pawn)
            {
                var enPassantPosition = GetEnPassantPosition();
                if (enPassantPosition.HasValue)
                {
                    var enPassantLSB = BitPositionConverter.ToULong(enPassantPosition.Value);

                    bitboard.EnPassant[(int)Color] |= enPassantLSB;
                    IncrementalZobrist.AddEnPassant(Color, enPassantLSB, bitboard);
                }
            }
        }

        /// <summary>
        /// Calculates en passant position for enemy pawns if possible.
        /// </summary>
        /// <returns>The en passant position if possible, otherwise null.</returns>
        private Position? GetEnPassantPosition()
        {
            switch (Color)
            {
                case Color.White when From.Y == 2 && To.Y == 4:
                {
                    return new Position(To.X, To.Y - 1);
                }
                case Color.Black when From.Y == 7 && To.Y == 5:
                {
                    return new Position(To.X, To.Y + 1);
                }
            }

            return null;
        }
    }
}
