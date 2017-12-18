using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.AI;
using Proxima.Core.Boards;
using Proxima.Core.Commons.Colors;
using Proxima.FICS.Source.GameSubsystem.Modes.Game;

namespace Proxima.FICS.Source.LogSubsystem
{
    /// <summary>
    /// Represents a set of methods to writing in csv file.
    /// </summary>
    public class CsvWriter : LogBase
    {
        private const char Delimeter = ';';
        private const string FileExtension = ".csv";
        private const string AITimeFormat = "0.000";

        private List<string> _header;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvWriter"/> class.
        /// </summary>
        /// <param name="directory">The directory where all logs will be stored.</param>
        public CsvWriter(string directory) : base(directory)
        {
            _header = new List<string>()
            {
                "Time",
                "Best move",
                "Total nodes",
                "End nodes",
                "Nodes per second",
                "Time per node",
                "Score",
                "Time",
                "White occ",
                "Black occ",
                "Game phase"
            };
        }

        /// <summary>
        /// Writes <see cref="AIResult"/> and <see cref="Bitboard"/> objects as new line to the csv file.
        /// </summary>
        /// <param name="aiResult">The AI result.</param>
        /// <param name="bitboard">The bitboard.</param>
        public void WriteLine(AIResult aiResult, Bitboard bitboard)
        {
            using (var csvWriter = OpenOrCreateFile(FileExtension))
            {
                if (csvWriter.BaseStream.Length == 0)
                {
                    WriteValues(csvWriter, _header);
                }

                var values = new List<string>()
                {
                    GetCurrentTime(),
                    aiResult.BestMove.ToString(),
                    aiResult.Stats.TotalNodes.ToString(),
                    aiResult.Stats.EndNodes.ToString(),
                    aiResult.NodesPerSecond.ToString(),
                    aiResult.TimePerNode.ToString(),
                    aiResult.Score.ToString(),
                    aiResult.Time.ToString(AITimeFormat),
                    bitboard.Occupancy[0].ToString(),
                    bitboard.Occupancy[1].ToString(),
                    bitboard.GamePhase.ToString()
                };

                WriteValues(csvWriter, values);
            }
        }

        /// <summary>
        /// Writes game result as new line to the csv file.
        /// </summary>
        /// <param name="gameResult">The game result.</param>
        /// <param name="engineColor">The engine color.</param>
        public void WriteLine(GameResult gameResult, Color? engineColor)
        {
            using (var csvWriter = OpenOrCreateFile(FileExtension))
            {
                if (gameResult == GameResult.Draw)
                {
                    csvWriter.WriteLine("DRAW");
                }
                else if (gameResult == GameResult.Aborted)
                {
                    csvWriter.WriteLine("ABORTED");
                }
                else if ((gameResult == GameResult.WhiteWon && engineColor == Color.White) ||
                         (gameResult == GameResult.BlackWon && engineColor == Color.Black))
                {
                    csvWriter.WriteLine("ENGINE_WON");
                }
                else
                {
                    csvWriter.WriteLine("ENGINE_LOST");
                }
            }
        }

        /// <summary>
        /// Writes values to the specified csv file (deparated by <see cref="Delimeter"/>.
        /// </summary>
        /// <param name="writer">The csv stream writer</param>
        /// <param name="values">The list of values to write.</param>
        private void WriteValues(StreamWriter writer, List<string> values)
        {
            var headerStringBuilder = new StringBuilder();

            foreach (var value in values)
            {
                headerStringBuilder.Append(value);
                headerStringBuilder.Append(Delimeter);
            }

            writer.WriteLine(headerStringBuilder.ToString());
        }
    }
}
