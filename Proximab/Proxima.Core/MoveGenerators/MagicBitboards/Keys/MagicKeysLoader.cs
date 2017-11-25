using System.IO;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Keys
{
    public class MagicKeysLoader
    {
        private const string RookKeysFileName = "Keys/RookKeys.txt";
        private const string BishopKeysFileName = "Keys/BishopKeys.txt";

        public ulong[] LoadRookKeys()
        {
            return LoadKeysFromFile(RookKeysFileName);
        }

        public ulong[] LoadBishopKeys()
        {
            return LoadKeysFromFile(BishopKeysFileName);
        }

        private ulong[] LoadKeysFromFile(string fileName)
        {
            var keys = new ulong[64];

            using (var reader = new StreamReader(fileName))
            {
                for (int i = 0; i < 64; i++)
                {
                    var line = reader.ReadLine();
                    keys[i] = ulong.Parse(line);
                }
            }

            return keys;
        }
    }
}
