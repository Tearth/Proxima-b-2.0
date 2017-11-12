using Proxima.Core.Commons.Randoms;

namespace Proxima.Core.Boards.Hashing
{
    public class ZobristHash
    {
        Random64 _random64;

        public ZobristHash()
        {
            _random64 = new Random64();
        }

        public ulong Calculate()
        {
            var hash = 0ul;
            return hash;
        }
    }
}
