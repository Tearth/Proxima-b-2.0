using System;

namespace Proxima.Core.Commons.Randoms
{
    public class Random64
    {
        Random _random;

        public Random64()
        {
            _random = new Random();
        }

        public ulong Next()
        {
            var leftPart = (ulong)_random.Next() << 32;
            var rightPart = (ulong)_random.Next();

            return leftPart | rightPart;
        }
    }
}
