using System;

namespace Proxima.Core.Evaluation
{
    public class IncrementalEvaluationData
    {
        public int Material { get; set; }
        public int Position { get; set; }
        public int Castling { get; set; }

        public IncrementalEvaluationData()
        {
        }

        public IncrementalEvaluationData(IncrementalEvaluationData incrementalEvaluationData)
        {
            Material = incrementalEvaluationData.Material;
            Position = incrementalEvaluationData.Position;
            Castling = incrementalEvaluationData.Castling;
        }

        public IncrementalEvaluationData(DetailedEvaluationData detailedEvaluationData)
        {
            Material = detailedEvaluationData.Material.Difference;
            Position = detailedEvaluationData.Position.Difference;
            Castling = detailedEvaluationData.Castling.Difference;
        }
    }
}
