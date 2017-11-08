using System;
using System.IO;

namespace MagicKeysGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting magic keys generator...");
            Console.WriteLine();

            var generator = new Generator();
            var keys = generator.GetKeys();

            SaveKeysToFile(keys);

            Console.WriteLine();
            Console.WriteLine("Keys saved to keys.txt");
            Console.WriteLine("Operation completed!");
            Console.ReadLine();
        }

        static void SaveKeysToFile(ulong[] keys)
        {
            using (var writer = new StreamWriter("keys.txt"))
            {
                foreach(var key in keys)
                {
                    writer.WriteLine(key);
                }
            }
        }
    }
}
