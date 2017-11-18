namespace Proxima.Core.Evaluation
{
    public struct IncrementalEvaluationData
    {
        public int Material { get; set; }
        public int Position { get; set; }

        public IncrementalEvaluationData(IncrementalEvaluationData incrementalEvaluation)
        {
            Material = incrementalEvaluation.Material;
            Position = incrementalEvaluation.Position;
        }

        public IncrementalEvaluationData(DetailedEvaluationData detailedEvaluationData)
        {
            Material = detailedEvaluationData.Material.Difference;
            Position = detailedEvaluationData.Position.Difference;
        }
    }
}
