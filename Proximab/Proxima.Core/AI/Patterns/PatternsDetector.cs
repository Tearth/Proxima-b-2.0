using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.AI.Patterns
{
    public class PatternsDetector
    {
        public bool IsPattern(Bitboard bitboard, Move move)
        {
            var isPattern = false;

            isPattern |= IsG4G5Sacrifice(bitboard, move);

            return isPattern;
        }

        private bool IsG4G5Sacrifice(Bitboard bitboard, Move move)
        {
            const ulong whiteTrapField = 0x0000000200000000;
            const ulong blackTrapField = 0x0000000002000000;

            var castlingDone = bitboard.CastlingDone[(int)move.Color];
            var regularGamePhase = bitboard.GamePhase == GamePhase.Regular;
            var moveDetected = false;
            var lightPieceDetected = false;

            var whitePiecesTrap = bitboard.Pieces[FastArray.GetPieceIndex(Color.White, PieceType.Knight)] |
                                  bitboard.Pieces[FastArray.GetPieceIndex(Color.White, PieceType.Bishop)];

            var blackPiecesTrap = bitboard.Pieces[FastArray.GetPieceIndex(Color.Black, PieceType.Knight)] |
                                  bitboard.Pieces[FastArray.GetPieceIndex(Color.Black, PieceType.Bishop)];

            switch (move.Color)
            {
                case Color.White:
                {
                    moveDetected = move.From == new Position(8, 3) && move.To == new Position(7, 4);
                    lightPieceDetected = (blackPiecesTrap & blackTrapField) != 0;
                    break;
                }

                case Color.Black:
                {
                    moveDetected = move.From == new Position(8, 6) && move.To == new Position(7, 5);
                    lightPieceDetected = (whitePiecesTrap & whiteTrapField) != 0;
                    break;
                }
            }

            return castlingDone && regularGamePhase && moveDetected && lightPieceDetected;
        }
    }
}
