using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.AI.HistoryHeuristic
{
    /// <summary>
    /// Represents a set of methods to manage history table heuristic.
    /// </summary>
    public class HistoryTable
    {
        private int[][][] _table;

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryTable"/> class.
        /// </summary>
        public HistoryTable()
        {
            _table = new int[2][][];

            for (var color = 0; color < 2; color++)
            {
                _table[color] = new int[64][];

                for (var fieldFrom = 0; fieldFrom < 64; fieldFrom++)
                {
                    _table[color][fieldFrom] = new int[64];
                }
            }
        }

        /// <summary>
        /// Clears history table (every value is set to 0).
        /// </summary>
        public void Clear()
        {
            for (var color = 0; color < 2; color++)
            {
                for (var fieldFrom = 0; fieldFrom < 64; fieldFrom++)
                {
                    for (var fieldTo = 0; fieldTo < 64; fieldTo++)
                    {
                        _table[color][fieldFrom][fieldTo] = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Increments a killers count at the specified depth.
        /// </summary>
        /// <param name="color">The killer color.</param>
        /// <param name="depth">The killer depth.</param>
        /// <param name="move">The killer move.</param>
        public void AddKiller(Color color, int depth, Move move)
        {
            var fromIndex = BitPositionConverter.ToBitIndex(move.From);
            var toIndex = BitPositionConverter.ToBitIndex(move.To);

            _table[(int)color][fromIndex][toIndex] += depth * depth;
        }

        /// <summary>
        /// Gets the killers count for the specified move.
        /// </summary>
        /// <param name="move">The move</param>
        /// <returns>The killers count for the specified move.</returns>
        public int GetKillersCount(Move move)
        {
            var fromIndex = BitPositionConverter.ToBitIndex(move.From);
            var toIndex = BitPositionConverter.ToBitIndex(move.To);

            return _table[(int)move.Color][fromIndex][toIndex];
        }
    }
}
