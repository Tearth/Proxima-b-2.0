using System;
using System.IO;

namespace MagicKeysGenerator.App
{
    /// <summary>
    /// Represents the entry point class with Main method.
    /// </summary>
    public class Program
    {
        private static readonly string KeysDirectory = "Keys";
        private static readonly string RookKeysFileName = "RookKeys.txt";
        private static readonly string BishopKeysFileName = "BishopKeys.txt";

        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">Program arguments.</param>
        public static void Main(string[] args)
        {
            var generator = new Generator();

            if (!Directory.Exists(KeysDirectory))
            {
                Directory.CreateDirectory(KeysDirectory);
            }

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
        private static void SaveKeysToFile(ulong[] keys, string fileName)
        {
            using (var writer = new StreamWriter($"{KeysDirectory}/{fileName}"))
            {
                foreach (var key in keys)
                {
                    writer.WriteLine(key);
                }
            }
        }
    }
}
