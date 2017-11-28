using Proxima.Core.Boards;
using Proxima.Core.Boards.Hashing;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;
using Proxima.Core.Evaluation;
using Proxima.Core.Evaluation.Castling;
using Proxima.Core.Evaluation.Position;
using Proxima.Core.MoveGenerators;

namespace Proxima.Core.MoveGenerators.Moves
{
    /// <summary>
    /// Represents a castling 
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

        public override void CalculateMove(BitBoard bitBoard)
        {
            var from = BitPositionConverter.ToULong(From);
            var to = BitPositionConverter.ToULong(To);

            var rookFrom = 0ul;
            var rookTo = 0ul;

            switch (CastlingType)
            {
                case CastlingType.Short:
                {
                    rookFrom = Color == Color.White ? KingMovesGenerator.RightRookLSB : KingMovesGenerator.RightRookLSB << 56;
                    rookTo = rookFrom << 2;

                    break;
                }

                case CastlingType.Long:
                {
                    rookFrom = Color == Color.White ? KingMovesGenerator.LeftRookLSB : KingMovesGenerator.LeftRookLSB << 56;
                    rookTo = rookFrom >> 3;

                    break;
                }
            }

            CalculatePieceMove(bitBoard, from, to);
            CalculatePieceMove(bitBoard, PieceType.Rook, rookFrom, rookTo);
            RemoveCastlingPossibility(bitBoard);
        }

        private void RemoveCastlingPossibility(BitBoard bitBoard)
        {
            bitBoard.Hash = IncrementalZobrist.RemoveCastlingPossibility(bitBoard.Hash, bitBoard.CastlingPossibility, Color, CastlingType.Short);
            bitBoard.Hash = IncrementalZobrist.RemoveCastlingPossibility(bitBoard.Hash, bitBoard.CastlingPossibility, Color, CastlingType.Long);

            bitBoard.CastlingPossibility[FastArray.GetCastlingIndex(Color, CastlingType.Short)] = false;
            bitBoard.CastlingPossibility[FastArray.GetCastlingIndex(Color, CastlingType.Long)] = false;
            bitBoard.IncrementalEvaluation.Castling = IncrementalCastling.SetCastlingDone(bitBoard.IncrementalEvaluation.Castling, Color, GamePhase.Regular);

            bitBoard.CastlingDone[(int)Color] = true;
        }
    }
}
