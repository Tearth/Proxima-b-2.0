using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Moves;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators.PatternGenerators;

namespace Proxima.Core.MoveGenerators
{
    public static class KingMovesGenerator
    {
        public const ulong WhiteRightRookLSB = 0x01;
        public const ulong WhiteLeftRookLSB = 0x80;
        public const ulong BlackRightRookLSB = 0x0100000000000000;
        public const ulong BlackLeftRookLSB = 0x8000000000000000;

        public const ulong WhiteShortCastlingCheckArea = 0x0eul;
        public const ulong WhiteLongCastlingCheckArea  = 0x38ul;
        public const ulong BlackShortCastlingCheckArea = 0x0e00000000000000ul;
        public const ulong BlackLongCastlingCheckArea  = 0x3800000000000000ul;

        public const ulong WhiteShortCastlingMoveArea = 0x06ul;
        public const ulong WhiteLongCastlingMoveArea  = 0x70ul;
        public const ulong BlackShortCastlingMoveArea = 0x0600000000000000ul;
        public const ulong BlackLongCastlingMoveArea  = 0x7000000000000000ul;

        public static readonly Position InitialWhiteKingPosition = new Position(5, 1);
        public static readonly Position InitialBlackKingPosition = new Position(5, 8);    

        public static void Calculate(GeneratorParameters opt)
        {
            var piecesToParse = opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.King)];

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
                            opt.Moves.AddLast(new QuietMove(piecePosition, to, PieceType.King, opt.FriendlyColor));
                        }
                        else
                        {
                            opt.Moves.AddLast(new KillMove(piecePosition, to, PieceType.King, opt.FriendlyColor));
                        }
                    }

                    if ((opt.Mode & GeneratorMode.CalculateAttacks) != 0)
                    {
                        opt.Attacks[patternIndex] |= pieceLSB;
                        opt.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
                    }
                }
            }
        }

        public static void CalculateCastling(GeneratorParameters opt)
        {
            if (!opt.CastlingPossibility[FastArray.GetCastlingIndex(opt.FriendlyColor, CastlingType.Short)] &&
                !opt.CastlingPossibility[FastArray.GetCastlingIndex(opt.FriendlyColor, CastlingType.Long)])
                return;

            Position initialKingPosition;

            var shortMoveArea = 0ul;
            var shortCheckArea = 0ul;
            var longMoveArea = 0ul;
            var longCheckArea = 0ul;

            if(opt.FriendlyColor == Color.White)
            {
                initialKingPosition = InitialWhiteKingPosition;

                shortMoveArea = WhiteShortCastlingMoveArea;
                shortCheckArea =  WhiteShortCastlingCheckArea;

                longMoveArea = WhiteLongCastlingMoveArea;
                longCheckArea = WhiteLongCastlingCheckArea;
            }
            else
            {
                initialKingPosition = InitialBlackKingPosition;

                shortMoveArea = BlackShortCastlingMoveArea;
                shortCheckArea = BlackShortCastlingCheckArea;

                longMoveArea = BlackLongCastlingMoveArea;
                longCheckArea = BlackLongCastlingCheckArea;
            }

            if (opt.CastlingPossibility[FastArray.GetCastlingIndex(opt.FriendlyColor, CastlingType.Short)] &&
               IsCastlingAreaEmpty(shortMoveArea, opt.Occupancy) &&
               !IsCastlingAreaChecked(opt.EnemyColor, shortCheckArea, opt))
            {
                var to = initialKingPosition + new Position(2, 0);
                var move = new CastlingMove(initialKingPosition, to, PieceType.King, opt.FriendlyColor, CastlingType.Short);

                opt.Moves.AddLast(move);
            }
            
            if(opt.CastlingPossibility[FastArray.GetCastlingIndex(opt.FriendlyColor, CastlingType.Long)] &&
               IsCastlingAreaEmpty(longMoveArea, opt.Occupancy) &&
               !IsCastlingAreaChecked(opt.EnemyColor, longCheckArea, opt))
            {
                var to = initialKingPosition - new Position(2, 0);
                var move = new CastlingMove(initialKingPosition, to, PieceType.King, opt.FriendlyColor, CastlingType.Long);

                opt.Moves.AddLast(move);
            }
        }

        static bool IsCastlingAreaEmpty(ulong areaToCheck, ulong occupancy)
        {
            return (areaToCheck & occupancy) == 0;
        }

        static bool IsCastlingAreaChecked(Color enemyColor, ulong areaToCheck, GeneratorParameters opt)
        {
            return (opt.AttacksSummary[(int)enemyColor] & areaToCheck) != 0;
        }
    }
}
