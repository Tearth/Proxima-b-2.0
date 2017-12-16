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
    public class CsvWriter : LogBase
    {
        public CsvWriter(string directory) : base(directory)
        {
        }

        public void WriteLine(AIResult aiResult, Bitboard bitboard)
        {
            using (var csvWriter = OpenOrCreateFile(".csv"))
            {
                if(csvWriter.BaseStream.Length == 0)
                {
                    WriteHeader(csvWriter);
                }
                
                var output = $"{CurrentTime()},{aiResult.BestMove},{aiResult.Stats.TotalNodes}," +
                             $"{aiResult.Stats.EndNodes},{aiResult.NodesPerSecond},{aiResult.TimePerNode},{aiResult.Score}," +
                             $"{aiResult.Time.ToString("0.000")},{bitboard.Occupancy[0]},{bitboard.Occupancy[1]}," +
                             $"{bitboard.GamePhase}";

                csvWriter.WriteLine(output);
            }
        }

        private void WriteHeader(StreamWriter writer)
        {
            writer.WriteLine("sep=,");
            writer.WriteLine("Time,Best move,Total nodes,End nodes,Nodes per second,Time per node,Score,Time," +
                             "White occ,Black occ,Game phase");
        }
    }
}
