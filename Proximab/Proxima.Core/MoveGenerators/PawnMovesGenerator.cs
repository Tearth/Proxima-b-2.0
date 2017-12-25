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
        /// <param name="opt">The generator parameters.</param>
        public static void Generate(GeneratorParameters opt)
        {
            CalculateMovesForSinglePush(opt);
            CalculateMovesForDoublePush(opt);
            CalculateDiagonalAttacks(7, 9, BitConstants.HFile, opt);
            CalculateDiagonalAttacks(9, 7, BitConstants.AFile, opt);
        }

        /// <summary>
        /// Calculates single push moves.
        /// </summary>
        /// <param name="opt">The generator parameters.</param>
        private static void CalculateMovesForSinglePush(GeneratorParameters opt)
        {
            if ((opt.Mode & GeneratorMode.CalculateMoves) == 0)
            {
                return;
            }

            var piecesToParse = opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Pawn)];

            var pattern = 0ul;
            var promotionLine = GetPromotionLine(opt.FriendlyColor);

            pattern = opt.FriendlyColor == Color.White ? piecesToParse << 8 : piecesToParse >> 8;
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

                if ((patternLSB & promotionLine) == 0 && !opt.QuiescenceSearch)
                {
                    opt.Bitboard.Moves.AddLast(new QuietMove(from, to, PieceType.Pawn, opt.FriendlyColor));
                }
                else if ((patternLSB & promotionLine) != 0)
                {
                    opt.Bitboard.Moves.AddLast(new PromotionMove(from, to, PieceType.Pawn, opt.FriendlyColor, PieceType.Queen, false));
                    opt.Bitboard.Moves.AddLast(new PromotionMove(from, to, PieceType.Pawn, opt.FriendlyColor, PieceType.Rook, false));
                    opt.Bitboard.Moves.AddLast(new PromotionMove(from, to, PieceType.Pawn, opt.FriendlyColor, PieceType.Bishop, false));
                    opt.Bitboard.Moves.AddLast(new PromotionMove(from, to, PieceType.Pawn, opt.FriendlyColor, PieceType.Knight, false));
                }
            }
        }

        /// <summary>
        /// Calculates double push moves.
        /// </summary>
        /// <param name="opt">The generator parameters.</param>
        private static void CalculateMovesForDoublePush(GeneratorParameters opt)
        {
            if ((opt.Mode & GeneratorMode.CalculateMoves) == 0 || opt.QuiescenceSearch)
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

        /// <summary>
        /// Calculates diagonal attacks (and moves if possible).
        /// </summary>
        /// <param name="leftAttackShift">The left attack shift.</param>
        /// <param name="rightAttackShift">The right attacks shift.</param>
        /// <param name="ignoreFields">The bitboard with fields to ignore (white and black pieces will have different ones).</param>
        /// <param name="opt">The generator parameters.</param>
        private static void CalculateDiagonalAttacks(int leftAttackShift, int rightAttackShift, ulong ignoreFields, GeneratorParameters opt)
        {
            var piecesToParse = opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Pawn)];
            var validPieces = piecesToParse & ~ignoreFields;

            var pattern = opt.FriendlyColor == Color.White ? validPieces << leftAttackShift : validPieces >> rightAttackShift;
            var promotionLine = GetPromotionLine(opt.FriendlyColor);

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(pattern);
                pattern = BitOperations.PopLSB(pattern);

                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var pieceLSB = opt.FriendlyColor == Color.White ? patternLSB >> leftAttackShift : patternLSB << rightAttackShift;
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                if ((opt.Mode & GeneratorMode.CalculateMoves) != 0)
                {
                    var piecePosition = BitPositionConverter.ToPosition(pieceIndex);
                    var enPassantField = opt.Bitboard.EnPassant[(int)opt.EnemyColor] & patternLSB;
                    
                    if ((patternLSB & opt.EnemyOccupancy) != 0 || enPassantField != 0)
                    {
                        var to = BitPositionConverter.ToPosition(patternIndex);

                        if (enPassantField != 0)
                        {
                            opt.Bitboard.Moves.AddLast(new EnPassantMove(piecePosition, to, PieceType.Pawn, opt.FriendlyColor));          
                        }
                        else if ((patternLSB & promotionLine) != 0)
                        {
                            opt.Bitboard.Moves.AddLast(new PromotionMove(piecePosition, to, PieceType.Pawn, opt.FriendlyColor, PieceType.Queen, true));
                            opt.Bitboard.Moves.AddLast(new PromotionMove(piecePosition, to, PieceType.Pawn, opt.FriendlyColor, PieceType.Rook, true));
                            opt.Bitboard.Moves.AddLast(new PromotionMove(piecePosition, to, PieceType.Pawn, opt.FriendlyColor, PieceType.Bishop, true));
                            opt.Bitboard.Moves.AddLast(new PromotionMove(piecePosition, to, PieceType.Pawn, opt.FriendlyColor, PieceType.Knight, true));
                        }
                        else
                        {
                            opt.Bitboard.Moves.AddLast(new KillMove(piecePosition, to, PieceType.Pawn, opt.FriendlyColor));
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

        /// <summary>
        /// Gets the promotion line for the specified player color.
        /// </summary>
        /// <param name="color">The player color.</param>
        /// <returns>The bitboard with set bits indicating the promotion area.</returns>
        private static ulong GetPromotionLine(Color color)
        {
            return color == Color.White ? BitConstants.HRank : BitConstants.ARank;
        }
    }
}
