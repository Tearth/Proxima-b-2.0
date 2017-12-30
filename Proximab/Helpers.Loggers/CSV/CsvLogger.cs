using System.Collections.Generic;
using System.IO;
using System.Text;
using Proxima.Core.AI;
using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;

namespace Helpers.Loggers.CSV
{
    /// <summary>
    /// Represents a set of methods to writing in csv file.
    /// </summary>
    public class CsvLogger : LogBase
    {
        private const char Delimeter = ';';
        private const string FileExtension = ".csv";
        private const string AITimeFormat = "0.000";

        private List<string> _header;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvLogger"/> class.
        /// </summary>
        /// <param name="directory">The directory where all logs will be stored.</param>
        public CsvLogger(string directory) : base(directory)
        {
            _header = new List<string>
            {
                "Time",
                "Best move",
                "Total nodes",
                "End nodes",
                "α-β cutoffs",
                "TT hits",
                "Branching factor",
                "Nodes per second",
                "Time per node",
                "Depth",
                "Score",
                "Preferred time",
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
            using (var csvLogger = OpenOrCreateFile(FileExtension))
            {
                if (csvLogger.BaseStream.Length == 0)
                {
                    WriteValues(csvLogger, _header);
                }

                var values = new List<string>
                {
                    GetCurrentTime(),
                    aiResult.PVNodes.ToString(),
                    aiResult.Stats.TotalNodes.ToString(),
                    aiResult.Stats.EndNodes.ToString(),
                    aiResult.Stats.AlphaBetaCutoffs.ToString(),
                    aiResult.Stats.TranspositionTableHits.ToString(),
                    aiResult.Stats.BranchingFactor.ToString(),
                    aiResult.NodesPerSecond.ToString(),
                    aiResult.TimePerNode.ToString(),
                    aiResult.Depth.ToString(),
                    aiResult.Score.ToString(),
                    aiResult.PreferredTime.ToString(AITimeFormat),
                    aiResult.Time.ToString(AITimeFormat),
                    bitboard.Occupancy[0].ToString(),
                    bitboard.Occupancy[1].ToString(),
                    bitboard.GamePhase.ToString()
                };

                WriteValues(csvLogger, values);
            }
        }

        /// <summary>
        /// Writes game result as new line to the csv file.
        /// </summary>
        /// <param name="gameResult">The game result.</param>
        /// <param name="engineColor">The engine color.</param>
        public void WriteLine(GameResult gameResult, Color? engineColor)
        {
            using (var csvLogger = OpenOrCreateFile(FileExtension))
            {
                switch (gameResult)
                {
                    case GameResult.Draw:
                    {
                        csvLogger.WriteLine("DRAW");
                        break;
                    }
                    case GameResult.Aborted:
                    {
                        csvLogger.WriteLine("ABORTED");
                        break;
                    }
                    case GameResult.WhiteWon when engineColor == Color.White:
                    case GameResult.BlackWon when engineColor == Color.Black:
                    {
                        csvLogger.WriteLine("ENGINE_WON");
                        break;
                    }
                    default:
                    {
                        csvLogger.WriteLine("ENGINE_LOST");
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Writes values to the specified csv file (separated by <see cref="Delimeter"/>.
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
