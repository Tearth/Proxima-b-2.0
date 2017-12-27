using Proxima.Core.Boards;
using Proxima.Core.Boards.Hashing;
using Proxima.Core.Commons;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;
using Proxima.Core.Evaluation.Castling;

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
        public CastlingType CastlingType { get; }

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

        /// <summary>
        /// Calculates a castling.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        public override void CalculateMove(Bitboard bitboard)
        {
            var from = BitPositionConverter.ToULong(From);
            var to = BitPositionConverter.ToULong(To);

            var rookFrom = 0ul;
            var rookTo = 0ul;

            switch (CastlingType)
            {
                case CastlingType.Short:
                {
                    rookFrom = Color == Color.White ? CastlingConstants.InitialRightRookBitboard : CastlingConstants.InitialRightRookBitboard << 56;
                    rookTo = rookFrom << 2;

                    break;
                }

                case CastlingType.Long:
                {
                    rookFrom = Color == Color.White ? CastlingConstants.InitialLeftRookBitboard : CastlingConstants.InitialLeftRookBitboard << 56;
                    rookTo = rookFrom >> 3;

                    break;
                }
            }

            CalculatePieceMove(bitboard, from, to);
            CalculatePieceMove(bitboard, PieceType.Rook, rookFrom, rookTo);
            RemoveCastlingPossibility(bitboard);
        }

        /// <summary>
        /// Removes castling possibility from the specified bitboard.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        private void RemoveCastlingPossibility(Bitboard bitboard)
        {
            IncrementalZobrist.RemoveCastlingPossibility(Color, CastlingType.Short, bitboard);
            IncrementalZobrist.RemoveCastlingPossibility(Color, CastlingType.Long, bitboard);

            bitboard.CastlingPossibility[FastArray.GetCastlingIndex(Color, CastlingType.Short)] = false;
            bitboard.CastlingPossibility[FastArray.GetCastlingIndex(Color, CastlingType.Long)] = false;
            IncrementalCastling.SetCastlingDone(bitboard, Color);

            bitboard.CastlingDone[(int)Color] = true;
        }
    }
}
