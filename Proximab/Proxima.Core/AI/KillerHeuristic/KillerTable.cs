using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.Commons.BitHelpers;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.AI.KillerHeuristic
{
    public class KillerTable
    {
        private byte[][][] _table;

        public KillerTable()
        {
            _table = new byte[12][][];

            for (int depth = 0; depth < 2; depth++)
            {
                _table[depth] = new byte[64][];

                for (int fieldFrom = 0; fieldFrom < 64; fieldFrom++)
                {
                    _table[depth][fieldFrom] = new byte[64];
                }
            }
        }

        public void Clear()
        {
            for (int depth = 0; depth < 12; depth++)
            {
                for (int fieldFrom = 0; fieldFrom < 64; fieldFrom++)
                {
                    for (int fieldTo = 0; fieldTo < 64; fieldTo++)
                    {
                        _table[depth][fieldFrom][fieldTo] = 0;
                    }
                }
            }
        }

        public void AddKiller(int depth, Move move)
        {
            var fromIndex = BitPositionConverter.ToBitIndex(move.From);
            var toIndex = BitPositionConverter.ToBitIndex(move.To);

            _table[depth][fromIndex][toIndex]++;
        }

        public int GetKillersCount(int depth, Move move)
        {
            var fromIndex = BitPositionConverter.ToBitIndex(move.From);
            var toIndex = BitPositionConverter.ToBitIndex(move.To);

            return _table[depth][fromIndex][toIndex];
        }
    }
}
