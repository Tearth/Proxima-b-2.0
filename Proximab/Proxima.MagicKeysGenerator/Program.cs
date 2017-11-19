using System;
using System.IO;

namespace Proxima.MagicKeysGenerator
{
    internal class Program
    {
        static readonly string RookKeysFileName = "Keys/RookKeys.txt";
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
