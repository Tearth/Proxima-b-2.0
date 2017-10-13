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

        public void GetMoves(PieceType pieceType, Color color, ulong[,] pieces, OccupancyContainer occupancyContainer, LinkedList<Move> moves, ref ulong[,] attacks)
        {
            var piecesToParse = pieces[(int)color, (int)pieceType];

            CalculateMovesForSinglePush(pieceType, color, piecesToParse, occupancyContainer, moves);
            CalculateMovesForDoublePush(pieceType, color, piecesToParse, occupancyContainer, moves);
            CalculateMovesForRightAttack(pieceType, color, piecesToParse, occupancyContainer, moves, ref attacks);
            CalculateMovesForLeftAttack(pieceType, color, piecesToParse, occupancyContainer, moves, ref attacks);
        }

        void CalculateMovesForSinglePush(PieceType pieceType, Color color, ulong piecesToParse, OccupancyContainer occupancyContainer, LinkedList<Move> moves)
        {
            var pattern = color == Color.White ? piecesToParse << 8 : piecesToParse >> 8;
            pattern &= ~occupancyContainer.Occupancy;

            while(pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var pieceLSB = color == Color.White ? patternLSB >> 8 : patternLSB << 8;

                var from = BitPositionConverter.ToPosition(pieceLSB);
                var to = BitPositionConverter.ToPosition(patternLSB);
                var moveType = MoveType.Quiet;

                moves.AddLast(new Move(from, to, pieceType, color, moveType));
            }
        }

        void CalculateMovesForDoublePush(PieceType pieceType, Color color, ulong piecesToParse, OccupancyContainer occupancyContainer, LinkedList<Move> moves)
        {
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
                var patternPosition = BitPositionConverter.ToPosition(patternLSB);

                var pieceLSB = color == Color.White ? patternLSB >> 16 : patternLSB << 16;

                var from = BitPositionConverter.ToPosition(pieceLSB);
                var to = BitPositionConverter.ToPosition(patternLSB);
                var moveType = MoveType.DoublePush;

                moves.AddLast(new Move(from, to, pieceType, color, moveType));
            }
        }

        void CalculateMovesForRightAttack(PieceType pieceType, Color color, ulong piecesToParse, OccupancyContainer occupancyContainer, LinkedList<Move> moves, ref ulong[,] attacks)
        {
            var validPieces = piecesToParse & ~BitConstants.HFile;

            var pattern = color == Color.White ? validPieces << 7 : validPieces >> 9;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var pieceLSB = color == Color.White ? patternLSB >> 7 : patternLSB << 9;

                if ((patternLSB & occupancyContainer.EnemyOccupancy) != 0)
                {
                    var from = BitPositionConverter.ToPosition(pieceLSB);
                    var to = BitPositionConverter.ToPosition(patternLSB);
                    var moveType = MoveType.Kill;

                    moves.AddLast(new Move(from, to, pieceType, color, moveType));
                }

                attacks[(int)color, patternIndex] |= pieceLSB;
            }
        }

        void CalculateMovesForLeftAttack(PieceType pieceType, Color color, ulong piecesToParse, OccupancyContainer occupancyContainer, LinkedList<Move> moves, ref ulong[,] attacks)
        {
            var validPieces = piecesToParse & ~BitConstants.AFile;

            var pattern = color == Color.White ? validPieces << 9 : validPieces >> 7;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var pieceLSB = color == Color.White ? patternLSB >> 9 : patternLSB << 7;

                if ((patternLSB & occupancyContainer.EnemyOccupancy) != 0)
                {
                    var from = BitPositionConverter.ToPosition(pieceLSB);
                    var to = BitPositionConverter.ToPosition(patternLSB);
                    var moveType = MoveType.Kill;

                    moves.AddLast(new Move(from, to, pieceType, color, moveType));
                }

                attacks[(int)color, patternIndex] |= pieceLSB;
            }
        }
    }
}
