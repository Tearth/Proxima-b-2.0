using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxima.Core.Evaluation.Position.Values
{
    public static class RookValues
    {
        public static readonly int[] Values = new int[2 * 64]
        {
            //Regular
            0,   0,   0,   0,   0,   0,   0,   0,
            5,   10,  10,  10,  10,  10,  10,  5,
           -5,   0,   0,   0,   0,   0,   0,  -5,
           -5,   0,   0,   0,   0,   0,   0,  -5,
           -5,   0,   0,   0,   0,   0,   0,  -5,
           -5,   0,   0,   0,   0,   0,   0,  -5,
           -5,   0,   0,   0,   0,   0,   0,  -5,
            0,   0,   0,   5,   5,   0,   0,   0,

            //End
            0,   0,   0,   0,   0,   0,   0,   0,
            5,   10,  10,  10,  10,  10,  10,  5,
           -5,   0,   0,   0,   0,   0,   0,  -5,
           -5,   0,   0,   0,   0,   0,   0,  -5,
           -5,   0,   0,   0,   0,   0,   0,  -5,
           -5,   0,   0,   0,   0,   0,   0,  -5,
           -5,   0,   0,   0,   0,   0,   0,  -5,
            0,   0,   0,   5,   5,   0,   0,   0
        };
    }
}
