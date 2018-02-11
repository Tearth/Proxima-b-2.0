using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.Commons.Colors;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.AI.HistoryHeuristic
{
    public class HistoryTable
    {
        private int[][][] _table;

        public HistoryTable()
        {
            _table = new int[2][][];

            for (var depth = 0; depth < 2; depth++)
            {
                _table[depth] = new int[64][];

                for (var fieldFrom = 0; fieldFrom < 64; fieldFrom++)
                {
                    _table[depth][fieldFrom] = new int[64];
                }
            }
        }

        public void Clear()
        {
            for (var depth = 0; depth < 2; depth++)
            {
                for (var fieldFrom = 0; fieldFrom < 64; fieldFrom++)
                {
                    for (var fieldTo = 0; fieldTo < 64; fieldTo++)
                    {
                        _table[depth][fieldFrom][fieldTo] = 0;
                    }
                }
            }
        }

        public void AddKiller(Color color, int depth, Move move)
        {
            var fromIndex = BitPositionConverter.ToBitIndex(move.From);
            var toIndex = BitPositionConverter.ToBitIndex(move.To);

            _table[(int)color][fromIndex][toIndex] += depth * depth;
        }

        public int GetKillersCount(Color color, Move move)
        {
            var fromIndex = BitPositionConverter.ToBitIndex(move.From);
            var toIndex = BitPositionConverter.ToBitIndex(move.To);

            return _table[(int)color][fromIndex][toIndex];
        }
    }
}
