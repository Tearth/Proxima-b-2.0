using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.AI.Search
{
    public class RegularSortedMove
    {
        public Move Move { get; set; }
        public int Score { get; set; }
    }
}
