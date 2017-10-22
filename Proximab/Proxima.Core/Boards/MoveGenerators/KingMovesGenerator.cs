using Proxima.Core.Boards.PatternGenerators;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Moves;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Boards.MoveGenerators
{
    public class KingMovesGenerator : MovesParserBase
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

        public readonly Position InitialWhiteKingLSB = new Position(5, 1);
        public readonly Position InitialBlackKingLSB = new Position(5, 8);    

        public KingMovesGenerator()
        {

        }

        public void GetMoves(GeneratorParameters opt)
        {
            var piecesToParse = opt.Pieces[FastArray.GetIndex(opt.FriendlyColor, PieceType.King)];

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref piecesToParse);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                var pattern = PatternsContainer.KingPattern[pieceIndex];

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    if ((opt.Mode & GeneratorMode.CalculateMoves) != 0 &&
                        (patternLSB & opt.FriendlyOccupancy) == 0)
                    {
                        var from = BitPositionConverter.ToPosition(pieceIndex);
                        var to = BitPositionConverter.ToPosition(patternIndex);
                        var moveType = GetMoveType(patternLSB, opt.EnemyOccupancy);

                        opt.Moves.AddLast(new Move(from, to, PieceType.King, opt.FriendlyColor, moveType));
                    }

                    if ((opt.Mode & GeneratorMode.CalculateAttacks) != 0)
                    {
                        opt.Attacks[patternIndex] |= pieceLSB;
                        opt.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
                    }
                }
            }
        }

        public void GetCastling(GeneratorParameters opt)
        {
            Position initialKingLSB;

            var shortMoveArea = 0ul;
            var shortCheckArea = 0ul;
            var longMoveArea = 0ul;
            var longCheckArea = 0ul;

            if(opt.FriendlyColor == Color.White)
            {
                initialKingLSB = InitialWhiteKingLSB;

                shortMoveArea = WhiteShortCastlingMoveArea;
                shortCheckArea =  WhiteShortCastlingCheckArea;

                longMoveArea = WhiteLongCastlingMoveArea;
                longCheckArea = WhiteLongCastlingCheckArea;
            }
            else
            {
                initialKingLSB = InitialBlackKingLSB;

                shortMoveArea = BlackShortCastlingMoveArea;
                shortCheckArea = BlackShortCastlingCheckArea;

                longMoveArea = BlackLongCastlingMoveArea;
                longCheckArea = BlackLongCastlingCheckArea;
            }

            if (opt.Castling[FastArray.GetIndex(opt.FriendlyColor, CastlingType.Short)] &&
               IsCastlingAreaEmpty(shortMoveArea, opt.Occupancy) &&
               !IsCastlingAreaChecked(opt.EnemyColor, shortCheckArea, opt))
            {
                var move = new Move(initialKingLSB, initialKingLSB + new Position(2, 0), 
                                    PieceType.King, opt.FriendlyColor, MoveType.ShortCastling);

                opt.Moves.AddLast(move);
            }
            
            if(opt.Castling[FastArray.GetIndex(opt.FriendlyColor, CastlingType.Long)] &&
               IsCastlingAreaEmpty(longMoveArea, opt.Occupancy) &&
               !IsCastlingAreaChecked(opt.EnemyColor, longCheckArea, opt))
            {
                var move = new Move(initialKingLSB, initialKingLSB - new Position(2, 0),
                                    PieceType.King, opt.FriendlyColor, MoveType.LongCastling);

                opt.Moves.AddLast(move);
            }
        }
        
        bool IsCastlingAreaEmpty(ulong areaToCheck, ulong occupancy)
        {
            return (areaToCheck & occupancy) == 0;
        }

        bool IsCastlingAreaChecked(Color enemyColor, ulong areaToCheck, GeneratorParameters opt)
        {
            return (opt.AttacksSummary[(int)enemyColor] & areaToCheck) != 0;
        }
    }
}
