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

                    if ((opt.Mode & GeneratorMode.CalculateMoves) != 0 && (patternLSB & opt.FriendlyOccupancy) == 0)
                    {
                        var to = BitPositionConverter.ToPosition(patternIndex);

                        if ((patternLSB & opt.EnemyOccupancy) == 0 && !opt.QuiescenceSearch)
                        {
                            opt.Bitboard.Moves.AddLast(new QuietMove(piecePosition, to, PieceType.King, opt.FriendlyColor));
                        }
                        else if((patternLSB & opt.EnemyOccupancy) != 0)
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

        /// <summary>
        /// Calculates castling moves.
        /// </summary>
        /// <param name="opt">The generator parameters.</param>
        public static void CalculateCastling(GeneratorParameters opt)
        {
            var kingLSB = CastlingConstants.InitialKingBitboard;
            var leftRookLSB = CastlingConstants.InitialLeftRookBitboard;
            var rightRookLSB = CastlingConstants.InitialRightRookBitboard;
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

        /// <summary>
        /// Checks if castling with the specified type is possible.
        /// </summary>
        /// <param name="type">The castling type.</param>
        /// <param name="opt">The generator parameters.</param>
        /// <returns>True if castling is possible, otherwise false.</returns>
        private static bool IsCastlingPossible(CastlingType type, GeneratorParameters opt)
        {
            return opt.Bitboard.CastlingPossibility[FastArray.GetCastlingIndex(opt.FriendlyColor, type)];
        }

        /// <summary>
        /// Checks if king is on initial position and can be a part of castling.
        /// </summary>
        /// <param name="kingBitboard">The bitboard with set bit at king position.</param>
        /// <param name="opt">The generator parameters.</param>
        /// <returns>True if king is on initial position, otherwise false.</returns>
        private static bool IsKingOnPosition(ulong kingBitboard, GeneratorParameters opt)
        {
            return (opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.King)] & kingBitboard) != 0;
        }

        /// <summary>
        /// Checks if rook is on initial position and can be a part of castling.
        /// </summary>
        /// <param name="rookBitboard">The bitboard with set bit at rook position.</param>
        /// <param name="opt">The generator parameters.</param>
        /// <returns>True if rook is on initial position, otherwise false.</returns>
        private static bool IsRookOnPosition(ulong rookBitboard, GeneratorParameters opt)
        {
            return (opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Rook)] & rookBitboard) != 0;
        }

        /// <summary>
        /// Checks if castling area (all fields that are on the way of king or rook during castling) is empty.
        /// </summary>
        /// <param name="areaToCheck">The bitboard with set bit at all fields to check.</param>
        /// <param name="occupancy">The bitboard with occupancy.</param>
        /// <returns>True if castling area is empty, otherwise false.</returns>
        private static bool IsCastlingAreaEmpty(ulong areaToCheck, ulong occupancy)
        {
            return (areaToCheck & occupancy) == 0;
        }

        /// <summary>
        /// Checks if check area (all fields that are on the way of king during castling) is checked.
        /// </summary>
        /// <param name="enemyColor">The enemy color.</param>
        /// <param name="areaToCheck">The bitboard with set bit at all fields to check.</param>
        /// <param name="opt">The generator parameters.</param>
        /// <returns>True if any of the castling area is checked, otherwise false.</returns>
        private static bool IsCastlingAreaChecked(Color enemyColor, ulong areaToCheck, GeneratorParameters opt)
        {
            return (opt.Bitboard.AttacksSummary[(int)enemyColor] & areaToCheck) != 0;
        }
    }
}
