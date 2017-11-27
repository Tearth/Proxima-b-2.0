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

        public override void CalculateMove(BitBoard bitBoard)
        {
            var from = BitPositionConverter.ToULong(From);
            var to = BitPositionConverter.ToULong(To);
            var change = from | to;

            bitBoard.Pieces[FastArray.GetPieceIndex(Color, Piece)] ^= change;
            bitBoard.Occupancy[(int)Color] ^= change;

            bitBoard.IncrementalEvaluation.Position = IncrementalPosition.RemovePiece(bitBoard.IncrementalEvaluation.Position, Color, Piece, from, GamePhase.Regular);
            bitBoard.IncrementalEvaluation.Position = IncrementalPosition.AddPiece(bitBoard.IncrementalEvaluation.Position, Color, Piece, to, GamePhase.Regular);

            bitBoard.Hash = IncrementalZobrist.AddOrRemovePiece(bitBoard.Hash, Color, Piece, from);
            bitBoard.Hash = IncrementalZobrist.AddOrRemovePiece(bitBoard.Hash, Color, Piece, to);

            CalculateEnPassant(bitBoard);
        }

        void CalculateEnPassant(BitBoard bitBoard)
        {
            if (Piece == PieceType.Pawn)
            {
                if (From.Y == 2 && To.Y == 4)
                {
                    var enPassantPosition = new Position(To.X, To.Y - 1);
                    var enPassantLSB = BitPositionConverter.ToULong(enPassantPosition);

                    bitBoard.EnPassant[(int)Color.White] |= enPassantLSB;
                    bitBoard.Hash = IncrementalZobrist.AddEnPassant(bitBoard.Hash, Color.White, enPassantLSB);
                }
                if (From.Y == 7 && To.Y == 5)
                {
                    var enPassantPosition = new Position(To.X, To.Y + 1);
                    var enPassantLSB = BitPositionConverter.ToULong(enPassantPosition);

                    bitBoard.EnPassant[(int)Color.Black] |= enPassantLSB;
                    bitBoard.Hash = IncrementalZobrist.AddEnPassant(bitBoard.Hash, Color.Black, enPassantLSB);
                }
            }
        }
    }
}
