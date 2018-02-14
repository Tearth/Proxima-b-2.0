using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.Boards;
using Proxima.Core.Commons.Colors;

namespace Proxima.Core.AI.LazySMP
{
    public struct HelperTaskParameters
    {
        public Color Color;
        public Bitboard Bitboard;
        public int InitialDepth;
        public long Deadline;
    }
}
