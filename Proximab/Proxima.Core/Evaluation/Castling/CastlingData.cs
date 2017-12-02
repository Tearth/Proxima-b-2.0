namespace Proxima.Core.Evaluation.Castling
{
    /// <summary>
    /// Represents a container of the castling evaluation data.
    /// </summary>
    public class CastlingData
    {
        /// <summary>
        /// Gets or sets the white castling evaluation result.
        /// </summary>
        public int WhiteCastling { get; set; }

        /// <summary>
        /// Gets or sets the black castling evaluation result.
        /// </summary>
        public int BlackCastling { get; set; }

        /// <summary>
        /// Gets the difference between white and black evaluation results.
        /// </summary>
        public int Difference
        {
            get { return WhiteCastling - BlackCastling; }
        }
    }
}
