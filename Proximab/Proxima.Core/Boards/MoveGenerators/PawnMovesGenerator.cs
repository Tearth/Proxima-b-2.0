using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Moves;
using System.Collections.Generic;

namespace Proxima.Core.Boards.MoveGenerators
{
    public class PawnMovesGenerator : MovesParserBase
    {
        public PawnMovesGenerator()
        {

        }

        public void GetMoves(PieceType pieceType, GeneratorParameters opt)
        {
            CalculateMovesForSinglePush(pieceType, opt);
            CalculateMovesForDoublePush(pieceType, opt);
            CalculateMovesForRightAttack(pieceType, opt);
            CalculateMovesForLeftAttack(pieceType, opt);
        }

        void CalculateMovesForSinglePush(PieceType pieceType, GeneratorParameters opt)
        {
            if ((opt.Mode & GeneratorMode.CalculateMoves) == 0)
                return;

            var piecesToParse = opt.Pieces[((int)opt.FriendlyColor * 6) + (int)pieceType];
            var pattern = opt.FriendlyColor == Color.White ? piecesToParse << 8 : piecesToParse >> 8;
            pattern &= ~opt.Occupancy;

            while(pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var pieceLSB = opt.FriendlyColor == Color.White ? patternLSB >> 8 : patternLSB << 8;
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                var from = BitPositionConverter.ToPosition(pieceIndex);
                var to = BitPositionConverter.ToPosition(patternIndex);
                var moveType = MoveType.Quiet;

                opt.Moves.AddLast(new Move(from, to, pieceType, opt.FriendlyColor, moveType));
            }
        }

        void CalculateMovesForDoublePush(PieceType pieceType, GeneratorParameters opt)
        {
            if ((opt.Mode & GeneratorMode.CalculateMoves) == 0)
                return;

            var piecesToParse = opt.Pieces[((int)opt.FriendlyColor * 6) + (int)pieceType];
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
                var moveType = MoveType.DoublePush;

                opt.Moves.AddLast(new Move(from, to, pieceType, opt.FriendlyColor, moveType));
            }
        }

        void CalculateMovesForRightAttack(PieceType pieceType, GeneratorParameters opt)
        {
            var piecesToParse = opt.Pieces[((int)opt.FriendlyColor * 6) + (int)pieceType];
            var validPieces = piecesToParse & ~BitConstants.HFile;

            var pattern = opt.FriendlyColor == Color.White ? validPieces << 7 : validPieces >> 9;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var pieceLSB = opt.FriendlyColor == Color.White ? patternLSB >> 7 : patternLSB << 9;
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                if ((opt.Mode & GeneratorMode.CalculateMoves) != 0)
                {
                    var enPassantField = opt.EnPassant[(int)opt.EnemyColor] & patternLSB;

                    if ((patternLSB & opt.EnemyOccupancy) != 0 || enPassantField != 0)
                    {
                        var from = BitPositionConverter.ToPosition(pieceIndex);
                        var to = BitPositionConverter.ToPosition(patternIndex);
                        var moveType = enPassantField == 0 ? MoveType.Kill : MoveType.EnPassant;

                        opt.Moves.AddLast(new Move(from, to, pieceType, opt.FriendlyColor, moveType));
                    }
                }

                if ((opt.Mode & GeneratorMode.CalculateAttacks) != 0)
                {
                    opt.Attacks[patternIndex] |= pieceLSB;
                    opt.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
                }
            }
        }

        void CalculateMovesForLeftAttack(PieceType pieceType, GeneratorParameters opt)
        {
            var piecesToParse = opt.Pieces[((int)opt.FriendlyColor * 6) + (int)pieceType];
            var validPieces = piecesToParse & ~BitConstants.AFile;

            var pattern = opt.FriendlyColor == Color.White ? validPieces << 9 : validPieces >> 7;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var pieceLSB = opt.FriendlyColor == Color.White ? patternLSB >> 9 : patternLSB << 7;
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                if ((opt.Mode & GeneratorMode.CalculateMoves) != 0)
                {
                    var enPassantField = opt.EnPassant[(int)opt.EnemyColor] & patternLSB;

                    if ((patternLSB & opt.EnemyOccupancy) != 0 || enPassantField != 0)
                    {
                        var from = BitPositionConverter.ToPosition(pieceIndex);
                        var to = BitPositionConverter.ToPosition(patternIndex);
                        var moveType = enPassantField == 0 ? MoveType.Kill : MoveType.EnPassant;

                        opt.Moves.AddLast(new Move(from, to, pieceType, opt.FriendlyColor, moveType));
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
