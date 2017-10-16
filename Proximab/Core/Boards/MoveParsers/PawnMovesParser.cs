using Core.Commons;
using Core.Commons.Colors;
using Core.Commons.Moves;
using System.Collections.Generic;

namespace Core.Boards.MoveParsers
{
    public class PawnMovesParser : MovesParserBase
    {
        public PawnMovesParser()
        {

        }

        public void GetMoves(PieceType pieceType, Color color, GeneratorMode mode, ulong[,] pieces, ulong[] enPassant, OccupancyContainer occupancyContainer, LinkedList<Move> moves, ulong[,] attacks)
        {
            var piecesToParse = pieces[(int)color, (int)pieceType];

            CalculateMovesForSinglePush(pieceType, color, mode, piecesToParse, occupancyContainer, moves);
            CalculateMovesForDoublePush(pieceType, color, mode, piecesToParse, occupancyContainer, moves);
            CalculateMovesForRightAttack(pieceType, color, mode, piecesToParse, enPassant, occupancyContainer, moves, attacks);
            CalculateMovesForLeftAttack(pieceType, color, mode, piecesToParse, enPassant, occupancyContainer, moves, attacks);
        }

        void CalculateMovesForSinglePush(PieceType pieceType, Color color, GeneratorMode mode, ulong piecesToParse, OccupancyContainer occupancyContainer, LinkedList<Move> moves)
        {
            if (mode != GeneratorMode.CalculateAll)
                return;

            var pattern = color == Color.White ? piecesToParse << 8 : piecesToParse >> 8;
            pattern &= ~occupancyContainer.Occupancy;

            while(pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var pieceLSB = color == Color.White ? patternLSB >> 8 : patternLSB << 8;
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                var from = BitPositionConverter.ToPosition(pieceIndex);
                var to = BitPositionConverter.ToPosition(patternIndex);
                var moveType = MoveType.Quiet;

                moves.AddLast(new Move(from, to, pieceType, color, moveType));
            }
        }

        void CalculateMovesForDoublePush(PieceType pieceType, Color color, GeneratorMode mode, ulong piecesToParse, OccupancyContainer occupancyContainer, LinkedList<Move> moves)
        {
            if (mode != GeneratorMode.CalculateAll)
                return;

            var validPieces = 0ul;
            var pattern = 0ul;

            if(color == Color.White)
            {
                validPieces = piecesToParse & BitConstants.BRank;
                validPieces &= ~occupancyContainer.Occupancy >> 8;
                pattern = validPieces << 16;
            }
            else
            {
                validPieces = piecesToParse & BitConstants.GRank;
                validPieces &= ~occupancyContainer.Occupancy << 8;
                pattern = validPieces >> 16;
            }
            
            pattern &= ~occupancyContainer.Occupancy;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var pieceLSB = color == Color.White ? patternLSB >> 16 : patternLSB << 16;
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                var from = BitPositionConverter.ToPosition(pieceIndex);
                var to = BitPositionConverter.ToPosition(patternIndex);
                var moveType = MoveType.DoublePush;

                moves.AddLast(new Move(from, to, pieceType, color, moveType));
            }
        }

        void CalculateMovesForRightAttack(PieceType pieceType, Color color, GeneratorMode mode, ulong piecesToParse, ulong[] enPassant, OccupancyContainer occupancyContainer, LinkedList<Move> moves, ulong[,] attacks)
        {
            var validPieces = piecesToParse & ~BitConstants.HFile;
            var enemyColor = ColorOperations.Invert(color);

            var pattern = color == Color.White ? validPieces << 7 : validPieces >> 9;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var pieceLSB = color == Color.White ? patternLSB >> 7 : patternLSB << 9;
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
                var enPassantField = enPassant[(int)enemyColor] & patternLSB;

                if (mode == GeneratorMode.CalculateAll)
                {
                    if ((patternLSB & occupancyContainer.EnemyOccupancy) != 0 || enPassantField != 0)
                    {
                        var from = BitPositionConverter.ToPosition(pieceIndex);
                        var to = BitPositionConverter.ToPosition(patternIndex);
                        var moveType = enPassantField == 0 ? MoveType.Kill : MoveType.EnPassant;

                        moves.AddLast(new Move(from, to, pieceType, color, moveType));
                    }
                }

                if (mode == GeneratorMode.CalculateAll || mode == GeneratorMode.CalculateAttackFields)
                {
                    attacks[(int)color, patternIndex] |= pieceLSB;
                }
            }
        }

        void CalculateMovesForLeftAttack(PieceType pieceType, Color color, GeneratorMode mode, ulong piecesToParse, ulong[] enPassant, OccupancyContainer occupancyContainer, LinkedList<Move> moves, ulong[,] attacks)
        {
            var validPieces = piecesToParse & ~BitConstants.AFile;
            var enemyColor = ColorOperations.Invert(color);

            var pattern = color == Color.White ? validPieces << 9 : validPieces >> 7;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var pieceLSB = color == Color.White ? patternLSB >> 9 : patternLSB << 7;
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
                var enPassantField = enPassant[(int)enemyColor] & patternLSB;

                if (mode == GeneratorMode.CalculateAll)
                {
                    if ((patternLSB & occupancyContainer.EnemyOccupancy) != 0 || enPassantField != 0)
                    {
                        var from = BitPositionConverter.ToPosition(pieceIndex);
                        var to = BitPositionConverter.ToPosition(patternIndex);
                        var moveType = enPassantField == 0 ? MoveType.Kill : MoveType.EnPassant;

                        moves.AddLast(new Move(from, to, pieceType, color, moveType));
                    }
                }

                if (mode == GeneratorMode.CalculateAll || mode == GeneratorMode.CalculateAttackFields)
                {
                    attacks[(int)color, patternIndex] |= pieceLSB;
                }
            }
        }
    }
}
