namespace Proxima.Core.Evaluation
{
    /// <summary>
    /// Represents a container of the incremental evaluation data.
    /// </summary>
    public class IncrementalEvaluationData
    {
        /// <summary>
        /// Gets or sets the incremental material evaluation result.
        /// </summary>
        public int Material { get; set; }

        /// <summary>
        /// Gets or sets the incremental position evaluation result.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the incremental castling evaluation result.
        /// </summary>
        public int Castling { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncrementalEvaluationData"/> class.
        /// </summary>
        /// <param name="incrementalEvaluationData">The incremental evaluation data container.</param>
        public IncrementalEvaluationData(IncrementalEvaluationData incrementalEvaluationData)
        {
            Material = incrementalEvaluationData.Material;
            Position = incrementalEvaluationData.Position;
            Castling = incrementalEvaluationData.Castling;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncrementalEvaluationData"/> class.
        /// </summary>
        /// <param name="detailedEvaluationData">The detailed evaluation data container.</param>
        public IncrementalEvaluationData(DetailedEvaluationData detailedEvaluationData)
        {
            Material = detailedEvaluationData.Material.Difference;
            Position = detailedEvaluationData.Position.Difference;
            Castling = detailedEvaluationData.Castling.Difference;
        }
    }
}
