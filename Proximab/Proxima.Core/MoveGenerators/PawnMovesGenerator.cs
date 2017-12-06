using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.MoveGenerators
{
    /// <summary>
    /// Represents a set of methods to generating pawn moves.
    /// </summary>
    public static class PawnMovesGenerator
    {
        /// <summary>
        /// Generates available moves.
        /// </summary>
        /// <param name="pieceType">The piece type.</param>
        /// <param name="opt">The generator parameters.</param>
        public static void Generate(GeneratorParameters opt)
        {
            CalculateMovesForSinglePush(opt);
            CalculateMovesForDoublePush(opt);
            CalculateMovesForRightAttack(opt);
            CalculateMovesForLeftAttack(opt);
        }

        private static void CalculateMovesForSinglePush(GeneratorParameters opt)
        {
            if ((opt.Mode & GeneratorMode.CalculateMoves) == 0)
            {
                return;
            }

            var piecesToParse = opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Pawn)];

            var pattern = 0ul;
            var promotionLine = 0ul;

            if (opt.FriendlyColor == Color.White)
            {
                pattern = piecesToParse << 8;
                promotionLine = BitConstants.HRank;
            }
            else
            {
                pattern = piecesToParse >> 8;
                promotionLine = BitConstants.ARank;
            }

            pattern &= ~opt.OccupancySummary;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(pattern);
                pattern = BitOperations.PopLSB(pattern);

                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var pieceLSB = opt.FriendlyColor == Color.White ? patternLSB >> 8 : patternLSB << 8;
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                var from = BitPositionConverter.ToPosition(pieceIndex);
                var to = BitPositionConverter.ToPosition(patternIndex);

                if ((patternLSB & promotionLine) != 0)
                {
                    opt.Bitboard.Moves.AddLast(new PromotionMove(from, to, PieceType.Pawn, opt.FriendlyColor, PieceType.Queen));
                    opt.Bitboard.Moves.AddLast(new PromotionMove(from, to, PieceType.Pawn, opt.FriendlyColor, PieceType.Rook));
                    opt.Bitboard.Moves.AddLast(new PromotionMove(from, to, PieceType.Pawn, opt.FriendlyColor, PieceType.Bishop));
                    opt.Bitboard.Moves.AddLast(new PromotionMove(from, to, PieceType.Pawn, opt.FriendlyColor, PieceType.Knight));
                }
                else
                {
                    opt.Bitboard.Moves.AddLast(new QuietMove(from, to, PieceType.Pawn, opt.FriendlyColor));
                }
            }
        }

        private static void CalculateMovesForDoublePush(GeneratorParameters opt)
        {
            if ((opt.Mode & GeneratorMode.CalculateMoves) == 0)
            {
                return;
            }

            var piecesToParse = opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Pawn)];
            var validPieces = 0ul;
            var pattern = 0ul;

            if (opt.FriendlyColor == Color.White)
            {
                validPieces = piecesToParse & BitConstants.BRank;
                validPieces &= ~opt.OccupancySummary >> 8;
                pattern = validPieces << 16;
            }
            else
            {
                validPieces = piecesToParse & BitConstants.GRank;
                validPieces &= ~opt.OccupancySummary << 8;
                pattern = validPieces >> 16;
            }

            pattern &= ~opt.OccupancySummary;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(pattern);
                pattern = BitOperations.PopLSB(pattern);

                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var pieceLSB = opt.FriendlyColor == Color.White ? patternLSB >> 16 : patternLSB << 16;
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                var from = BitPositionConverter.ToPosition(pieceIndex);
                var to = BitPositionConverter.ToPosition(patternIndex);

                opt.Bitboard.Moves.AddLast(new QuietMove(from, to, PieceType.Pawn, opt.FriendlyColor));
            }
        }

        private static void CalculateMovesForRightAttack(GeneratorParameters opt)
        {
            var piecesToParse = opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Pawn)];
            var validPieces = piecesToParse & ~BitConstants.HFile;

            var pattern = opt.FriendlyColor == Color.White ? validPieces << 7 : validPieces >> 9;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(pattern);
                pattern = BitOperations.PopLSB(pattern);

                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var pieceLSB = opt.FriendlyColor == Color.White ? patternLSB >> 7 : patternLSB << 9;
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                if ((opt.Mode & GeneratorMode.CalculateMoves) != 0)
                {
                    var piecePosition = BitPositionConverter.ToPosition(pieceIndex);
                    var enPassantField = opt.Bitboard.EnPassant[(int)opt.EnemyColor] & patternLSB;

                    if ((patternLSB & opt.EnemyOccupancy) != 0 || enPassantField != 0)
                    {
                        var to = BitPositionConverter.ToPosition(patternIndex);

                        if (enPassantField == 0)
                        {
                            opt.Bitboard.Moves.AddLast(new KillMove(piecePosition, to, PieceType.Pawn, opt.FriendlyColor));
                        }
                        else
                        {
                            opt.Bitboard.Moves.AddLast(new EnPassantMove(piecePosition, to, PieceType.Pawn, opt.FriendlyColor));
                        }
                    }
                }

                if ((opt.Mode & GeneratorMode.CalculateAttacks) != 0)
                {
                    opt.Bitboard.Attacks[patternIndex] |= pieceLSB;
                    opt.Bitboard.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
                }
            }
        }

        private static void CalculateMovesForLeftAttack(GeneratorParameters opt)
        {
            var piecesToParse = opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Pawn)];
            var validPieces = piecesToParse & ~BitConstants.AFile;

            var pattern = opt.FriendlyColor == Color.White ? validPieces << 9 : validPieces >> 7;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(pattern);
                pattern = BitOperations.PopLSB(pattern);

                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var pieceLSB = opt.FriendlyColor == Color.White ? patternLSB >> 9 : patternLSB << 7;
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                if ((opt.Mode & GeneratorMode.CalculateMoves) != 0)
                {
                    var piecePosition = BitPositionConverter.ToPosition(pieceIndex);
                    var enPassantField = opt.Bitboard.EnPassant[(int)opt.EnemyColor] & patternLSB;

                    if ((patternLSB & opt.EnemyOccupancy) != 0 || enPassantField != 0)
                    {
                        var to = BitPositionConverter.ToPosition(patternIndex);

                        if (enPassantField == 0)
                        {
                            opt.Bitboard.Moves.AddLast(new KillMove(piecePosition, to, PieceType.Pawn, opt.FriendlyColor));
                        }
                        else
                        {
                            opt.Bitboard.Moves.AddLast(new EnPassantMove(piecePosition, to, PieceType.Pawn, opt.FriendlyColor));
                        }
                    }
                }

                if ((opt.Mode & GeneratorMode.CalculateAttacks) != 0)
                {
                    opt.Bitboard.Attacks[patternIndex] |= pieceLSB;
                    opt.Bitboard.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
                }
            }
        }
    }
}
