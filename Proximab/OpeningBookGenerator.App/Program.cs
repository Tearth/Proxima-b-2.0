using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpeningBookGenerator.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new Generator();

            Console.WriteLine("Proxima b opening book generator");
            Console.WriteLine();

            var openingBook = generator.GetOpeningBook("Books/input.book");
        }
    }
}
