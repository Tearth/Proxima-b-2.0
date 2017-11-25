using Proxima.Core.Boards;

namespace Proxima.Core.MoveGenerators.MagicBitboards
{
    public class MaskBitsCountCalculator
    {
        public int[] Calculate(ulong[] patterns)
        {
            var bitsCountArray = new int[64];

            for (int i = 0; i < 64; i++)
            {
                bitsCountArray[i] = BitOperations.Count(patterns[i]);
            }

            return bitsCountArray;
        }
    }
}
