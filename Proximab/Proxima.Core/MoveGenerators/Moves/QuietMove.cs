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

        public override void Do(BitBoard bitBoard)
        {
            var from = BitPositionConverter.ToULong(From);
            var to = BitPositionConverter.ToULong(To);
            var change = from | to;

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

            bitBoard.Pieces[FastArray.GetPieceIndex(Color, Piece)] ^= change;
            bitBoard.Occupancy[(int)Color] ^= change;

            bitBoard.IncrementalEvaluation.Position = IncrementalPosition.RemovePiece(bitBoard.IncrementalEvaluation.Position, Color, Piece, from, GamePhase.Regular);
            bitBoard.IncrementalEvaluation.Position = IncrementalPosition.AddPiece(bitBoard.IncrementalEvaluation.Position, Color, Piece, to, GamePhase.Regular);

            bitBoard.Hash = IncrementalZobrist.AddOrRemovePiece(bitBoard.Hash, Color, Piece, from);
            bitBoard.Hash = IncrementalZobrist.AddOrRemovePiece(bitBoard.Hash, Color, Piece, to);
        }
    }
}
