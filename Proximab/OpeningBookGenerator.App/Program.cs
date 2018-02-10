using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.MoveGenerators.Moves;

namespace OpeningBookGenerator.App
{
    /// <summary>
    /// Represents the entry point class with Main method.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">Program arguments.</param>
        public static void Main(string[] args)
        {
            var generator = new Generator();

            Console.WriteLine("Proxima b opening book generator");
            Console.WriteLine();

            var openingBook = generator.GetOpeningBook("Books/input.book");
            SaveOpeningBook("Books/output.book", openingBook);
        }
        /// <summary>
        /// Saves parsed openings to the specified file.
        /// </summary>
        /// <param name="outputFile">The filename with parsed openings.</param>
        /// <param name="openingBook">The list of openings.</param>
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
