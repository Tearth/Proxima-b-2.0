using System.Collections.Generic;
using System.IO;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.OpeningBook
{
    public class OpeningBookContainer
    {
        public static List<List<OpeningBookMove>> Openings { get; set; }

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
