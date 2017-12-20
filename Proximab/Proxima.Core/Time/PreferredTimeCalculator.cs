using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxima.Core.Time
{
    public class PreferredTimeCalculator
    {
        private int _expectedMovesCount;
        private int _edge;

        public PreferredTimeCalculator(int expectedMovesCount)
        {
            _expectedMovesCount = expectedMovesCount;
            _edge = (int)(_expectedMovesCount * 0.75f);
        }

        public float Calculate(int movesCount, int remainingTime)
        {
            if(movesCount < _edge)
            {
                return (float)remainingTime / (_expectedMovesCount - movesCount);
            }
            else
            {
                return (float)remainingTime / (_expectedMovesCount - _edge);
            }
        }
    }
}
