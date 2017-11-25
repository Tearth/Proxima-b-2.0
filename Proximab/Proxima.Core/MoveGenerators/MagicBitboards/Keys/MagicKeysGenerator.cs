using System;
using System.Collections.Generic;
using Proxima.Core.Commons.Randoms;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Keys
{
    public class MagicKeyGenerator
    {
        private Random64 _random64;

        public MagicKeyGenerator()
        {
            _random64 = new Random64();
        }

        public ulong GenerateKey(List<FieldPattern> patterns, int maskLength)
        {
            var attacks = new ulong[MagicConstants.RookMovesPerField];

            var key = 0ul;
            var fail = true;

            while (fail)
            {
                key = GetRandomKey();
                fail = false;

                Array.Clear(attacks, 0, MagicConstants.RookMovesPerField);

                foreach (var pattern in patterns)
                {
                    var hash = (pattern.Occupancy * key) >> (64 - maskLength);

                    if (attacks[hash] != 0 && attacks[hash] != pattern.Attacks)
                    {
                        fail = true;
                        break;
                    }

                    attacks[hash] = pattern.Attacks;
                }
            }

            return key;
        }

        private ulong GetRandomKey()
        {
            return _random64.Next() & _random64.Next() & _random64.Next();
        }
    }
}
