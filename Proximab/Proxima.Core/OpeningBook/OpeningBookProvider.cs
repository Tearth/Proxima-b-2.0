using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.OpeningBook
{
    public class OpeningBookProvider
    {
        private Random _random;

        public OpeningBookProvider()
        {
            _random = new Random();
        }

        public OpeningBookMove GetMoveFromBook(List<Move> history)
        {
            var availableOpeningMoves = OpeningBookContainer.Openings;
            for(var i=0; i<history.Count; i++)
            {
                var historyMove = history[i];

                availableOpeningMoves = availableOpeningMoves.Where(p =>
                    p.Count > i && p[i].From == historyMove.From && p[i].To == historyMove.To)
                    .ToList();

                if (availableOpeningMoves.Count <= 0)
                {
                    return null;
                }
            }

            if (availableOpeningMoves.Count > 0)
            {
                var openingMoveIndex = _random.Next(0, availableOpeningMoves.Count - 1);
                return availableOpeningMoves[openingMoveIndex][history.Count];
            }

            return null;
        }
    }
}
