using System.Diagnostics;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Hashing;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;
using Proxima.Core.Evaluation;

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
        public Position From { get; private set; }

        /// <summary>
        /// Gets the destination piece position.
        /// </summary>
        public Position To { get; private set; }

        /// <summary>
        /// Gets the piece type.
        /// </summary>
        public PieceType Piece { get; private set; }

        /// <summary>
        /// Gets the piece color.
        /// </summary>
        public Color Color { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Move"/> class.
        /// </summary>
        public Move()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Move"/> class.
        /// </summary>
        /// <param name="from">The source piece position.</param>
        /// <param name="to">The destination piece position.</param>
        /// <param name="piece">The piece type.</param>
        /// <param name="color">The piece color.</param>
        public Move(Position from, Position to, PieceType piece, Color color)
        {
            From = from;
            To = to;
            Piece = piece;
            Color = color;
        }

        public void Do(BitBoard bitBoard)
        {
            CalculateMove(bitBoard);
            CalculateCastling(bitBoard);
        }

        public abstract void CalculateMove(BitBoard bitBoard);

        public void CalculateCastling(BitBoard bitBoard)
        {
            if (Piece == PieceType.King)
            {
                var shortCastlingIndex = FastArray.GetCastlingIndex(Color, CastlingType.Short);
                var longCastlingIndex = FastArray.GetCastlingIndex(Color, CastlingType.Long);

                bitBoard.Hash = IncrementalZobrist.RemoveCastlingPossibility(bitBoard.Hash, bitBoard.CastlingPossibility, Color, CastlingType.Short);
                bitBoard.Hash = IncrementalZobrist.RemoveCastlingPossibility(bitBoard.Hash, bitBoard.CastlingPossibility, Color, CastlingType.Long);

                bitBoard.CastlingPossibility[shortCastlingIndex] = false;
                bitBoard.CastlingPossibility[longCastlingIndex] = false;
            }
            else if (Piece == PieceType.Rook)
            {
                if (From == new Position(1, 1) || From == new Position(1, 8))
                {
                    bitBoard.Hash = IncrementalZobrist.RemoveCastlingPossibility(bitBoard.Hash, bitBoard.CastlingPossibility, Color, CastlingType.Long);
                    bitBoard.CastlingPossibility[FastArray.GetCastlingIndex(Color, CastlingType.Long)] = false;
                }
                else if (From == new Position(8, 1) || From == new Position(8, 8))
                {
                    bitBoard.Hash = IncrementalZobrist.RemoveCastlingPossibility(bitBoard.Hash, bitBoard.CastlingPossibility, Color, CastlingType.Short);
                    bitBoard.CastlingPossibility[FastArray.GetCastlingIndex(Color, CastlingType.Short)] = false;
                }
            }
        }

        /// <summary>
        /// Checks if the move is valid.
        /// </summary>
        /// <returns>True if the move is valid, otherwise false.</returns>
        public bool IsValid()
        {
            return From.IsValid() && To.IsValid();
        }
    }
}
