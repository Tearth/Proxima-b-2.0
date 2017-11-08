using System;

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

            Console.WriteLine();
            Console.WriteLine("Operation completed!");
            Console.ReadLine();
        }
    }
}
