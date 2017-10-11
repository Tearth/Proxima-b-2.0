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
                var patternPosition = BitPositionConverter.ToPosition(patternLSB);

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
                validPieces = (~occupancyContainer.Occupancy >> 8) & validPieces;
                pattern = validPieces << 16;
            }
            else
            {
                validPieces = piecesToParse & BitConstants.GRank;
                validPieces = (~occupancyContainer.Occupancy << 8) & validPieces;
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
    }
}
