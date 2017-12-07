using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxima.Core.Boards.Friendly
{
    /// <summary>
    /// Represents a list of attacks in the user-friendly way.
    /// </summary>
    public class FriendlyAttacksList : List<FriendlyAttack>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FriendlyAttacksList"/> class.
        /// </summary>
        public FriendlyAttacksList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendlyAttacksList"/> class.
        /// </summary>
        /// <param name="attacks">The array of attack bitboards (where index means field index (source piece position),
        ///                       and set bits are the destination attack positions.</param>
        /// <param name="pieces">The list of pieces.</param>
        public FriendlyAttacksList(ulong[] attacks, FriendlyPiecesList pieces)
        {
            for (int i = 0; i < 64; i++)
            {
                var fieldAttackers = attacks[i];
                var targetPosition = BitPositionConverter.ToPosition(i);

                while (fieldAttackers != 0)
                {
                    var attackerLSB = BitOperations.GetLSB(fieldAttackers);
                    fieldAttackers = BitOperations.PopLSB(fieldAttackers);

                    var attackerIndex = BitOperations.GetBitIndex(attackerLSB);
                    var attackerPosition = BitPositionConverter.ToPosition(attackerIndex);
                    var attackerColor = pieces.First(p => p.Position == attackerPosition).Color;

                    Add(new FriendlyAttack(attackerColor, attackerPosition, targetPosition));
                }
            }
        }
    }
}
