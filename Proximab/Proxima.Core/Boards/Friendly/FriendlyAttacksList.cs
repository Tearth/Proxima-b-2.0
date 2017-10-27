using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxima.Core.Boards.Friendly
{
    public class FriendlyAttacksList : List<FriendlyAttack>
    {
        public FriendlyAttacksList()
        {

        }

        public FriendlyAttacksList(ulong[] attacks, FriendlyPiecesList pieces)
        {
            for(int i=0; i<64; i++)
            {
                var fieldAttackers = attacks[i];
                var targetPosition = BitPositionConverter.ToPosition(i);

                while(fieldAttackers != 0)
                {
                    var attackerLSB = BitOperations.GetLSB(ref fieldAttackers);
                    var attackerIndex = BitOperations.GetBitIndex(attackerLSB);
                    var attackerPosition = BitPositionConverter.ToPosition(attackerIndex);
                    var attackerColor = pieces.First(p => p.Position == attackerPosition).Color;

                    Add(new FriendlyAttack(attackerColor, attackerPosition, targetPosition));
                }
            }
        }
    }
}
