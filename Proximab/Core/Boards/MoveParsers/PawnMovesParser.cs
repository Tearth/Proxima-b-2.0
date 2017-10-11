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

        public List<Move> GetMoves(PieceType pieceType, Color color, ulong[,] pieces, OccupancyContainer occupancyContainer, ref ulong[,] attacks)
        {
            var moves = new List<Move>();

            var piecesToParse = pieces[(int)color, (int)pieceType];

            moves.AddRange(GetMovesForSinglePush(pieceType, color, piecesToParse, occupancyContainer));
            moves.AddRange(GetMovesForDoublePush(pieceType, color, piecesToParse, occupancyContainer));
            moves.AddRange(GetMovesForRightAttack(pieceType, color, piecesToParse, occupancyContainer, ref attacks));
            moves.AddRange(GetMovesForLeftAttack(pieceType, color, piecesToParse, occupancyContainer, ref attacks));

            return moves;
        }

        List<Move> GetMovesForSinglePush(PieceType pieceType, Color color, ulong piecesToParse, OccupancyContainer occupancyContainer)
        {
            var moves = new List<Move>();
            
            var pattern = color == Color.White ? piecesToParse << 8 : piecesToParse >> 8;

            pattern &= ~occupancyContainer.Occupancy;

            while(pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var pieceLSB = color == Color.White ? patternLSB >> 8 : patternLSB << 8;

                var from = BitPositionConverter.ToPosition(pieceLSB);
                var to = BitPositionConverter.ToPosition(patternLSB);
                var moveType = MoveType.Quiet;

                moves.Add(new Move(from, to, pieceType, color, moveType));
            }

            return moves;
        }

        List<Move> GetMovesForDoublePush(PieceType pieceType, Color color, ulong piecesToParse, OccupancyContainer occupancyContainer)
        {
            var moves = new List<Move>();
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

                moves.Add(new Move(from, to, pieceType, color, moveType));
            }

            return moves;
        }

        List<Move> GetMovesForRightAttack(PieceType pieceType, Color color, ulong piecesToParse, OccupancyContainer occupancyContainer, ref ulong[,] attacks)
        {
            var moves = new List<Move>();
            var validPieces = piecesToParse & ~BitConstants.HFile;

            var pattern = color == Color.White ? validPieces << 7 : validPieces >> 7;
            pattern &= ~occupancyContainer.FriendlyOccupancy;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var pieceLSB = color == Color.White ? patternLSB >> 7 : patternLSB << 7;

                var from = BitPositionConverter.ToPosition(pieceLSB);
                var to = BitPositionConverter.ToPosition(patternLSB);
                var moveType = MoveType.Kill;

                moves.Add(new Move(from, to, pieceType, color, moveType));
                attacks[(int)color, patternIndex] |= pieceLSB;
            }

            return moves;
        }

        List<Move> GetMovesForLeftAttack(PieceType pieceType, Color color, ulong piecesToParse, OccupancyContainer occupancyContainer, ref ulong[,] attacks)
        {
            var moves = new List<Move>();
            var validPieces = piecesToParse & ~BitConstants.AFile;

            var pattern = color == Color.White ? validPieces << 9 : validPieces >> 9;
            pattern &= ~occupancyContainer.FriendlyOccupancy;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                var pieceLSB = color == Color.White ? patternLSB >> 9 : patternLSB << 9;

                var from = BitPositionConverter.ToPosition(pieceLSB);
                var to = BitPositionConverter.ToPosition(patternLSB);
                var moveType = MoveType.Kill;

                moves.Add(new Move(from, to, pieceType, color, moveType));
                attacks[(int)color, patternIndex] |= pieceLSB;
            }

            return moves;
        }
    }
}
