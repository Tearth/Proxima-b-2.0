using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Commons.Moves
{
    /// <summary>
    /// Represents a castling move.
    /// </summary>
    public class CastlingMove : Move
    {
        /// <summary>
        /// Gets the castling type.
        /// </summary>
        public CastlingType CastlingType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastlingMove"/> class.
        /// </summary>
        /// <param name="from">The source piece position.</param>
        /// <param name="to">The destination piece position.</param>
        /// <param name="piece">The piece type.</param>
        /// <param name="color">The piece color.</param>
        /// <param name="castlingType">The castling type.</param>
        public CastlingMove(Position from, Position to, PieceType piece, Color color, CastlingType castlingType) 
            : base(from, to, piece, color)
        {
            CastlingType = castlingType;
        }
    }
}
