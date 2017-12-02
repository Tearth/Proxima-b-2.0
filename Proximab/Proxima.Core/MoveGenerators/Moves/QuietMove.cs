using Proxima.Core.Boards;
using Proxima.Core.Boards.Hashing;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;
using Proxima.Core.Evaluation;
using Proxima.Core.Evaluation.Position;

namespace Proxima.Core.MoveGenerators.Moves
{
    /// <summary>
    /// Represents a quiet move (without any kills, castlings, promotions etc.).
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

        public override void CalculateMove(Bitboard bitboard)
        {
            var from = BitPositionConverter.ToULong(From);
            var to = BitPositionConverter.ToULong(To);

            CalculatePieceMove(bitboard, from, to);
            CalculateEnPassant(bitboard);
        }

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

        private Position? GetEnPassantPosition()
        {
            if (From.Y == 2 && To.Y == 4)
            {
                return new Position(To.X, To.Y - 1);
            }
            if (From.Y == 7 && To.Y == 5)
            {
                return new Position(To.X, To.Y + 1);
            }

            return null;
        }
    }
}
