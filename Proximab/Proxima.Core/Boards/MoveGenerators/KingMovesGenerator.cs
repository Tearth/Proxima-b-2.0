using Proxima.Core.Boards.PatternGenerators;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Moves;
using System.Collections.Generic;

namespace Proxima.Core.Boards.MoveGenerators
{
    public class KingMovesGenerator : MovesParserBase
    {
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
                        opt.Attacks[(int)opt.Color, patternIndex] |= pieceLSB;
                        opt.AttacksSummary[(int)opt.Color] |= patternLSB;
                    }
                }
            }
        }

        public void GetCastling(PieceType pieceType, GeneratorParameters opt)
        {
            if (opt.Color == Color.White)
                GetWhiteCastlnig(pieceType, opt);
        }

        void GetWhiteCastlnig(PieceType pieceType, GeneratorParameters opt)
        {
            if (!opt.CastlingData.WhiteShortCastlingPossible && !opt.CastlingData.WhiteLongCastlingPossible)
                return;

            var enemyColor = ColorOperations.Invert(opt.Color);
            var piecesToParse = opt.Pieces[(int)opt.Color, (int)pieceType];

            var pieceLSB = BitOperations.GetLSB(ref piecesToParse);
            var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
        }
    }
}
