using Proxima.Core.Boards;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.MoveGenerators.Moves
{
    /// <summary>
    /// Represents a kill move (on destination position is another enemy piece).
    /// </summary>
    public class KillMove : Move
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KillMove"/> class.
        /// </summary>
        /// <param name="from">The source piece position.</param>
        /// <param name="to">The destination piece position.</param>
        /// <param name="piece">The piece type.</param>
        /// <param name="color">The piece color.</param>
        public KillMove(Position from, Position to, PieceType piece, Color color)
            : base(from, to, piece, color)
        {
        }

        /// <summary>
        /// Calculates a kill move.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        public override void CalculateMove(Bitboard bitboard)
        {
            var from = BitPositionConverter.ToULong(From);
            var to = BitPositionConverter.ToULong(To);
            var enemyColor = ColorOperations.Invert(Color);

            CalculateKill(bitboard, enemyColor, to);
            CalculatePieceMove(bitboard, from, to);
        }
    }
}
