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
            return (ulong)(position.X - 1) << ((position.Y - 1) * 8);
        }
    }
}
