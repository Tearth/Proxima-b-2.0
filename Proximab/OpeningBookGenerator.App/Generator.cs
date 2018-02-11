using System.Collections.Generic;
using System.IO;
using System.Linq;
using Proxima.Core.MoveGenerators.Moves;
using Proxima.Core.MoveGenerators.PatternGenerators;

namespace OpeningBookGenerator.App
{
    /// <summary>
    /// Represents a set of methods for parsing text move notation to more readable (for engine) form.
    /// </summary>
    /// <remarks>
    /// Example: "1.f4 e5 2.fxe5 d6" is converted to "f2f4 e7e5 f4e5 d7d6".
    /// </remarks>
    public class Generator
    {
        private OpeningParser _openingParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="Generator"/> class.
        /// </summary>
        public Generator()
        {
            _openingParser = new OpeningParser();

            PatternsContainer.Init();
        }

        /// <summary>
        /// Converts openings from the specified input file to more readable (for engine) form.
        /// </summary>
        /// <param name="inputFile">The input file with openings to convert.</param>
        /// <returns>The list of converted openings.</returns>
        public List<List<Move>> GetOpeningBook(string inputFile)
        {
            var openingBook = new List<List<Move>>();

            using (var streamReader = new StreamReader(inputFile))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var opening = ParseOpening(line);

                    openingBook.Add(opening);
                }
            }

            return openingBook;
        }

        /// <summary>
        /// Parses single opening to list of converted moves.
        /// </summary>
        /// <param name="line">The opening to parse.</param>
        /// <returns>The list of converted moves</returns>
        private List<Move> ParseOpening(string line)
        {
            var rawMoves = line.Split(' ').ToList();
            var trimMoves = TrimRawMoves(rawMoves);

            return _openingParser.ParseMoves(trimMoves);
        }

        /// <summary>
        /// Trims text move notations by removing spaces and check/mate symbols.
        /// </summary>
        /// <param name="rawMoves">The list of text move notations.</param>
        /// <returns>The list of trimmed text move notations.</returns>
        private List<string> TrimRawMoves(List<string> rawMoves)
        {
            var trimMoves = new List<string>();

            foreach (var move in rawMoves)
            {
                var trimMove = move.Trim();
                if (char.IsDigit(move[0]))
                {
                    trimMove = move.Substring(2, move.Length - 2);
                }

                trimMove = trimMove.Replace("+", "");
                trimMoves.Add(trimMove);
            }

            return trimMoves;
        }
    }
}
