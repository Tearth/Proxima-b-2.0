using System;
using System.IO;

namespace Proxima.MagicKeysGenerator
{
    internal class Program
    {
        /// <summary>
        /// Name of the file where all magic keys for rook will be stored.
        /// </summary>
        static readonly string RookKeysFileName = "Keys/RookKeys.txt";

        /// <summary>
        /// Name of the file where all magic keys for bishop will be stored.
        /// </summary>
        static readonly string BishopKeysFileName = "Keys/BishopKeys.txt";

        static void Main(string[] args)
        {
            var generator = new Generator();

            Console.WriteLine("Proxima b magic keys generator");
            Console.WriteLine();
            Console.WriteLine("Rook magic keys...");

            SaveKeysToFile(generator.GetRookKeys(), RookKeysFileName);

            Console.WriteLine();
            Console.WriteLine("Bishop magic keys...");

            SaveKeysToFile(generator.GetBishopKeys(), BishopKeysFileName);

            Console.WriteLine();
            Console.WriteLine("Operation completed!");
            Console.ReadLine();
        }

        /// <summary>
        /// Saves magic keys to the specified file. Each key is on a separate line.
        /// </summary>
        /// <param name="keys">Array of magic keys to save.</param>
        /// <param name="fileName">Path to file where all magic keys will be stored.</param>
        static void SaveKeysToFile(ulong[] keys, string fileName)
        {
            using (var writer = new StreamWriter(fileName))
            {
                foreach(var key in keys)
                {
                    writer.WriteLine(key);
                }
            }
        }
    }
}
