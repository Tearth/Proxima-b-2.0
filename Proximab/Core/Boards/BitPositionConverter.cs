using Core.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Boards
{
    internal class BitPositionConverter
    {
        public BitPositionConverter()
        {

        }

        public ulong Convert(Position position)
        {
            return 1ul << ((position.X - 1) + ((position.Y - 1) * 8));
        }

        public Position Convert(ulong bitPosition)
        {
            var bitIndex = (int)Math.Log(bitPosition, 2);

            return new Position((bitIndex % 8) + 1, (bitIndex / 8) + 1);
        }
    }
}
