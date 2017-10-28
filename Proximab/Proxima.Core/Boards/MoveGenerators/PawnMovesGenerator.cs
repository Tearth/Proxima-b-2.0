using Proxima.Core.Commons;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Moves;
using Proxima.Core.Commons.Performance;
using System.Collections.Generic;

namespace Proxima.Core.Boards.MoveGenerators
{
    public class PawnMovesGenerator
    {
        public PawnMovesGenerator()
        {

        }

        public void Calculate(GeneratorParameters opt)
        {
            CalculateMovesForSinglePush(opt);
            CalculateMovesForDoublePush(opt);
            CalculateMovesForRightAttack(opt);
            CalculateMovesForLeftAttack(opt);
        }

        void CalculateMovesForSinglePush(GeneratorParameters opt)
        {
            if ((opt.Mode & GeneratorMode.CalculateMoves) == 0)
            {
                return;
            }

            var piecesToParse = opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Pawn)];

            var pattern = 0ul;
            var promotionLine = 0ul;

            if (opt.FriendlyColor == Color.White)
            {
                pattern = piecesToParse << 8;
                promotionLine = BitConstants.HRank;
            }
            else
            {
                pattern = piecesToParse >> 8;
                promotionLine = BitConstants.ARank;
            }

            pattern &= ~opt.Occupancy;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var pieceLSB = opt.FriendlyColor == Color.White ? patternLSB >> 8 : patternLSB << 8;
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                var from = BitPositionConverter.ToPosition(pieceIndex);
                var to = BitPositionConverter.ToPosition(patternIndex);

                if((patternLSB & promotionLine) != 0)
                {
                    opt.Moves.AddLast(new PromotionMove(from, to, PieceType.Pawn, opt.FriendlyColor, PieceType.Queen));
                    opt.Moves.AddLast(new PromotionMove(from, to, PieceType.Pawn, opt.FriendlyColor, PieceType.Rook));
                    opt.Moves.AddLast(new PromotionMove(from, to, PieceType.Pawn, opt.FriendlyColor, PieceType.Bishop));
                    opt.Moves.AddLast(new PromotionMove(from, to, PieceType.Pawn, opt.FriendlyColor, PieceType.Knight));
                }
                else
                {
                    opt.Moves.AddLast(new QuietMove(from, to, PieceType.Pawn, opt.FriendlyColor));
                }

            }
        }

        void CalculateMovesForDoublePush(GeneratorParameters opt)
        {
            if ((opt.Mode & GeneratorMode.CalculateMoves) == 0)
            {
                return;
            }

            var piecesToParse = opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Pawn)];
            var validPieces = 0ul;
            var pattern = 0ul;

            if(opt.FriendlyColor == Color.White)
            {
                validPieces = piecesToParse & BitConstants.BRank;
                validPieces &= ~opt.Occupancy >> 8;
                pattern = validPieces << 16;
            }
            else
            {
                validPieces = piecesToParse & BitConstants.GRank;
                validPieces &= ~opt.Occupancy << 8;
                pattern = validPieces >> 16;
            }
            
            pattern &= ~opt.Occupancy;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var pieceLSB = opt.FriendlyColor == Color.White ? patternLSB >> 16 : patternLSB << 16;
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                var from = BitPositionConverter.ToPosition(pieceIndex);
                var to = BitPositionConverter.ToPosition(patternIndex);

                opt.Moves.AddLast(new QuietMove(from, to, PieceType.Pawn, opt.FriendlyColor));
            }
        }

        void CalculateMovesForRightAttack(GeneratorParameters opt)
        {
            var piecesToParse = opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Pawn)];
            var validPieces = piecesToParse & ~BitConstants.HFile;

            var pattern = opt.FriendlyColor == Color.White ? validPieces << 7 : validPieces >> 9;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var pieceLSB = opt.FriendlyColor == Color.White ? patternLSB >> 7 : patternLSB << 9;
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
                var piecePosition = BitPositionConverter.ToPosition(pieceIndex);

                if ((opt.Mode & GeneratorMode.CalculateMoves) != 0)
                {
                    var enPassantField = opt.EnPassant[(int)opt.EnemyColor] & patternLSB;

                    if ((patternLSB & opt.EnemyOccupancy) != 0 || enPassantField != 0)
                    {
                        var to = BitPositionConverter.ToPosition(patternIndex);

                        if (enPassantField == 0)
                        {
                            opt.Moves.AddLast(new KillMove(piecePosition, to, PieceType.Pawn, opt.FriendlyColor));
                        }
                        else
                        {
                            opt.Moves.AddLast(new EnPassantMove(piecePosition, to, PieceType.Pawn, opt.FriendlyColor));
                        }
                    }
                }

                if ((opt.Mode & GeneratorMode.CalculateAttacks) != 0)
                {
                    opt.Attacks[patternIndex] |= pieceLSB;
                    opt.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
                }
            }
        }

        void CalculateMovesForLeftAttack(GeneratorParameters opt)
        {
            var piecesToParse = opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Pawn)];
            var validPieces = piecesToParse & ~BitConstants.AFile;

            var pattern = opt.FriendlyColor == Color.White ? validPieces << 9 : validPieces >> 7;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var pieceLSB = opt.FriendlyColor == Color.White ? patternLSB >> 9 : patternLSB << 7;
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
                var piecePosition = BitPositionConverter.ToPosition(pieceIndex);

                if ((opt.Mode & GeneratorMode.CalculateMoves) != 0)
                {
                    var enPassantField = opt.EnPassant[(int)opt.EnemyColor] & patternLSB;

                    if ((patternLSB & opt.EnemyOccupancy) != 0 || enPassantField != 0)
                    {
                        var to = BitPositionConverter.ToPosition(patternIndex);

                        if (enPassantField == 0)
                        {
                            opt.Moves.AddLast(new KillMove(piecePosition, to, PieceType.Pawn, opt.FriendlyColor));
                        }
                        else
                        {
                            opt.Moves.AddLast(new EnPassantMove(piecePosition, to, PieceType.Pawn, opt.FriendlyColor));
                        }
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
}
