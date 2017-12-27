namespace Proxima.Core.Evaluation.Material
{
    /// <summary>
    /// Represents a container of the material evaluation data.
    /// </summary>
    public class MaterialData
    {
        /// <summary>
        /// Gets or sets the white material evaluation result.
        /// </summary>
        public int WhiteMaterial { get; set; }

        /// <summary>
        /// Gets or sets the black material evaluation result.
        /// </summary>
        public int BlackMaterial { get; set; }

        /// <summary>
        /// Gets the difference between white and black evaluation results.
        /// </summary>
        public int Difference => WhiteMaterial - BlackMaterial;
    }
}
