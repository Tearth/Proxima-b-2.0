using Proxima.Core.Boards;
using Proxima.Core.Commons.Randoms;
using Proxima.Core.MoveGenerators.MagicBitboards.Attacks;
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
            var attacks = new ulong[4096];

            var key = 0ul;
            var fail = true;
            
            while(fail)
            {
                Array.Clear(attacks, 0, 4096);

                key = _random64.Next() & _random64.Next() & _random64.Next();
                fail = false;
                
                var shift = 64 - BitOperations.Count(mask);
                foreach (var pattern in patterns)
                {
                    var hash = (pattern.Occupancy * key) >> shift;

                    if(attacks[hash] != 0 && attacks[hash] != pattern.Attacks)
                    {
                        fail = true;
                        break;
                    }

                    attacks[hash] = pattern.Attacks;
                }
            }

            return key;
        }
    }
}
