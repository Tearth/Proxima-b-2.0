using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.AI.Patterns
{
    /// <summary>
    /// Represents a set of methods to detect patterns (hard-coded positions which can be difficult to evaluate by standard
    /// methods but easy to detect and prune).
    /// </summary>
    public class PatternsDetector
    {
        /// <summary>
        /// Checks if the specified position and move are classified as pattern.
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="move">The move.</param>
        /// <returns>True if pattern has detected (and move should be pruned), otherwise false.</returns>
        public bool IsPattern(Bitboard bitboard, Move move)
        {
            var isPattern = false;

            isPattern |= IsG4G5Sacrifice(bitboard, move);

            return isPattern;
        }

        /// <summary>
        /// Checks if there is a G4/G5 sacrifice pattern (enemy sacrifice light piece to open file near to the king and
        /// attack him with the Queen/Rook).
        /// </summary>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="move">The move.</param>
        /// <returns>True if patterns is detected and move should be pruned, otherwise false.</returns>
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
