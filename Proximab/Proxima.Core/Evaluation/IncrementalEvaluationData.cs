using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Evaluation
{
    /// <summary>
    /// Represents a container of the incremental evaluation data.
    /// </summary>
    public class IncrementalEvaluationData
    {
        /// <summary>
        /// Gets or sets the white incremental material evaluation result.
        /// </summary>
        public int WhiteMaterial { get; set; }

        /// <summary>
        /// Gets or sets the black incremental material evaluation result.
        /// </summary>
        public int BlackMaterial { get; set; }

        public int Material => WhiteMaterial - BlackMaterial;

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
            WhiteMaterial = incrementalEvaluationData.WhiteMaterial;
            BlackMaterial = incrementalEvaluationData.BlackMaterial;
            Position = incrementalEvaluationData.Position;
            Castling = incrementalEvaluationData.Castling;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncrementalEvaluationData"/> class.
        /// </summary>
        /// <param name="detailedEvaluationData">The detailed evaluation data container.</param>
        public IncrementalEvaluationData(DetailedEvaluationData detailedEvaluationData)
        {
            WhiteMaterial = detailedEvaluationData.Material.WhiteMaterial;
            BlackMaterial = detailedEvaluationData.Material.BlackMaterial;
            Position = detailedEvaluationData.Position.Difference;
            Castling = detailedEvaluationData.Castling.Difference;
        }
    }
}
