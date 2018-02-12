using System;
using System.Collections.Generic;
using System.Linq;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.OpeningBook
{
    /// <summary>
    /// Represents a set of methods to manage opening book.
    /// </summary>
    public class OpeningBookProvider
    {
        private Random _random;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpeningBookProvider"/> class.
        /// </summary>
        public OpeningBookProvider()
        {
            _random = new Random();
        }

        /// <summary>
        /// Gets the opening move based on game moves history.
        /// </summary>
        /// <param name="history">The game moves history.</param>
        /// <returns>The next move from the opening book. If there is no any matches, then returns null (and AI should
        /// run the regular search).</returns>
        public OpeningBookMove GetMoveFromBook(List<Move> history)
        {
            var availableOpeningMoves = OpeningBookContainer.Openings.Where(p => p.Count > history.Count).ToList();
            for(var i=0; i<history.Count; i++)
            {
                var historyMove = history[i];

                availableOpeningMoves = availableOpeningMoves.Where(p =>
                        p[i].From == historyMove.From && p[i].To == historyMove.To)
                    .ToList();

                if (availableOpeningMoves.Count <= 0)
                {
                    return null;
                }
            }

            var openingMoveIndex = _random.Next(0, availableOpeningMoves.Count - 1);
            return availableOpeningMoves[openingMoveIndex][history.Count];
        }
    }
}
