using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.MoveGenerators.Moves;

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
            SaveOpeningBook("Books/output.book", openingBook);
        }

        private static void SaveOpeningBook(string outputFile, List<List<Move>> openingBook)
        {
            using (var streamWriter = new StreamWriter(outputFile))
            {
                foreach (var opening in openingBook)
                {
                    for(var i=0; i<opening.Count; i++)
                    {
                        if (i > 0)
                        {
                            streamWriter.Write(" ");
                        }

                        streamWriter.Write(opening[i].ToString());
                    }

                    streamWriter.WriteLine();
                }
            }
        }
    }
}
