namespace Proxima.Core.Evaluation
{
    public struct IncrementalEvaluationData
    {
        public int Material { get; set; }

        public void Set(DetailedEvaluationData detailedEvaluationData)
        {
            Material = detailedEvaluationData.Material.Difference;
        }
    }
}
