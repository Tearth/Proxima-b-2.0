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

        public void GetMoves(PieceType pieceType, GeneratorParameters opt)
        {
            var piecesToParse = opt.Pieces[FastArray.GetIndex(opt.FriendlyColor, pieceType)];

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

                        opt.Moves.AddLast(new Move(from, to, pieceType, opt.FriendlyColor, moveType));
                    }

                    if ((opt.Mode & GeneratorMode.CalculateAttacks) != 0)
                    {
                        opt.Attacks[patternIndex] |= pieceLSB;
                        opt.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
                    }
                }
            }
        }

        public void GetCastling(PieceType pieceType, GeneratorParameters opt)
        {
            if (opt.FriendlyColor == Color.White)
                GetWhiteCastling(pieceType, opt);
            else
                GetBlackCastling(pieceType, opt);
        }

        void GetWhiteCastling(PieceType pieceType, GeneratorParameters opt)
        {
            if(opt.CastlingData.CastlingPossible[((int)Color.White * 2) + CastlingData.ShortCastling] &&
               IsCastlingAreaEmpty(WhiteShortCastlingMoveArea, opt.Occupancy) &&
               !IsCastlingAreaChecked(opt.EnemyColor, WhiteShortCastlingCheckArea, opt))
            {
                var move = new Move(InitialWhiteKingLSB, InitialWhiteKingLSB + new Position(2, 0), 
                                    PieceType.King, opt.FriendlyColor, MoveType.ShortCastling);

                opt.Moves.AddLast(move);
            }
            
            if(opt.CastlingData.CastlingPossible[((int)Color.White * 2) + CastlingData.LongCastling] &&
               IsCastlingAreaEmpty(WhiteLongCastlingMoveArea, opt.Occupancy) &&
               !IsCastlingAreaChecked(opt.EnemyColor, WhiteLongCastlingCheckArea, opt))
            {
                var move = new Move(InitialWhiteKingLSB, InitialWhiteKingLSB - new Position(2, 0),
                                    PieceType.King, opt.FriendlyColor, MoveType.LongCastling);

                opt.Moves.AddLast(move);
            }
        }

        void GetBlackCastling(PieceType pieceType, GeneratorParameters opt)
        {
            if (opt.CastlingData.CastlingPossible[((int)Color.Black * 2) + CastlingData.ShortCastling] &&
               IsCastlingAreaEmpty(BlackShortCastlingMoveArea, opt.Occupancy) &&
               !IsCastlingAreaChecked(opt.EnemyColor, BlackShortCastlingCheckArea, opt))
            {
                var move = new Move(InitialBlackKingLSB, InitialBlackKingLSB + new Position(2, 0),
                                    PieceType.King, opt.FriendlyColor, MoveType.ShortCastling);

                opt.Moves.AddLast(move);
            }

            if (opt.CastlingData.CastlingPossible[((int)Color.Black * 2) + CastlingData.LongCastling] &&
               IsCastlingAreaEmpty(BlackLongCastlingMoveArea, opt.Occupancy) &&
               !IsCastlingAreaChecked(opt.EnemyColor, BlackLongCastlingCheckArea, opt))
            {
                var move = new Move(InitialBlackKingLSB, InitialBlackKingLSB - new Position(2, 0),
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
