using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.AI;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;

namespace Proxima.Core.Session
{
    public class GameSession
    {
        public int MovesCount { get; private set; }

        private AICore _aiCore;
        private Bitboard _bitboard;

        public GameSession()
        {
            MovesCount = 0;

            _aiCore = new AICore();
            _bitboard = new Bitboard(new DefaultFriendlyBoard());
        }
    }
}
