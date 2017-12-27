namespace Proxima.Core.Evaluation.Mobility
{
    /// <summary>
    /// Represents a container of the mobility evaluation data.
    /// </summary>
    public class MobilityData
    {
        /// <summary>
        /// Gets or sets the white mobility evaluation result.
        /// </summary>
        public int WhiteMobility { get; set; }

        /// <summary>
        /// Gets or sets the black mobility evaluation result.
        /// </summary>
        public int BlackMobility { get; set; }

        /// <summary>
        /// Gets the difference between white and black evaluation results.
        /// </summary>
        public int Difference => WhiteMobility - BlackMobility;
    }
}
