using Proxima.Core.Boards;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Randoms;
using System;
using System.Collections.Generic;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Keys
{
    public class MagicKeyGenerator
    {
        Random64 _random64;

        public MagicKeyGenerator()
        {
            _random64 = new Random64();
        }

        public ulong GenerateKey(List<FieldPattern> patterns, ulong mask)
        {
            var attacks = new ulong[MagicConstants.RookMovesPerField];

            var key = 0ul;
            var fail = true;
            
            while(fail)
            {
                key = GetRandomKey();
                fail = false;

                Array.Clear(attacks, 0, MagicConstants.RookMovesPerField);
                var bitsCount = BitOperations.Count(mask);

                foreach (var pattern in patterns)
                {
                    var hash = (pattern.Occupancy * key) >> (64 - bitsCount);

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

        ulong GetRandomKey()
        {
            return _random64.Next() & _random64.Next() & _random64.Next();
        }
    }
}
