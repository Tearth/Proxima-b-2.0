using System.IO;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Keys
{
    /// <summary>
    /// Represents a set of methods to loading magic keys from file.
    /// </summary>
    public class MagicKeysLoader
    {
        private const string RookKeysFileName = "Keys/RookKeys.txt";
        private const string BishopKeysFileName = "Keys/BishopKeys.txt";

        /// <summary>
        /// Loads rook magic keys from file specified in the <see cref="RookKeysFileName"/> variable.
        /// </summary>
        /// <returns>The array of magic keys for all fields.</returns>
        public ulong[] LoadRookKeys()
        {
            return LoadKeysFromFile(RookKeysFileName);
        }

        /// <summary>
        /// Loads bishop magic keys from file specified in the <see cref="BishopKeysFileName"/> variable.
        /// </summary>
        /// <returns>The array of magic keys for all fields.</returns>
        public ulong[] LoadBishopKeys()
        {
            return LoadKeysFromFile(BishopKeysFileName);
        }

        /// <summary>
        /// Loads magic keys from the specified file.
        /// </summary>
        /// <param name="fileName">The file with magic keys.</param>
        /// <returns>The array of magic keys for all fields.</returns>
        private ulong[] LoadKeysFromFile(string fileName)
        {
            var keys = new ulong[64];

            using (var reader = new StreamReader(fileName))
            {
                for (var i = 0; i < 64; i++)
                {
                    var line = reader.ReadLine();
                    keys[i] = ulong.Parse(line);
                }
            }

            return keys;
        }
    }
}
