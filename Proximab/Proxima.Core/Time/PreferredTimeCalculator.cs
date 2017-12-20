﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxima.Core.Time
{
    /// <summary>
    /// Represents a set of methods for calculate preferred time.
    /// </summary>
    public class PreferredTimeCalculator
    {
        private int _expectedMovesCount;
        private int _edge;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PreferredTimeCalculator"/> class.
        /// </summary>
        /// <param name="expectedMovesCount">The expected moves count (during game, number of moves can be greater
        /// than this parameter but preferred times will be not equal.</param>
        public PreferredTimeCalculator(int expectedMovesCount)
        {
            _expectedMovesCount = expectedMovesCount;
            _edge = (int)(_expectedMovesCount * 0.75f);
        }

        /// <summary>
        /// Calculates a preferred time.
        /// </summary>
        /// <param name="movesCount">The moves count of the current game.</param>
        /// <param name="remainingTime">The remaining time for the specified color.</param>
        /// <returns>The preferred time.</returns>
        public float Calculate(int movesCount, int remainingTime)
        {
            if (movesCount < _edge)
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
