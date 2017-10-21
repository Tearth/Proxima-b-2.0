using Proxima.Core.Boards.PatternGenerators;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Moves;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Boards.MoveGenerators
{
    public class KingMovesGenerator : MovesParserBase
    {
        const ulong WhiteShortCastlingCheckArea = 0x0eul;
        const ulong WhiteLongCastlingCheckArea = 0x38ul;
        const ulong BlackShortCastlingCheckArea = 0x0e00000000000000ul;
        const ulong BlackLongCastlingCheckArea = 0x3800000000000000ul;

        const ulong WhiteShortCastlingMoveArea = 0x06ul;
        const ulong WhiteLongCastlingMoveArea = 0x30ul;
        const ulong BlackShortCastlingMoveArea = 0x0600000000000000ul;
        const ulong BlackLongCastlingMoveArea = 0x3000000000000000ul;

        readonly Position InitialWhiteKingLSB = new Position(5, 1);
        readonly Position InitialBlackKingLSB = new Position(5, 8);    

        public KingMovesGenerator()
        {

        }

        public void GetMoves(PieceType pieceType, GeneratorParameters opt)
        {
            var piecesToParse = opt.Pieces[(int)opt.Color, (int)pieceType];

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref piecesToParse);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                var pattern = PatternsContainer.KingPattern[pieceIndex] & ~opt.FriendlyOccupancy;

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    if ((opt.Mode & GeneratorMode.CalculateMoves) != 0)
                    {
                        var from = BitPositionConverter.ToPosition(pieceIndex);
                        var to = BitPositionConverter.ToPosition(patternIndex);
                        var moveType = GetMoveType(patternLSB, opt.EnemyOccupancy);

                        opt.Moves.AddLast(new Move(from, to, pieceType, opt.Color, moveType));
                    }

                    if ((opt.Mode & GeneratorMode.CalculateAttacks) != 0)
                    {
                        opt.Attacks[patternIndex] |= pieceLSB;
                        opt.AttacksSummary[(int)opt.Color] |= patternLSB;
                    }
                }
            }
        }

        public void GetCastling(PieceType pieceType, GeneratorParameters opt)
        {
            if (opt.Color == Color.White)
                GetWhiteCastling(pieceType, opt);
        }

        void GetWhiteCastling(PieceType pieceType, GeneratorParameters opt)
        {
            if (!opt.CastlingData.WhiteShortCastlingPossible && !opt.CastlingData.WhiteLongCastlingPossible)
                return;

            var enemyColor = ColorOperations.Invert(opt.Color);
            
            if(opt.CastlingData.WhiteShortCastlingPossible &&
               IsCastlingAreaEmpty(WhiteShortCastlingMoveArea, opt.Occupancy) &&
               !IsCastlingAreaChecked(enemyColor, WhiteShortCastlingCheckArea, opt))
            {
                var move = new Move(InitialWhiteKingLSB, InitialWhiteKingLSB + new Position(2, 0), 
                                    PieceType.King, opt.Color, MoveType.ShortCastling);

                opt.Moves.AddLast(move);
            }
            
            if(opt.CastlingData.WhiteLongCastlingPossible &&
               IsCastlingAreaEmpty(WhiteLongCastlingMoveArea, opt.Occupancy) &&
               !IsCastlingAreaChecked(enemyColor, WhiteLongCastlingCheckArea, opt))
            {
                var move = new Move(InitialWhiteKingLSB, InitialWhiteKingLSB - new Position(2, 0),
                                    PieceType.King, opt.Color, MoveType.LongCastling);

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
