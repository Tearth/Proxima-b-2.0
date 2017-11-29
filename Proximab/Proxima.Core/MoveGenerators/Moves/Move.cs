using System.Diagnostics;
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

        /// <summary>
        /// Checks if the move is valid.
        /// </summary>
        /// <returns>True if the move is valid, otherwise false.</returns>
        public bool IsValid()
        {
            return From.IsValid() && To.IsValid();
        }

        public void Do(BitBoard bitBoard)
        {
            CalculateMove(bitBoard);
            CalculateCastling(bitBoard);
        }

        public abstract void CalculateMove(BitBoard bitBoard);

        protected void CalculatePieceMove(BitBoard bitBoard, ulong from, ulong to)
        {
            CalculatePieceMove(bitBoard, Piece, from, Piece, to);
        }

        protected void CalculatePieceMove(BitBoard bitBoard, PieceType pieceType, ulong from, ulong to)
        {
            CalculatePieceMove(bitBoard, pieceType, from, pieceType, to);
        }

        protected void CalculatePieceMove(BitBoard bitBoard, PieceType pieceFrom, ulong from, PieceType pieceTo, ulong to)
        {
            bitBoard.Pieces[FastArray.GetPieceIndex(Color, pieceFrom)] &= ~from;
            bitBoard.Pieces[FastArray.GetPieceIndex(Color, pieceTo)] |= to;
            bitBoard.Occupancy[(int)Color] ^= from | to;

            bitBoard.IncEvaluation.Position = IncrementalPosition.RemovePiece(bitBoard.IncEvaluation.Position, Color, pieceFrom, from, GamePhase.Regular);
            bitBoard.IncEvaluation.Position = IncrementalPosition.AddPiece(bitBoard.IncEvaluation.Position, Color, pieceTo, to, GamePhase.Regular);

            bitBoard.Hash = IncrementalZobrist.AddOrRemovePiece(bitBoard.Hash, Color, pieceFrom, from);
            bitBoard.Hash = IncrementalZobrist.AddOrRemovePiece(bitBoard.Hash, Color, pieceTo, to);
        }

        private void CalculateCastling(BitBoard bitBoard)
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
    }
}
