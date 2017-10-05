using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Boards
{
    public static class BitOperations
    {
        public static ulong GetLSB(ref ulong value)
        {
            var lsb = (ulong)((long)value & -((long)value));
            value &= value - 1;

            return lsb;
        }
    }
}
