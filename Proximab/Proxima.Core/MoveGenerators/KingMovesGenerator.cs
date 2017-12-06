using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators.Moves;
using Proxima.Core.MoveGenerators.PatternGenerators;

namespace Proxima.Core.MoveGenerators
{
    /// <summary>
    /// Represents a set of methods to generating king moves.
    /// </summary>
    public static class KingMovesGenerator
    {
        private static readonly Position InitialKingPosition = new Position(5, 1);

        /// <summary>
        /// Generates available moves.
        /// </summary>
        /// <param name="opt">The generator parameters.</param>
        public static void Generate(GeneratorParameters opt)
        {
            var piecesToParse = opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.King)];

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(piecesToParse);
                piecesToParse = BitOperations.PopLSB(piecesToParse);

                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
                var piecePosition = BitPositionConverter.ToPosition(pieceIndex);

                var pattern = PatternsContainer.KingPattern[pieceIndex];

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(pattern);
                    pattern = BitOperations.PopLSB(pattern);

                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    if ((opt.Mode & GeneratorMode.CalculateMoves) != 0 &&
                        (patternLSB & opt.FriendlyOccupancy) == 0)
                    {
                        var to = BitPositionConverter.ToPosition(patternIndex);

                        if ((patternLSB & opt.EnemyOccupancy) == 0)
                        {
                            opt.Bitboard.Moves.AddLast(new QuietMove(piecePosition, to, PieceType.King, opt.FriendlyColor));
                        }
                        else
                        {
                            opt.Bitboard.Moves.AddLast(new KillMove(piecePosition, to, PieceType.King, opt.FriendlyColor));
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

        public static void CalculateCastling(GeneratorParameters opt)
        {
            var kingLSB = CastlingConstants.KingLSB;
            var leftRookLSB = CastlingConstants.LeftRookLSB;
            var rightRookLSB = CastlingConstants.RightRookLSB;
            var shortMoveArea = CastlingConstants.ShortCastlingMoveArea;
            var shortCheckArea = CastlingConstants.ShortCastlingCheckArea;
            var longMoveArea = CastlingConstants.LongCastlingMoveArea;
            var longCheckArea = CastlingConstants.LongCastlingCheckArea;
            var kingPosition = InitialKingPosition;

            if (opt.FriendlyColor == Color.Black)
            {
                kingLSB <<= 56;
                leftRookLSB <<= 56;
                rightRookLSB <<= 56;
                shortMoveArea <<= 56;
                shortCheckArea <<= 56;
                longMoveArea <<= 56;
                longCheckArea <<= 56;

                kingPosition += new Position(0, 7);
            }

            if (IsCastlingPossible(CastlingType.Short, opt) &&
                IsKingOnPosition(kingLSB, opt) && IsRookOnPosition(rightRookLSB, opt) &&
                IsCastlingAreaEmpty(shortMoveArea, opt.OccupancySummary) &&
               !IsCastlingAreaChecked(opt.EnemyColor, shortCheckArea, opt))
            {
                var kingDestinationPosition = kingPosition + new Position(2, 0);
                var move = new CastlingMove(kingPosition, kingDestinationPosition, PieceType.King, opt.FriendlyColor, CastlingType.Short);

                opt.Bitboard.Moves.AddLast(move);
            }

            if (IsCastlingPossible(CastlingType.Long, opt) &&
                IsKingOnPosition(kingLSB, opt) && IsRookOnPosition(leftRookLSB, opt) &&
                IsCastlingAreaEmpty(longMoveArea, opt.OccupancySummary) &&
               !IsCastlingAreaChecked(opt.EnemyColor, longCheckArea, opt))
            {
                var kingDestinationPosition = kingPosition - new Position(2, 0);
                var move = new CastlingMove(kingPosition, kingDestinationPosition, PieceType.King, opt.FriendlyColor, CastlingType.Long);

                opt.Bitboard.Moves.AddLast(move);
            }
        }

        private static bool IsCastlingPossible(CastlingType type, GeneratorParameters opt)
        {
            return opt.Bitboard.CastlingPossibility[FastArray.GetCastlingIndex(opt.FriendlyColor, CastlingType.Short)];
        }

        private static bool IsKingOnPosition(ulong kingLSB, GeneratorParameters opt)
        {
            return (opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.King)] & kingLSB) != 0;
        }

        private static bool IsRookOnPosition(ulong rookLSB, GeneratorParameters opt)
        {
            return (opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Rook)] & rookLSB) != 0;
        }

        private static bool IsCastlingAreaEmpty(ulong areaToCheck, ulong occupancy)
        {
            return (areaToCheck & occupancy) == 0;
        }

        private static bool IsCastlingAreaChecked(Color enemyColor, ulong areaToCheck, GeneratorParameters opt)
        {
            return (opt.Bitboard.AttacksSummary[(int)enemyColor] & areaToCheck) != 0;
        }
    }
}
