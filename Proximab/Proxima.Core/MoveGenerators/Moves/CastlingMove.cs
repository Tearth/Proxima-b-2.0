﻿using Proxima.Core.Boards;
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
                    rookFrom = Color == Color.White ? CastlingConstants.RightRookLSB : CastlingConstants.RightRookLSB << 56;
                    rookTo = rookFrom << 2;

                    break;
                }

                case CastlingType.Long:
                {
                    rookFrom = Color == Color.White ? CastlingConstants.LeftRookLSB : CastlingConstants.LeftRookLSB << 56;
                    rookTo = rookFrom >> 3;

                    break;
                }
            }

            CalculatePieceMove(bitboard, from, to);
            CalculatePieceMove(bitboard, PieceType.Rook, rookFrom, rookTo);
            RemoveCastlingPossibility(bitboard);
        }

        private void RemoveCastlingPossibility(Bitboard bitboard)
        {
            bitboard.Hash = IncrementalZobrist.RemoveCastlingPossibility(bitboard.Hash, bitboard.CastlingPossibility, Color, CastlingType.Short);
            bitboard.Hash = IncrementalZobrist.RemoveCastlingPossibility(bitboard.Hash, bitboard.CastlingPossibility, Color, CastlingType.Long);

            bitboard.CastlingPossibility[FastArray.GetCastlingIndex(Color, CastlingType.Short)] = false;
            bitboard.CastlingPossibility[FastArray.GetCastlingIndex(Color, CastlingType.Long)] = false;
            bitboard.IncEvaluation.Castling = IncrementalCastling.SetCastlingDone(bitboard.IncEvaluation.Castling, Color, GamePhase.Regular);

            bitboard.CastlingDone[(int)Color] = true;
        }
    }
}
