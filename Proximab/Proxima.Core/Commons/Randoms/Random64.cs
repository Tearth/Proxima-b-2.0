using System;

namespace Proxima.Core.Commons.Randoms
{
    /// <summary>
    /// Represents a set of methods to generate 64-bit integers.
    /// </summary>
    public class Random64
    {
        private Random _random;

        /// <summary>
        /// Initializes a new instance of the <see cref="Random64"/> class.
        /// </summary>
        public Random64()
        {
            _random = new Random();
        }

        /// <summary>
        /// Calculates a new random 64-bit integer.
        /// </summary>
        /// <returns>The random 64-bit integer.</returns>
        public ulong Next()
        {
            var leftPart = (ulong)_random.Next() << 32;
            var rightPart = (ulong)_random.Next();

            return leftPart | rightPart;
        }
    }
}
