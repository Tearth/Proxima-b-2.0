namespace Proxima.Core.Evaluation.Position
{
    /// <summary>
    /// Represents a container of the position evaluation data.
    /// </summary>
    public class PositionData
    {
        /// <summary>
        /// Gets or sets the white position evaluation result.
        /// </summary>
        public int WhitePosition { get; set; }

        /// <summary>
        /// Gets or sets the black position evaluation result.
        /// </summary>
        public int BlackPosition { get; set; }

        /// <summary>
        /// Gets the difference between white and black evaluation results.
        /// </summary>
        public int Difference
        {
            get { return WhitePosition - BlackPosition; }
        }
    }
}
