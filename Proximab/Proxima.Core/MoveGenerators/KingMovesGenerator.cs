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
    public static class KingMovesGenerator
    {
        public static readonly Position InitialKingPosition = new Position(5, 1);

        public static void Calculate(GeneratorParameters opt)
        {
            var piecesToParse = opt.BitBoard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.King)];

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
                            opt.BitBoard.Moves.AddLast(new QuietMove(piecePosition, to, PieceType.King, opt.FriendlyColor));
                        }
                        else
                        {
                            opt.BitBoard.Moves.AddLast(new KillMove(piecePosition, to, PieceType.King, opt.FriendlyColor));
                        }
                    }

                    if ((opt.Mode & GeneratorMode.CalculateAttacks) != 0)
                    {
                        opt.BitBoard.Attacks[patternIndex] |= pieceLSB;
                        opt.BitBoard.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
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

                opt.BitBoard.Moves.AddLast(move);
            }

            if (IsCastlingPossible(CastlingType.Long, opt) &&
                IsKingOnPosition(kingLSB, opt) && IsRookOnPosition(leftRookLSB, opt) &&
                IsCastlingAreaEmpty(longMoveArea, opt.OccupancySummary) &&
               !IsCastlingAreaChecked(opt.EnemyColor, longCheckArea, opt))
            {
                var kingDestinationPosition = kingPosition - new Position(2, 0);
                var move = new CastlingMove(kingPosition, kingDestinationPosition, PieceType.King, opt.FriendlyColor, CastlingType.Long);

                opt.BitBoard.Moves.AddLast(move);
            }
        }

        private static bool IsCastlingPossible(CastlingType type, GeneratorParameters opt)
        {
            return opt.BitBoard.CastlingPossibility[FastArray.GetCastlingIndex(opt.FriendlyColor, CastlingType.Short)];
        }

        private static bool IsKingOnPosition(ulong kingLSB, GeneratorParameters opt)
        {
            return (opt.BitBoard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.King)] & kingLSB) != 0;
        }

        private static bool IsRookOnPosition(ulong rookLSB, GeneratorParameters opt)
        {
            return (opt.BitBoard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Rook)] & rookLSB) != 0;
        }

        private static bool IsCastlingAreaEmpty(ulong areaToCheck, ulong occupancy)
        {
            return (areaToCheck & occupancy) == 0;
        }

        private static bool IsCastlingAreaChecked(Color enemyColor, ulong areaToCheck, GeneratorParameters opt)
        {
            return (opt.BitBoard.AttacksSummary[(int)enemyColor] & areaToCheck) != 0;
        }
    }
}
