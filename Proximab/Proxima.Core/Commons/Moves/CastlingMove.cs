using Proxima.Core.Boards;
using Proxima.Core.Boards.Hashing;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;
using Proxima.Core.Evaluation;
using Proxima.Core.Evaluation.Castling;
using Proxima.Core.Evaluation.Position;
using Proxima.Core.MoveGenerators;

namespace Proxima.Core.Commons.Moves
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

        public override void Do(BitBoard bitBoard)
        {
            var from = BitPositionConverter.ToULong(From);
            var to = BitPositionConverter.ToULong(To);
            var change = from | to;

            switch (CastlingType)
            {
                case CastlingType.Short:
                {
                    var rookLSB = Color == Color.White ? KingMovesGenerator.WhiteRightRookLSB : KingMovesGenerator.BlackRightRookLSB;
                    var rookChange = rookLSB | (rookLSB << 2);

                    bitBoard.Pieces[FastArray.GetPieceIndex(Color, PieceType.Rook)] ^= rookChange;
                    bitBoard.Occupancy[(int)Color] ^= rookChange;

                    bitBoard.IncrementalEvaluation.Position = IncrementalPosition.RemovePiece(bitBoard.IncrementalEvaluation.Position, Color, PieceType.Rook, rookLSB, GamePhase.Regular);
                    bitBoard.IncrementalEvaluation.Position = IncrementalPosition.AddPiece(bitBoard.IncrementalEvaluation.Position, Color, PieceType.Rook, rookLSB << 2, GamePhase.Regular);

                    bitBoard.Hash = IncrementalZobrist.AddOrRemovePiece(bitBoard.Hash, Color, PieceType.Rook, rookLSB);
                    bitBoard.Hash = IncrementalZobrist.AddOrRemovePiece(bitBoard.Hash, Color, PieceType.Rook, rookLSB << 2);

                    break;
                }

                case CastlingType.Long:
                {
                    var rookLSB = Color == Color.White ? KingMovesGenerator.WhiteLeftRookLSB : KingMovesGenerator.BlackLeftRookLSB;
                    var rookChange = rookLSB | (rookLSB >> 3);

                    bitBoard.Pieces[FastArray.GetPieceIndex(Color, PieceType.Rook)] ^= rookChange;
                    bitBoard.Occupancy[(int)Color] ^= rookChange;

                    bitBoard.IncrementalEvaluation.Position = IncrementalPosition.RemovePiece(bitBoard.IncrementalEvaluation.Position, Color, PieceType.Rook, rookLSB, GamePhase.Regular);
                    bitBoard.IncrementalEvaluation.Position = IncrementalPosition.AddPiece(bitBoard.IncrementalEvaluation.Position, Color, PieceType.Rook, rookLSB >> 3, GamePhase.Regular);

                    bitBoard.Hash = IncrementalZobrist.AddOrRemovePiece(bitBoard.Hash, Color, PieceType.Rook, rookLSB);
                    bitBoard.Hash = IncrementalZobrist.AddOrRemovePiece(bitBoard.Hash, Color, PieceType.Rook, rookLSB >> 3);

                    break;
                }
            }

            bitBoard.Pieces[FastArray.GetPieceIndex(Color, Piece)] ^= change;
            bitBoard.Occupancy[(int)Color] ^= change;

            bitBoard.Hash = IncrementalZobrist.AddOrRemovePiece(bitBoard.Hash, Color, Piece, from);
            bitBoard.Hash = IncrementalZobrist.AddOrRemovePiece(bitBoard.Hash, Color, Piece, to);
            bitBoard.Hash = IncrementalZobrist.RemoveCastlingPossibility(bitBoard.Hash, bitBoard.CastlingPossibility, Color, CastlingType.Short);
            bitBoard.Hash = IncrementalZobrist.RemoveCastlingPossibility(bitBoard.Hash, bitBoard.CastlingPossibility, Color, CastlingType.Long);

            bitBoard.IncrementalEvaluation.Position = IncrementalPosition.RemovePiece(bitBoard.IncrementalEvaluation.Position, Color, Piece, from, GamePhase.Regular);
            bitBoard.IncrementalEvaluation.Position = IncrementalPosition.AddPiece(bitBoard.IncrementalEvaluation.Position, Color, Piece, to, GamePhase.Regular);

            bitBoard.CastlingPossibility[FastArray.GetCastlingIndex(Color, CastlingType.Short)] = false;
            bitBoard.CastlingPossibility[FastArray.GetCastlingIndex(Color, CastlingType.Long)] = false;

            bitBoard.IncrementalEvaluation.Castling = IncrementalCastling.SetCastlingDone(bitBoard.IncrementalEvaluation.Castling, Color, GamePhase.Regular);
            bitBoard.CastlingDone[(int)Color] = true;
        }
    }
}
