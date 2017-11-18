namespace Proxima.Core.Evaluation
{
    public struct IncrementalEvaluationData
    {
        public int Material { get; set; }
        public int Position { get; set; }
        public int Castling { get; set; }

        public IncrementalEvaluationData(DetailedEvaluationData detailedEvaluationData)
        {
            Material = detailedEvaluationData.Material.Difference;
            Position = detailedEvaluationData.Position.Difference;
            Castling = detailedEvaluationData.Castling.Difference;
        }
    }
}
