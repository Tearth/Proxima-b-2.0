using Proxima.Core.Evaluation.Castling;
using Proxima.Core.Evaluation.KingSafety;
using Proxima.Core.Evaluation.Material;
using Proxima.Core.Evaluation.Mobility;
using Proxima.Core.Evaluation.PawnStructure;
using Proxima.Core.Evaluation.Position;

namespace Proxima.Core.Evaluation
{
    /// <summary>
    /// Represents a container of the evaluation data.
    /// </summary>
    public class DetailedEvaluationData
    {
        /// <summary>
        /// Gets or sets the material evaluation data.
        /// </summary>
        public MaterialData Material { get; set; }

        /// <summary>
        /// Gets or sets the mobility evaluation data.
        /// </summary>
        public MobilityData Mobility { get; set; }

        /// <summary>
        /// Gets or sets the castling evaluation data.
        /// </summary>
        public CastlingData Castling { get; set; }

        /// <summary>
        /// Gets or sets the position evaluation data.
        /// </summary>
        public PositionData Position { get; set; }

        /// <summary>
        /// Gets or sets the pawn structure evaluation data.
        /// </summary>
        public PawnStructureData PawnStructure { get; set; }

        /// <summary>
        /// Gets or sets the king safety evaluation data.
        /// </summary>
        public KingSafetyData KingSafety { get; set; }

        /// <summary>
        /// Gets the total difference between all evaluation results.
        /// </summary>
        public int Total
        {
            get
            {
                return Material.Difference +
                       Mobility.Difference +
                       Castling.Difference +
                       Position.Difference +
                       PawnStructure.Difference + 
                       KingSafety.Difference;
            }
        }
    }
}
