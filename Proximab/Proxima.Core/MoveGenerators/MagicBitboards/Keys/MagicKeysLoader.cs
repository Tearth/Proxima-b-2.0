using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Keys
{
    public class MagicKeysLoader
    {
        const string RookKeysFileName = "Keys/RookKeys.txt";
        const string BishopKeysFileName = "Keys/BishopKeys.txt";

        public ulong[] LoadRookKeys()
        {
            return LoadKeysFromFile(RookKeysFileName);
        }

        public ulong[] LoadBishopKeys()
        {
            return LoadKeysFromFile(BishopKeysFileName);
        }

        ulong[] LoadKeysFromFile(string fileName)
        {
            var keys = new ulong[64];

            using (var reader = new StreamReader(fileName))
            {
                for(int i=0; i<64; i++)
                {
                    var line = reader.ReadLine();
                    keys[i] = UInt64.Parse(line);
                }
            }

            return keys;
        }
    }
}
