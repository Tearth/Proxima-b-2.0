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

        public List<Move> GetMoves(PieceType pieceType, Color color, ulong[,] pieces, ulong[] occupancy, ref ulong[,] attacks)
        {
            var moves = new List<Move>();

            var friendlyOccupancy = occupancy[(int)color];
            var enemyOccupancy = occupancy[(int)ColorOperations.Invert(color)];

            var allPiecesOccupancy = friendlyOccupancy | enemyOccupancy;

            var piecesToParse = pieces[(int)color, (int)pieceType];

            moves.AddRange(GetMovesForSinglePush(pieceType, color, piecesToParse, allPiecesOccupancy));

            return moves;
        }

        List<Move> GetMovesForSinglePush(PieceType pieceType, Color color, ulong piecesToParse, ulong occupancy)
        {
            var moves = new List<Move>();

            var pattern = piecesToParse << 8;

            while(pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(ref pattern);
                var patternPosition = BitPositionConverter.ToPosition(patternLSB);

                var pieceLSB = patternLSB >> 8;

                var from = BitPositionConverter.ToPosition(pieceLSB);
                var to = BitPositionConverter.ToPosition(patternLSB);
                var moveType = MoveType.Quiet;

                moves.Add(new Move(from, to, pieceType, color, moveType));
            }

            return moves;
        }
    }
}
