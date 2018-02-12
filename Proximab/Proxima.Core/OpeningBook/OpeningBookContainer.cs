using System.Collections.Generic;
using System.IO;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.OpeningBook
{
    /// <summary>
    /// Represents a set of methods to load openings from the specified file.
    /// </summary>
    public class OpeningBookContainer
    {
        /// <summary>
        /// Gets the list of openings.
        /// </summary>
        public static List<List<OpeningBookMove>> Openings { get; private set; }

        /// <summary>
        /// Loads openings from the specified file.
        /// </summary>
        public static void Init()
        {
            Openings = new List<List<OpeningBookMove>>();

            using (var streamReader = new StreamReader("Book/openings.book"))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var opening = ReadOpening(line);

                    Openings.Add(opening);
                }
            }
        }

        /// <summary>
        /// Reads openings from the specified line.
        /// </summary>
        /// <param name="line">The line to parse.</param>
        /// <returns>The list of parsed opening moves.</returns>
        private static List<OpeningBookMove> ReadOpening(string line)
        {
            var opening = new List<OpeningBookMove>();
            var textMoves = line.Split(' ');

            foreach (var textMove in textMoves)
            {
                var fromText = textMove.Substring(0, 2);
                var toText = textMove.Substring(2, 2);

                var from = PositionConverter.ToPosition(fromText);
                var to = PositionConverter.ToPosition(toText);

                var openingMove = new OpeningBookMove(from, to);
                opening.Add(openingMove);
            }

            return opening;
        }
    }
}
