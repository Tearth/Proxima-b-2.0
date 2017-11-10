using System;
using System.IO;

namespace Proxima.MagicKeysGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting magic keys generator...");
            Console.WriteLine();

            var generator = new Generator();

            Console.WriteLine("Rook magic keys...");
            SaveKeysToFile(generator.GetRookKeys(), "Keys/RookKeys.txt");

            Console.WriteLine();
            Console.WriteLine("Bishop magic keys...");
            SaveKeysToFile(generator.GetBishopKeys(), "Keys/BishopKeys.txt");

            Console.WriteLine();
            Console.WriteLine("Operation completed!");
            Console.ReadLine();
        }

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
