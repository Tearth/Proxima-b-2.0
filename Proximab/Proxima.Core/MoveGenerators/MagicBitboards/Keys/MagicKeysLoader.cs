using System.IO;
using Proxima.Core.MoveGenerators.MagicBitboards.Keys.Exceptions;

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
                    keys[i] = LoadKey(reader);
                }
            }

            return keys;
        }

        /// <summary>
        /// Loads single magic key from a file.
        /// </summary>
        /// <param name="reader">The file reader.</param>
        /// <exception cref="InvalidMagicKeysFileException">Thrown when file is shorter than expected and cannot load next magic key.</exception>
        /// <returns>The next magic key from file.</returns>
        private ulong LoadKey(StreamReader reader)
        {
            var line = reader.ReadLine();
            if (line == null)
            {
                throw new InvalidMagicKeysFileException();
            }

            return ulong.Parse(line);
        }
    }
}
