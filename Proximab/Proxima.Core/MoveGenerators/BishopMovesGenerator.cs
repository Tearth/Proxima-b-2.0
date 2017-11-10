using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Moves;
using Proxima.Core.Commons.Performance;
using Proxima.Core.MoveGenerators.MagicBitboards;
using Proxima.Core.MoveGenerators.PatternGenerators;

namespace Proxima.Core.MoveGenerators
{
    public class BishopMovesGenerator
    {
        public BishopMovesGenerator()
        {

        }

        public void Calculate(PieceType pieceType, GeneratorParameters opt)
        {
            var piecesToParse = opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, pieceType)];
            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref piecesToParse);

                var patternContainer = CalculateMoves(pieceType, pieceLSB, opt);
                CalculateAttacks(pieceType, pieceLSB, patternContainer, opt);
            }
        }

        ulong CalculateMoves(PieceType pieceType, ulong pieceLSB, GeneratorParameters opt)
        {
            if ((opt.Mode & GeneratorMode.CalculateMoves) == 0)
                return 0;

            var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
            var piecePosition = BitPositionConverter.ToPosition(pieceIndex);

            var pattern = MagicContainer.GetBishopAttacks(pieceIndex, opt.Occupancy);
            var excludeFromAttacks = pattern;
            pattern &= ~opt.FriendlyOccupancy;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var to = BitPositionConverter.ToPosition(patternIndex);

                if((patternLSB & opt.EnemyOccupancy) == 0)
                {
                    opt.Moves.AddLast(new QuietMove(piecePosition, to, pieceType, opt.FriendlyColor));
                }
                else
                {
                    opt.Moves.AddLast(new KillMove(piecePosition, to, pieceType, opt.FriendlyColor));
                }

                opt.Attacks[patternIndex] |= pieceLSB;
                opt.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
            }

            return excludeFromAttacks;
        }

        void CalculateAttacks(PieceType pieceType, ulong pieceLSB, ulong movesPattern, GeneratorParameters opt)
        {
            if ((opt.Mode & GeneratorMode.CalculateAttacks) == 0)
                return;

            var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
            var blockersToRemove = opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Bishop)] |
                                   opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Queen)];

            var piecesToParse = opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, pieceType)];
            var allPiecesOccupancy = opt.Occupancy & ~blockersToRemove;

            var pattern = MagicContainer.GetBishopAttacks(pieceIndex, allPiecesOccupancy);
            pattern ^= movesPattern;
            pattern &= ~opt.FriendlyOccupancy;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                opt.Attacks[patternIndex] |= pieceLSB;
                opt.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
            }
        }
    }
}