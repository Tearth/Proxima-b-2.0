using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxima.Core.Time
{
    public class PreferredTimeCalculator
    {
        private int _edge;

        public PreferredTimeCalculator(int edge)
        {
            _edge = edge;
        }

        public float Calculate(int movesCount, int remainingTime)
        {
            if(movesCount < _edge)
            {
                return (float)remainingTime / movesCount;
            }
            else
            {
                return (float)remainingTime / _edge;
            }
        }
    }
}
