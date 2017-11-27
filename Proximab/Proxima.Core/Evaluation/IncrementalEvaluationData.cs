using System;

namespace Proxima.Core.Evaluation
{
    public class IncrementalEvaluationData : ICloneable
    {
        public int Material { get; set; }
        public int Position { get; set; }
        public int Castling { get; set; }

        public IncrementalEvaluationData()
        {
        }

        public IncrementalEvaluationData(DetailedEvaluationData detailedEvaluationData)
        {
            Material = detailedEvaluationData.Material.Difference;
            Position = detailedEvaluationData.Position.Difference;
            Castling = detailedEvaluationData.Castling.Difference;
        }

        public object Clone()
        {
            return new IncrementalEvaluationData()
            {
                Material = Material,
                Position = Position,
                Castling = Castling
            };
        }
    }
}
