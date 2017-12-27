namespace Proxima.Core.Evaluation.PawnStructure
{
    /// <summary>
    /// Represents a container of the pawn structure evaluation data.
    /// </summary>
    public class PawnStructureData
    {
        /// <summary>
        /// Gets or sets the white doubled pawns evaluation result.
        /// </summary>
        public int WhiteDoubledPawns { get; set; }

        /// <summary>
        /// Gets or sets the black doubled pawns evaluation result.
        /// </summary>
        public int BlackDoubledPawns { get; set; }

        /// <summary>
        /// Gets or sets the white isolated pawns evaluation result.
        /// </summary>
        public int WhiteIsolatedPawns { get; set; }

        /// <summary>
        /// Gets or sets the black isolated pawns evaluation result.
        /// </summary>
        public int BlackIsolatedPawns { get; set; }

        /// <summary>
        /// Gets or sets the white pawn chain evaluation result.
        /// </summary>
        public int WhitePawnChain { get; set; }

        /// <summary>
        /// Gets or sets the black pawn chain evaluation result.
        /// </summary>
        public int BlackPawnChain { get; set; }

        /// <summary>
        /// Gets the difference between white and black evaluation results.
        /// </summary>
        public int Difference => (WhiteDoubledPawns - BlackDoubledPawns) + 
                                 (WhiteIsolatedPawns - BlackIsolatedPawns) + 
                                 (WhitePawnChain - BlackPawnChain);
    }
}
