using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxima.Core.Boards.Friendly
{
    public class FriendlyAttack
    {
        public Color Color { get; set; }
        public Position To { get; set; }

        public FriendlyAttack()
        {

        }

        public FriendlyAttack(Color color, Position to)
        {
            Color = Color;
            To = to;
        }
    }
}
