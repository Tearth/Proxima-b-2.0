using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Boards.Friendly
{
    public class FriendlyAttack
    {
        public Color Color { get; private set; }
        public Position From { get; private set; }
        public Position To { get; private set; }

        public FriendlyAttack()
        {

        }

        public FriendlyAttack(Color color, Position from, Position to)
        {
            Color = color;
            From = from;
            To = to;
        }
    }
}
