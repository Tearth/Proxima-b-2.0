using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.MoveGenerators.Moves;
using Proxima.Core.MoveGenerators.PatternGenerators;

namespace OpeningBookGenerator.App
{
    public class Generator
    {
        private OpeningParser _openingParser;

        public Generator()
        {
            _openingParser = new OpeningParser();

            PatternsContainer.Init();
        }

        public List<List<Move>> GetOpeningBook(string inputFile)
        {
            var openingBook = new List<List<Move>>();

            using (var streamReader = new StreamReader(inputFile))
            {
                var line = streamReader.ReadLine();
                var opening = ParseOpening(line);

                openingBook.Add(opening);
            }

            return openingBook;
        }

        private List<Move> ParseOpening(string line)
        {
            var rawMoves = line.Split(' ').ToList();
            var trimMoves = TrimRawMoves(rawMoves);

            return _openingParser.ParseMoves(trimMoves);
        }

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

                trimMoves.Add(trimMove);
            }

            return trimMoves;
        }
    }
}
