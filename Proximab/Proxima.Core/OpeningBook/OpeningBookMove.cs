using Proxima.Core.Commons.Positions;

namespace Proxima.Core.OpeningBook
{
    public class OpeningBookMove
    {
        public Position From { get; set; }
        public Position To { get; set; }

        public OpeningBookMove(Position from, Position to)
        {
            From = from;
            To = to;
        }
    }
}
