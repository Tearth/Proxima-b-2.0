namespace Proxima.Core.Evaluation.KingSafety
{
    /// <summary>
    /// Represents a container for the king safety evaluation data.
    /// </summary>
    public class KingSafetyData
    {
        /// <summary>
        /// Gets or sets the value of the pieces that are attacking neighbour fields of the black king.
        /// </summary>
        public int WhiteAttackedNeighbours { get; set; }

        /// <summary>
        /// Gets or sets the value of the pieces that are attacking neighbour fields of the white king 
        /// </summary>
        public int BlackAttackedNeighbours { get; set; }

        /// <summary>
        /// Gets the difference between white and black evaluation results.
        /// </summary>
        public int Difference
        {
            get { return WhiteAttackedNeighbours - BlackAttackedNeighbours; }
        }
    }
}
