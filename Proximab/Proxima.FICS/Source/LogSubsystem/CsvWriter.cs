using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.AI;
using Proxima.Core.Boards;

namespace Proxima.FICS.Source.LogSubsystem
{
    /// <summary>
    /// Represents a set of methods to writing in csv file.
    /// </summary>
    public class CsvWriter : LogBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvWriter"/> class.
        /// </summary>
        /// <param name="directory">The directory where all logs will be stored.</param>
        public CsvWriter(string directory) : base(directory)
        {
        }

        /// <summary>
        /// Writes <see cref="AIResult"/> and <see cref="Bitboard"/> objects as new line to the csv file.
        /// </summary>
        /// <param name="aiResult">The AI result.</param>
        /// <param name="bitboard">The bitboard.</param>
        public void WriteLine(AIResult aiResult, Bitboard bitboard)
        {
            using (var csvWriter = OpenOrCreateFile(".csv"))
            {
                if (csvWriter.BaseStream.Length == 0)
                {
                    WriteHeader(csvWriter);
                }

                var output = $"{GetCurrentTime()};{aiResult.BestMove};{aiResult.Stats.TotalNodes};" +
                             $"{aiResult.Stats.EndNodes};{aiResult.NodesPerSecond};{aiResult.TimePerNode};{aiResult.Score};" +
                             $"{aiResult.Time.ToString("0.000")};{bitboard.Occupancy[0]};{bitboard.Occupancy[1]};" +
                             $"{bitboard.GamePhase}";

                csvWriter.WriteLine(output);
            }
        }

        /// <summary>
        /// Writes csv header (column names) to the file.
        /// </summary>
        /// <param name="writer">The csv stream writer</param>
        private void WriteHeader(StreamWriter writer)
        {
            writer.WriteLine("Time;Best move;Total nodes;End nodes;Nodes per second;Time per node;Score;Time;" +
                             "White occ;Black occ;Game phase");
        }
    }
}
